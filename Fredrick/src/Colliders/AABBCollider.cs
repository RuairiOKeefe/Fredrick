using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework.Content;

namespace Fredrick.src
{
	[Serializable]
	public class AABBCollider : Component
	{
		private int _index;//index in ColliderManager
		private RectangleF _rectangle;

		private Vector2 _tempMove;

		private List<int[]> cells;

		private int minX, maxX, minY, maxY;

		public RectangleF Rectangle
		{
			get { return _rectangle; }
			set { _rectangle = value; }
		}

		private bool _platformCollided;

		public AABBCollider()
		{
			_owner = null;
			Position = new Vector2(0);
		}

		public AABBCollider(Entity owner, Vector2 position, float width = 1.0f, float height = 1.0f) : base(owner)
		{
			_rectangle = new RectangleF(position, width, height);
		}

		public bool CheckCollision(RectangleF other)
		{
			bool collided = false;

			Vector2 testMove = new Vector2(_tempMove.X, 0);
			Vector2 newPos = (_owner.Position + testMove);

			_rectangle.UpdatePosition(newPos);
			if (_rectangle.Intersect(other))
			{
				float distanceX = _rectangle.CurrentPosition.X - other.CurrentPosition.X;
				float minDistanceX = (_rectangle.Width + other.Width) / 2;

				if (distanceX > 0)
					_tempMove.X += (minDistanceX - distanceX) * 1.05f;//For the record I hate this but without it weird stuff happens because numbers are the worst and binary was a mistake
				else
					_tempMove.X += (-minDistanceX - distanceX) * 1.05f;

				_owner.GetComponent<Character>().StopVelX();
				collided = true;
			}

			testMove = new Vector2(0, _tempMove.Y);
			newPos = (_owner.Position + testMove);

			_rectangle.UpdatePosition(newPos);
			if (_rectangle.Intersect(other))
			{
				float distanceY = _rectangle.CurrentPosition.Y - other.CurrentPosition.Y;
				float minDistanceY = (_rectangle.Height + other.Height) / 2;

				if (distanceY > 0)
					_tempMove.Y += (minDistanceY - distanceY) * 1.05f;
				else
					_tempMove.Y += (-minDistanceY - distanceY) * 1.05f;

				_owner.GetComponent<Character>().StopVelY();
				collided = true;
			}

			return collided;
		}

		public bool CheckCollision(Platform other)
		{
			bool collided = false;
			if (other.PlatformDepth < 0)
			{
				Vector2 testMove = new Vector2(_tempMove.X, _tempMove.Y);
				Vector2 newPos = (_owner.Position + testMove);

				_rectangle.UpdatePosition(newPos);

				if (_owner.GetComponent<Character>().Velocity.Y <= 0)
				{
					float f = (_rectangle.CurrentPosition.X - (other.CurrentPosition.X - (other.Width / 2))) / ((other.CurrentPosition.X + (other.Width / 2)) - (other.CurrentPosition.X - (other.Width / 2)));//(currentX - minX) / (maxX - minX)
					if (f > 0 && f < 1)
					{
						float y = ((other.LHeight * (1.0f - f)) + (other.RHeight * f)) + other.CurrentPosition.Y;//desired y coordinate

						if (((_rectangle.CurrentPosition.Y - (_rectangle.Height / 2)) > (y + other.PlatformDepth) && (_rectangle.CurrentPosition.Y - (_rectangle.Height / 2)) < y) || (_platformCollided && _owner.GetComponent<Character>().Velocity.Y <= 0))
						{
							_tempMove.Y += (y - (newPos.Y - (_rectangle.Height / 2)));
							_owner.GetComponent<Character>().StopVelY();
							collided = true;
						}
					}
				}
			}
			else
			{
				Vector2 testMove = new Vector2(_tempMove.X, _tempMove.Y);
				Vector2 newPos = (_owner.Position + testMove);
				{
					_rectangle.UpdatePosition(newPos);
					float f = (_rectangle.CurrentPosition.X - (other.CurrentPosition.X - (other.Width / 2))) / ((other.CurrentPosition.X + (other.Width / 2)) - (other.CurrentPosition.X - (other.Width / 2)));//(currentX - minX) / (maxX - minX)
					if (f > 0 && f < 1)
					{
						float y = ((other.LHeight * (1.0f - f)) + (other.RHeight * f)) + other.CurrentPosition.Y;

						if ((_rectangle.CurrentPosition.Y + (_rectangle.Height / 2)) > y && (_rectangle.CurrentPosition.Y + (_rectangle.Height / 2)) < y + other.PlatformDepth)
						{
							_tempMove.Y -= ((newPos.Y + (_rectangle.Height / 2)) - y);
							_owner.GetComponent<Character>().Velocity = (_owner.GetComponent<Character>().Velocity - (2f * Vector2.Dot(_owner.GetComponent<Character>().Velocity, other.Normal)) * other.Normal) * 0.5f;
							collided = true;
						}
					}
				}
			}

			return collided;
		}

		public void SetCells(bool addToScene)
		{
			minX = Math.Min(Math.Max((int)Math.Floor(_rectangle.CurrentPosition.X - (_rectangle.Width / 2) + 0.5), 0), 1000);
			maxX = Math.Min(Math.Max((int)Math.Floor(_rectangle.CurrentPosition.X + (_rectangle.Width / 2) + 0.5), 0), 1000);
			minY = Math.Min(Math.Max((int)Math.Floor(_rectangle.CurrentPosition.Y - (_rectangle.Height / 2) + 0.5), 0), 1000);
			maxY = Math.Min(Math.Max((int)Math.Floor(_rectangle.CurrentPosition.Y + (_rectangle.Height / 2) + 0.5), 0), 1000);

			for (int i = minX; i < maxX + 1; i++)
			{
				for (int j = minY; j < maxY + 1; j++)
				{
					if (addToScene)
						ColliderManager.Instance.Terrain[i, j].Add(_owner);
					cells.Add(new int[2] { i, j });
				}
			}
		}

		public void ClearCells(bool removeFromScene)
		{
			if (removeFromScene)
				foreach (int[] c in cells)
				{
					ColliderManager.Instance.Terrain[c[0], c[1]].Remove(_owner);
				}
			cells.Clear();
		}

		public override void Load(ContentManager content)
		{
			_rectangle.UpdatePosition(_owner.Position + Position);
			_index = ColliderManager.Instance.Colliders.Count;
			ColliderManager.Instance.Colliders.Add(this);

			Body body;
			PolygonShape box;
			Fixture fixture;

			body = new Body(ColliderManager.Instance.World, _owner.Position + Position, 0, BodyType.Static);
			if (_owner.GetComponent<Character>() != null)
			{
				body.BodyType = BodyType.Kinematic;
			}
			body.UserData = _owner;
			Vertices verts = new Vertices();
			verts.Add(_rectangle.Corners[0]);
			verts.Add(_rectangle.Corners[1]);
			verts.Add(_rectangle.Corners[2]);
			verts.Add(_rectangle.Corners[3]);
			box = new PolygonShape(verts, 1.0f);
			fixture = body.CreateFixture(box);

			cells = new List<int[]>();
			SetCells(true);
		}

		public override void Unload()
		{
			ClearCells(true);
			foreach (Body b in ColliderManager.Instance.World.BodyList)
			{
				if (b.UserData == (object)_owner)
				{
					ColliderManager.Instance.World.RemoveBody(b);
				}
			}
		}

		public override void Update(double deltaTime)
		{
			if (_owner.GetComponent<Character>() != null)
			{
				_tempMove = _owner.GetComponent<Character>().AttemptedPosition;
				_rectangle.UpdatePosition(_owner.Position + _tempMove);

				ClearCells(true);
				SetCells(false);

				bool newPlatformCollided = false;
				foreach (int[] c in cells)
				{
					if (ColliderManager.Instance.Terrain[c[0], c[1]].Count != 0)
					{
						foreach (Entity e in ColliderManager.Instance.Terrain[c[0], c[1]])
						{
							if (e.GetComponent<AABBCollider>() != null)
								if (CheckCollision(e.GetComponent<AABBCollider>().Rectangle))
								{
								}
							if (e.GetComponent<Platform>() != null)
								if (CheckCollision(e.GetComponent<Platform>()))
								{
									newPlatformCollided = true;
								}
						}
					}
				}
				_platformCollided = newPlatformCollided;

				_owner.Move(_tempMove);
				foreach (Body b in ColliderManager.Instance.World.BodyList)
				{
					if (b.UserData == (object)_owner)
					{
						b.Position = _owner.Position + Position;
					}
				}
				ClearCells(false);

				_rectangle.UpdatePosition(_owner.Position + _tempMove);
				SetCells(true);
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{

		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{
			Vector2 tl = new Vector2(_owner.Position.X - _rectangle.Width / 2, _owner.Position.Y + _rectangle.Height / 2);
			Vector2 tr = new Vector2(_owner.Position.X + _rectangle.Width / 2, _owner.Position.Y + _rectangle.Height / 2);
			Vector2 bl = new Vector2(_owner.Position.X - _rectangle.Width / 2, _owner.Position.Y - _rectangle.Height / 2);
			Vector2 br = new Vector2(_owner.Position.X + _rectangle.Width / 2, _owner.Position.Y - _rectangle.Height / 2);
			DebugManager.Instance.DrawLine(spriteBatch, tl, tr);
			DebugManager.Instance.DrawLine(spriteBatch, tr, br);
			DebugManager.Instance.DrawLine(spriteBatch, br, bl);
			DebugManager.Instance.DrawLine(spriteBatch, bl, tl);
		}

		public override Component Copy(Entity owner)
		{
			return new AABBCollider();
		}
	}
}

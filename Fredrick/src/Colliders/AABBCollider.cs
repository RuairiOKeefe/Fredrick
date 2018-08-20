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

namespace Fredrick.src
{
	public class AABBCollider : Component
	{
		int _index;//index in ColliderManager
		RectangleF _rectangle;

		Vector2 tempMove;


		Body _body;
		PolygonShape _box;
		Fixture _fixture;

		public RectangleF Rectangle
		{
			get { return _rectangle; }
			set { _rectangle = value; }
		}

		public Body Body
		{
			get { return _body; }
			set { _body = value; }
		}


		public AABBCollider(Entity owner) : base(owner)
		{
			_rectangle = new RectangleF(new Vector2(0), 1, 1, 0.0f, 0.0f);
			_rectangle.UpdatePosition(_owner.GetPosition());
			_index = ColliderManager.Instance.Colliders.Count;
			ColliderManager.Instance.Colliders.Add(this);

			_body = new Body(ColliderManager.Instance.World, _owner.GetPosition() + _position, 0, BodyType.Static);
			if (_owner.GetComponent<Character>() != null)
			{
				_body.BodyType = BodyType.Kinematic;
			}
			_body.UserData = _owner;
			Vertices verts = new Vertices();
			verts.Add(_rectangle.Corners[0]);
			verts.Add(_rectangle.Corners[1]);
			verts.Add(_rectangle.Corners[2]);
			verts.Add(_rectangle.Corners[3]);
			_box = new PolygonShape(verts, 1.0f);
			_fixture = _body.CreateFixture(_box);
		}

		public void CheckCollision(RectangleF other)
		{
			Vector2 testMove = new Vector2(tempMove.X, 0);
			Vector2 newPos = ((_owner.GetPosition() + testMove));

			_rectangle.UpdatePosition(newPos);
			if (_rectangle.Intersect(other))
			{
				float distanceX = _rectangle.CurrentPosition.X - other.CurrentPosition.X;
				float minDistanceX = (_rectangle.Width + other.Width) / 2;

				if (distanceX > 0)
					tempMove.X += (minDistanceX - distanceX) * 1.05f;//For the record I hate this but without it weird stuff happens because numbers are the worst and binary was a mistake
				else
					tempMove.X += (-minDistanceX - distanceX) * 1.05f;

				_owner.GetComponent<Character>().StopVelX();
			}

			testMove = new Vector2(0, tempMove.Y);
			newPos = ((_owner.GetPosition() + testMove));

			_rectangle.UpdatePosition(newPos);
			if (_rectangle.Intersect(other))
			{
				float distanceY = _rectangle.CurrentPosition.Y - other.CurrentPosition.Y;
				float minDistanceY = (_rectangle.Height + other.Height) / 2;

				if (distanceY > 0)
					tempMove.Y += (minDistanceY - distanceY) * 1.05f;
				else
					tempMove.Y += (-minDistanceY - distanceY) * 1.05f;

				_owner.GetComponent<Character>().StopVelY();
			}
		}

		public void CheckCollision(Platform other)
		{
			if (other.PlatformDepth < 0)
			{
				Vector2 testMove = new Vector2(tempMove.X, 0);
				Vector2 newPos = ((_owner.GetPosition() + testMove));

				_rectangle.UpdatePosition(newPos);
				float f = (_rectangle.CurrentPosition.X - (other.CurrentPosition.X - (other.Width / 2))) / ((other.CurrentPosition.X + (other.Width / 2)) - (other.CurrentPosition.X - (other.Width / 2)));//(currentX - minX) / (maxX - minX)
				if (f > 0 && f < 1)
				{
					float y = ((other.LHeight * (1.0f - f)) + (other.RHeight * f)) + other.CurrentPosition.Y;//desired y coordinate

					if ((_rectangle.CurrentPosition.Y - (_rectangle.Height / 2)) > (y + other.PlatformDepth) && (_rectangle.CurrentPosition.Y - (_rectangle.Height / 2)) < y && _owner.GetComponent<Character>().PrevGrounded)
					{
						//correct postion but leave y velocity intact (only for x move)
						tempMove.Y += (y - (_rectangle.CurrentPosition.Y - (_rectangle.Height / 2)));
					}
				}

				testMove = new Vector2(tempMove.X, tempMove.Y);
				newPos = ((_owner.GetPosition() + testMove));

				_rectangle.UpdatePosition(newPos);
				f = (_rectangle.CurrentPosition.X - (other.CurrentPosition.X - (other.Width / 2))) / ((other.CurrentPosition.X + (other.Width / 2)) - (other.CurrentPosition.X - (other.Width / 2)));//(currentX - minX) / (maxX - minX)
				if (f > 0 && f < 1)
				{
					float y = ((other.LHeight * (1.0f - f)) + (other.RHeight * f)) + other.CurrentPosition.Y;//desired y coordinate

					if ((_rectangle.CurrentPosition.Y - (_rectangle.Height / 2)) > (y + other.PlatformDepth) && (_rectangle.CurrentPosition.Y - (_rectangle.Height / 2)) < y + 1 && _owner.GetComponent<Character>().Grounded && _owner.GetComponent<Character>().Velocity.Y < 0)
					{
						tempMove.Y += (y - (_rectangle.CurrentPosition.Y - (_rectangle.Height / 2)));
						_owner.GetComponent<Character>().StopVelY();
					}
				}
			}
			else
			{
				Vector2 testMove = new Vector2(tempMove.X, tempMove.Y);
				Vector2 newPos = ((_owner.GetPosition() + testMove));
				{
					_rectangle.UpdatePosition(newPos);
					float f = (_rectangle.CurrentPosition.X - (other.CurrentPosition.X - (other.Width / 2))) / ((other.CurrentPosition.X + (other.Width / 2)) - (other.CurrentPosition.X - (other.Width / 2)));//(currentX - minX) / (maxX - minX)
					if (f > 0 && f < 1)
					{
						float y = ((other.LHeight * (1.0f - f)) + (other.RHeight * f)) + other.CurrentPosition.Y;

						if ((_rectangle.CurrentPosition.Y + (_rectangle.Height / 2)) > y && (_rectangle.CurrentPosition.Y + (_rectangle.Height / 2)) < y + other.PlatformDepth)
						{
							tempMove.Y -= ((_rectangle.CurrentPosition.Y + (_rectangle.Height / 2)) - y);
							_owner.GetComponent<Character>().Velocity = (_owner.GetComponent<Character>().Velocity - (2f * Vector2.Dot(_owner.GetComponent<Character>().Velocity, other.Normal)) * other.Normal) * 0.5f;
						}
					}
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 tl = new Vector2(_owner.GetPosition().X - _rectangle.Width / 2, _owner.GetPosition().Y + _rectangle.Height / 2);
			Vector2 tr = new Vector2(_owner.GetPosition().X + _rectangle.Width / 2, _owner.GetPosition().Y + _rectangle.Height / 2);
			Vector2 bl = new Vector2(_owner.GetPosition().X - _rectangle.Width / 2, _owner.GetPosition().Y - _rectangle.Height / 2);
			Vector2 br = new Vector2(_owner.GetPosition().X + _rectangle.Width / 2, _owner.GetPosition().Y - _rectangle.Height / 2);
			DebugManager.Instance.DrawLine(spriteBatch, tl, tr);
			DebugManager.Instance.DrawLine(spriteBatch, tr, br);
			DebugManager.Instance.DrawLine(spriteBatch, br, bl);
			DebugManager.Instance.DrawLine(spriteBatch, bl, tl);
		}

		public override void Update(double deltaTime)
		{
			if (_owner.GetComponent<Character>() != null)
			{
				tempMove = _owner.GetComponent<Character>().AttemptedPosition;

				foreach (Platform p in ColliderManager.Instance.Platforms)
				{
					if (p.GetOwner() != _owner)
						CheckCollision(p);
				}

				foreach (AABBCollider c in ColliderManager.Instance.Colliders)
				{
					if (c.GetOwner() != _owner)
						CheckCollision(c.Rectangle);
				}
				_owner.Move(tempMove);
				_body.Position = _owner.GetPosition() + _position;
			}
		}
	}
}

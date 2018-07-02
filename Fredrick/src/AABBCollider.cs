using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	class AABBCollider : Component
	{
		int _index;//index in ColliderManager
		RectangleF _rectangle;

		Vector2 move;
		Vector2 tempMove;

		public AABBCollider(Entity owner) : base(owner)
		{
			_rectangle = new RectangleF(new Entity(), new Vector2(0), 1, 1, 0.0f, 0.0f);
			_rectangle.UpdatePosition(_owner.GetPosition());
			_index = ColliderManager.Instance.Colliders.Count;
			ColliderManager.Instance.Colliders.Add(_rectangle);
		}

		public void CheckCollision(RectangleF other)
		{
			Vector2 testMove = new Vector2(tempMove.X, 0);
			Vector2 newPos = ((_owner.GetPosition() + testMove));

			_rectangle.UpdatePosition(newPos);
			if (_rectangle.Intersect(other))
			{
				float distanceX = _rectangle.Position.X - other.Position.X;
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
				float distanceY = _rectangle.Position.Y - other.Position.Y;
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
			Vector2 testMove = new Vector2(tempMove.X, 0);
			Vector2 newPos = ((_owner.GetPosition() + testMove));

			_rectangle.UpdatePosition(newPos);
			float f = (_rectangle.Position.X - (other.Position.X - (other.Width / 2))) / ((other.Position.X + (other.Width / 2)) - (other.Position.X - (other.Width / 2)));//(currentX - minX) / (maxX - minX)
			Debug.Print(f.ToString());
			if (f > 0 && f < 1)
			{
				float y = ((other.LHeight * (1.0f - f)) + (other.RHeight * f)) + other.Position.Y;//desired y coordinate

				if ((_rectangle.Position.Y - (_rectangle.Height / 2)) < y && (_rectangle.Position.Y - (_rectangle.Height / 2)) > (y - other.PlatformDepth))
				{
					tempMove.Y += (y - (_rectangle.Position.Y - (_rectangle.Height / 2))) * 1.05f;
					if (_owner.GetComponent<Character>().Velocity.Y < 0)
						_owner.GetComponent<Character>().StopVelY();
				}
			}

			testMove = new Vector2(0, tempMove.Y);
			newPos = ((_owner.GetPosition() + testMove));

			_rectangle.UpdatePosition(newPos);
			f = (_rectangle.Position.X - (other.Position.X - (other.Width / 2))) / ((other.Position.X + (other.Width / 2)) - (other.Position.X - (other.Width / 2)));//(currentX - minX) / (maxX - minX)
			Debug.Print(f.ToString());
			if (f > 0 && f < 1)
			{
				float y = ((other.LHeight * (1.0f - f)) + (other.RHeight * f)) + other.Position.Y;//desired y coordinate

				if ((_rectangle.Position.Y - (_rectangle.Height / 2)) < y && (_rectangle.Position.Y - (_rectangle.Height / 2)) > (y - other.PlatformDepth))
				{
					tempMove.Y += (y - (_rectangle.Position.Y - (_rectangle.Height / 2))) * 1.05f;
					if (_owner.GetComponent<Character>().Velocity.Y < 0)
						_owner.GetComponent<Character>().StopVelY();
				}
			}
		}
		public override void Draw(SpriteBatch spriteBatch)
		{

		}

		public override void Update(double deltaTime)
		{
			if (_owner.GetComponent<Character>() != null)
			{
				move = _owner.GetComponent<Character>().GetMove();
				tempMove = move;

				foreach (Platform p in ColliderManager.Instance.Platforms)
				{
					CheckCollision(p);
				}

				int n = 0;
				foreach (RectangleF r in ColliderManager.Instance.Colliders)
				{
					if (_index != n)
						CheckCollision(r);
					n++;
				}
				_owner.Move(tempMove);
			}
		}
	}
}

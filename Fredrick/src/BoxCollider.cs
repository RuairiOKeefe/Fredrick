using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	class BoxCollider : Collider
	{
		RectangleF _rectangle;

		public BoxCollider(Entity owner) : base(owner)
		{
			_rectangle = new RectangleF(new Vector2(0), 1, 1, 1, 1);
		}

		public bool CheckCollision()
		{
			return false;
		}

		public override void CheckCollision(Collider other)
		{
			if (other.GetType() == typeof(BoxCollider))
			{
				CheckCollision((BoxCollider)other);
			}
		}

		public void CheckCollision(BoxCollider other)
		{
			Vector2 move = _owner.GetComponent<Character>().GetMove();
			Vector2 tempMove = move;
			for (int i = 0; i < 2; i++)
			{
				if (i == 0)
				{
					//if i = 0 try horizontal collision,
					Vector2 testMove = new Vector2(move.X, 0);
					Vector2 newPos = ((_owner.GetPosition() + GetPosition() + testMove));

					_rectangle.UpdatePosition(newPos);
					if (_rectangle.Intersect(other._rectangle))
					{
						if (_owner.GetComponent<Character>().Velocity.X > 0)
						{
							tempMove.X -= _rectangle.GetMax().X - other._rectangle.GetMin().X;
						}
						else
							if (_owner.GetComponent<Character>().Velocity.X < 0)
						{
							tempMove.X -= _rectangle.GetMin().X - other._rectangle.GetMax().X;
						}
						_owner.GetComponent<Character>().StopVelX();
					}
				}
				else
				{
					Vector2 testMove = new Vector2(0, move.Y);
					Vector2 newPos = ((_owner.GetPosition() + GetPosition() + testMove));

					_rectangle.UpdatePosition(newPos);
					if (_rectangle.Intersect(other._rectangle))
					{
						if (_owner.GetComponent<Character>().Velocity.Y > 0)
						{
							tempMove.Y -= _rectangle.GetMin().Y - other._rectangle.GetMax().Y;
						}
						else
							if (_owner.GetComponent<Character>().Velocity.Y < 0)
						{
							tempMove.Y -= _rectangle.GetMax().Y - other._rectangle.GetMin().Y;
						}
						_owner.GetComponent<Character>().StopVelY();
					}
				}
				_owner.Move(tempMove);
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{

		}

		public override void Update(double deltaTime)
		{
			BoxCollider a = new BoxCollider(_owner);
			a._rectangle = new RectangleF(new Vector2(3, 0), 1, 1, 0, 0);
			a._rectangle.UpdatePosition(new Vector2(3, 0));
			CheckCollision(a);
		}
	}
}

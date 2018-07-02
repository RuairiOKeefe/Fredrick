using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fredrick.src
{
	internal class Character : Movable
	{




		public Character(Entity owner) : base(owner)
		{
			_velocity = new Vector2(0, 0);
			_acceleration = new Vector2(0, 0);
			_horAcc = 30;
			_maxSpeed = 5;
			_acceleration.Y = -9.8f;

			_friction = 100;
		}

		public void Move(double deltaTime)
		{
			float move = InputHandler.Instance.MoveX;
			if (move != 0)
			{
				_acceleration.X = _horAcc * move;
				_friction = 100;
				if ((_velocity.X * _velocity.X) > (_maxSpeed * _maxSpeed))
				{
					_acceleration.X = 0;
				}
			}
			else
			{
				_acceleration.X = 0;
				_friction = 600;
			}


			if (InputHandler.Instance.IsKeyPressed(InputHandler.Action.Jump))
			{
				_velocity.Y = 10;
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
		}

		public override void Update(double deltaTime)
		{
			Move(deltaTime);
			ResolveMotion(deltaTime);
		}
	}
}

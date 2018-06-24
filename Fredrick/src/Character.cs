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
			_horAcc = 100;
			_maxSpeed = 10;
			//_acceleration.Y = 9.8f;//scaled currently due to the coordinate system used

			_friction = 100;
		}

		public void Move(double deltaTime)
		{
			float move = InputHandler.Instance.MoveX;
			if (move != 0)
			{
				_acceleration.X = _horAcc * move;
				_friction = 0;
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

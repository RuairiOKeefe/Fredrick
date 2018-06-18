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
	internal class Character : Component
	{
		Vector2 _velocity;
		Vector2 _acceleration;
		float _horAcc;
		float _maxSpeed;

		float _friction;

		public Character(Entity owner) : base(owner)
		{
			_velocity = new Vector2(0, 0);
			_acceleration = new Vector2(0, 0);
			_horAcc = 500;
			_maxSpeed = 200;
			//_acceleration.Y = 9.8f * 10;//scaled currently due to the coordinate system used

			_friction = 500;
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
			float tempAccX = _acceleration.X - (_friction * _velocity.X * (float)deltaTime);

			if (move == 0)
				if (_velocity.X > 0)
				{
					if (_velocity.X + tempAccX * (float)deltaTime < 0)
						_velocity.X = 0;
					else
						_velocity.X += tempAccX * (float)deltaTime;
				}
				else
				{
					if (_velocity.X < 0)
						if (_velocity.X + tempAccX * (float)deltaTime > 0)
							_velocity.X = 0;
						else
							_velocity.X += tempAccX * (float)deltaTime;
				}
			else
				_velocity.X += tempAccX * (float)deltaTime;

			_velocity.Y += _acceleration.Y * (float)deltaTime;
			_owner.Move(Vector2.Multiply(_velocity, (float)deltaTime));
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
		}

		public override void Update(double deltaTime)
		{
			Move(deltaTime);
		}
	}
}

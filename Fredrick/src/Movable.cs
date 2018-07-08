using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class Movable : Component
	{
		protected Vector2 _attemptedPosition;//The location the entity wants to move to
		protected Vector2 _velocity;
		protected Vector2 _acceleration;
		protected float _horAcc;
		protected float _maxSpeed;

		protected float _friction;

		public Movable(Entity owner) : base(owner)
		{

		}

		public Vector2 AttemptedPosition
		{
			get { return _attemptedPosition; }
			set { _attemptedPosition = value; }
		}

		public Vector2 Velocity
		{
			get { return _velocity; }
			set { _velocity = value; }
		}

		public Vector2 Acceleration
		{
			get { return _acceleration; }
			set { _acceleration = value; }
		}

		public void StopVelX()
		{
			_velocity.X = 0;
		}

		public void StopVelY()
		{
			_velocity.Y = 0;
		}

		public void ResolveMotion(double deltaTime)
		{
			float tempAccX = _acceleration.X - (_friction * _velocity.X * (float)deltaTime);

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
				else
					_velocity.X += tempAccX * (float)deltaTime;
			}

			_velocity.Y += _acceleration.Y * (float)deltaTime;
			_attemptedPosition = Vector2.Multiply(_velocity, (float)deltaTime);

			if (_owner.GetComponent<AABBCollider>() == null)
				_owner.Move(_attemptedPosition);//If this does not contain a collider just move it because nothing will stop it.
		}

		public override void Draw(SpriteBatch spriteBatch)
		{

		}

		public override void Update(double deltaTime)
		{

		}
	}
}

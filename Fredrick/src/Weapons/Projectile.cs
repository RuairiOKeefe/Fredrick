using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class Projectile : Component
	{
		Vector2 _velocity;
		double _lifeTime;
		double _halfpoint;
		float _opacity;

		public double LifeTime
		{
			get { return _lifeTime; }
			set { _lifeTime = value; }
		}

		public float Opacity
		{
			get { return _opacity; }
		}

		public Projectile(Entity owner) : base(owner)
		{
			_position = new Vector2();
			_velocity = new Vector2();
		}

		public Projectile(Entity owner, Vector2 position, Vector2 velocity, double lifeTime) : base(owner)
		{
			_position = position;
			_velocity = velocity;
			_lifeTime = lifeTime;
		}

		public void Revive(Vector2 position, Vector2 velocity, double lifeTime)
		{
			_position = position;
			_velocity = velocity;
			_lifeTime = lifeTime;
			_halfpoint = lifeTime / 2;
			_opacity = 1;
		}

		public void Update(double deltaTime, Vector2 acceleration)
		{
			_velocity += acceleration * (float)deltaTime;
			_position += _velocity * (float)deltaTime;
			if (_velocity.Length() > 0)
			{
				Vector2 v = _velocity;
				v.Normalize();
				_rotation = (float)Math.Atan2(-v.Y, v.X);
			}
			else
			{
				_rotation = 0;
			}
			_lifeTime -= deltaTime;
			if (_lifeTime < _halfpoint)
			{
				_opacity = (float)(_lifeTime / _halfpoint);
			}
			else
			{
				_opacity = 1;
			}
		}

		public override void Update(double deltaTime)
		{
			
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			
		}
	}
}

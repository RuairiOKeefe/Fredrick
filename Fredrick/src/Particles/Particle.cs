using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public class Particle
	{
		//need to add collision logic and animation code
		Vector2 _position;
		float _rotation;
		Vector2 _velocity;
		double _lifeTime;
		double _halfpoint;
		float _opacity;
	
		public Vector2 Position
		{
			get { return _position; }
			set { _position = value; }
		}

		public float Rotation
		{
			get { return _rotation; }
		}

		public double LifeTime
		{
			get { return _lifeTime; }
			set { _lifeTime = value; }
		}

		public float Opacity
		{
			get { return _opacity; }
		}

		public Particle()
		{
			_position = new Vector2();
			_velocity = new Vector2();
		}

		public Particle(Vector2 position, Vector2 velocity, double lifeTime)
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
	}
}

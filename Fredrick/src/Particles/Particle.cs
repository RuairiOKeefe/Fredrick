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
	
		public Vector2 Position
		{
			get { return _position; }
			set { _position = value; }
		}

		public double LifeTime
		{
			get { return _lifeTime; }
			set { _lifeTime = value; }
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
		}

		public void Update(double deltaTime, Vector2 acceleration)
		{
			_velocity += acceleration * (float)deltaTime;
			_position += _velocity * (float)deltaTime;

			_lifeTime -= deltaTime;
		}
	}
}

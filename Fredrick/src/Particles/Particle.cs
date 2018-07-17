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
		Vector2 _position;
		float _rotation;
		Vector2 _velocity;
	
		public Vector2 Position
		{
			get { return _position; }
			set { _position = value; }
		}

		public Particle(Vector2 position, Vector2 velocity)
		{
			_position = position;
			_velocity = velocity;
		}

		public void Update(double deltaTime, Vector2 acceleration)
		{
			_velocity += acceleration * (float)deltaTime;
			_position += _velocity * (float)deltaTime;
		}
	}
}

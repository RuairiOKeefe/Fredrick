using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Fredrick.src
{
	[Serializable]
	public class Transform
	{
		protected Vector2 _position;
		protected float _rotation;
		protected Vector2 _scale;

		public Vector2 Position
		{
			get { return _position; }
			set { _position = value; }
		}

		public float Rotation
		{
			get { return _rotation; }
			set { _rotation = value; }
		}

		public Vector2 Scale
		{
			get { return _scale; }
			set { _scale = value; }
		}

		public void Move(Vector2 offset)
		{
			_position += offset;
		}
	}
}

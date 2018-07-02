using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Fredrick.src
{
	public class Transform
	{
		protected Vector2 _position;
		protected float _rotation;
		protected Vector2 _scale;

		public Vector2 GetPosition()
		{
			return _position;
		}

		public void SetPosition(Vector2 position)
		{
			this._position = position;
		}

		public float GetRotation()
		{
			return _rotation;
		}

		public void SetRotation(float rotation)
		{
			this._rotation = rotation;
		}

		public Vector2 GetScale()
		{
			return _scale;
		}

		public void SetScale(Vector2 scale)
		{
			this._scale = scale;
		}

		public void Move(Vector2 offset)
		{
			_position += offset;
		}
	}
}

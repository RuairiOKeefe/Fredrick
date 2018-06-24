using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Fredrick.src
{
	class RectangleF
	{
		Vector2 _position;
		float _width;
		float _height;

		float _offsetX;
		float _offsetY;

		Vector2 _min;
		Vector2 _max;

		public RectangleF(Vector2 position, float width, float height, float offsetX, float offsetY)
		{
			_position = position;
			_width = width;
			_height = height;

			_offsetX = offsetX;
			_offsetY = offsetY;
		}

		public Vector2 GetMin()
		{
			return _min;
		}

		public Vector2 GetMax()
		{
			return _max;
		}

		public void UpdatePosition(Vector2 position)
		{
			_position = position + new Vector2(_offsetX, _offsetY);
			_min = _position;
			_max = _position + new Vector2(_width, _height);
		}

		public bool Intersect(RectangleF other)
		{
			if (_max.X < other._min.X) return false; // a is left of b
			if (_min.X > other._max.X) return false; // a is right of b
			if (_max.Y < other._min.Y) return false; // a is above b
			if (_min.Y > other._max.Y) return false; // a is below b
			return true; // boxes overlap
		}
	}
}

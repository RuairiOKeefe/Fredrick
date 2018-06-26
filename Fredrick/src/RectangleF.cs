using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Fredrick.src
{
	public class RectangleF
	{
		protected Vector2 _position;
		protected float _width;
		protected float _height;

		protected float _offsetX;
		protected float _offsetY;

		protected Vector2 _min;
		protected Vector2 _max;

		public float Width
		{
			get
			{
				return _width;
			}
			set
			{
				_width = value;
			}
		}

		public float Height
		{
			get
			{
				return _height;
			}
			set
			{
				_height = value;
			}
		}

		public Vector2 Position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
			}
		}

		public RectangleF(Vector2 position, float width, float height, float offsetX, float offsetY)
		{
			_position = position;
			_width = width;
			_height = height;

			_offsetX = offsetX;
			_offsetY = offsetY;

			_min = _position - new Vector2(_offsetX, _offsetY);
			_max = _position + new Vector2(_offsetX, _offsetY);
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
			_position = position;
			_min = _position - new Vector2(_offsetX, _offsetY);
			_max = _position + new Vector2(_offsetX, _offsetY);
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

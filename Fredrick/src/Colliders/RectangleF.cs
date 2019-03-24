using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	[Serializable]
	public class RectangleF : Transform
	{
		protected Vector2 _currentPosition;//current position of collider to be tested
		protected float _width;
		protected float _height;

		public Vector2[] Corners { get; set; } = new Vector2[4];

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

		public Vector2 CurrentPosition
		{
			get
			{
				return _currentPosition;
			}
			set
			{
				_currentPosition = value;
			}
		}

		public RectangleF()
		{
		}

		public RectangleF(Vector2 position, float width, float height)
		{
			Position = position;
			_width = width;
			_height = height;

			Corners = new Vector2[4];
			Corners[0] = new Vector2(-width / 2, height / 2);
			Corners[1] = new Vector2(width / 2, height / 2);
			Corners[2] = new Vector2(width / 2, -height / 2);
			Corners[3] = new Vector2(-width / 2, -height / 2);

		}

		public RectangleF(RectangleF original)
		{
			Position = original.Position;
			_width = original.Width;
			_height = original.Height;

			Corners = new Vector2[4];
			Corners[0] = new Vector2(-_width / 2, _height / 2);
			Corners[1] = new Vector2(_width / 2, _height / 2);
			Corners[2] = new Vector2(_width / 2, -_height / 2);
			Corners[3] = new Vector2(-_width / 2, -_height / 2);
		}


		public void UpdatePosition(Vector2 currentPosition)
		{
			_currentPosition = currentPosition + Position;
		}

		public bool Intersect(RectangleF other)
		{
			if (_currentPosition.X + (Width / 2) < other.CurrentPosition.X - (other.Width / 2)) return false; // a is left of b
			if (_currentPosition.X - (Width / 2) > other.CurrentPosition.X + (other.Width / 2)) return false; // a is right of b
			if (_currentPosition.Y + (Height / 2) < other.CurrentPosition.Y - (other.Height / 2)) return false; // a is above b
			if (_currentPosition.Y - (Height / 2) > other.CurrentPosition.Y + (other.Height / 2)) return false; // a is below b

			return true; // boxes overlap
		}

		public bool Intersect(Vector2 other)
		{
			if (other.X > _currentPosition.X + (Width / 2)) return false;
			if (other.X <  _currentPosition.X - (Width / 2)) return false; 
			if (other.Y >  _currentPosition.Y + (Height / 2)) return false;
			if (other.Y <  _currentPosition.Y - (Height / 2)) return false;

			return true;
		}
	}
}

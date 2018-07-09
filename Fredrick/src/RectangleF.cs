﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class RectangleF : Transform
	{
		protected Vector2 _currentPosition;//current position of collider to be tested
		protected float _width;
		protected float _height;

		protected float _offsetX;
		protected float _offsetY;

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

		public RectangleF(Vector2 position, float width, float height, float offsetX, float offsetY)
		{
			_position = position;
			_width = width;
			_height = height;

			_offsetX = offsetX;
			_offsetY = offsetY;
		}

		public void UpdatePosition(Vector2 currentPosition)
		{
			_currentPosition = currentPosition + _position + new Vector2(_offsetX, _offsetY);
		}

		public bool Intersect(RectangleF other)
		{
			if (_currentPosition.X + (Width / 2) < other.CurrentPosition.X - (other.Width / 2)) return false; // a is left of b
			if (_currentPosition.X - (Width / 2) > other.CurrentPosition.X + (other.Width / 2)) return false; // a is right of b
			if (_currentPosition.Y + (Height / 2) < other.CurrentPosition.Y - (other.Height / 2)) return false; // a is above b
			if (_currentPosition.Y - (Height / 2) > other.CurrentPosition.Y + (other.Height / 2)) return false; // a is below b

			return true; // boxes overlap
		}
	}
}
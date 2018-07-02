using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class RectangleF : Component
	{
		protected Vector2 _position;
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

		public RectangleF(Entity owner) : base(owner)
		{
		}

		public RectangleF(Entity owner, Vector2 position, float width, float height, float offsetX, float offsetY) : base(owner)
		{
			_position = position;
			_width = width;
			_height = height;

			_offsetX = offsetX;
			_offsetY = offsetY;
		}

		public void UpdatePosition(Vector2 position)
		{
			_position = position + new Vector2(_offsetX, _offsetY);
		}

		public bool Intersect(RectangleF other)
		{
			if (_position.X + (Width / 2) < other.Position.X - (other.Width / 2)) return false; // a is left of b
			if (_position.X - (Width / 2) > other.Position.X + (other.Width / 2)) return false; // a is right of b
			if (_position.Y + (Height / 2) < other.Position.Y - (other.Height / 2)) return false; // a is above b
			if (_position.Y - (Height / 2) > other.Position.Y + (other.Height / 2)) return false; // a is below b

			return true; // boxes overlap
		}

		public override void Update(double deltaTime)
		{
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
		}
	}
}

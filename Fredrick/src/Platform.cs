using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Fredrick.src
{
	public class Platform : RectangleF
	{
		protected float _lHeight;//Height of the surface of the leftmost point on the platform, should not excede height
		protected float _rHeight;//Height of the surface of the rightmost point on the platform, should not excede height
		protected float _platformDepth;
		

		public float LHeight
		{
			get
			{
				return _lHeight;
			}
			set
			{
				_lHeight = value;
			}
		}

		public float RHeight
		{
			get
			{
				return _rHeight;
			}
			set
			{
				_rHeight = value;
			}
		}

		public float PlatformDepth
		{
			get
			{
				return _platformDepth;
			}
			set
			{
				_platformDepth = value;
			}
		}

		public Platform(Vector2 position, float width, float height, float offsetX, float offsetY, float lHeight, float rHeight, float platformDepth)
		{
			_position = position;
			_width = width;
			_height = height;

			_offsetX = offsetX;
			_offsetY = offsetY;

			_lHeight = lHeight;
			_rHeight = rHeight;
			_platformDepth = platformDepth;
		}
	}
}

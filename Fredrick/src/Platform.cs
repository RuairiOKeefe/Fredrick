using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class Platform : Component
	{

		protected Vector2 _currentPosition;//current position of collider to be tested
		protected float _width;
		protected float _height;

		protected float _offsetX;
		protected float _offsetY;

		protected float _lHeight;//Height of the surface of the leftmost point on the platform, should not excede height
		protected float _rHeight;//Height of the surface of the rightmost point on the platform, should not excede height
		protected float _platformDepth;
		protected Vector2 _normal;

		protected Texture2D _lineTex;//A simple 1x1 texture to be used for line rendering in debug

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

		public Vector2 Normal
		{
			get
			{
				return _normal;
			}
			set
			{
				_normal = value;
			}
		}

		public Platform(Entity owner, Vector2 currentPosition, float width, float height, float offsetX, float offsetY, float lHeight, float rHeight, float platformDepth) : base(owner)
		{
			_currentPosition = currentPosition + owner.GetPosition();
			_width = width;
			_height = height;

			_offsetX = offsetX;
			_offsetY = offsetY;

			_lHeight = lHeight;
			_rHeight = rHeight;
			_platformDepth = platformDepth;

			Vector2 a = new Vector2();
			Vector2 b = new Vector2();
			if (platformDepth < 0)
			{
				a = new Vector2(-width / 2, rHeight);
				b = new Vector2(width / 2, lHeight);
			}
			else
			{
				a = new Vector2(-width / 2, lHeight);
				b = new Vector2(width / 2, rHeight);
			}
			Vector2 v = b - a;
			_normal = new Vector2(v.Y, -v.X);
			_normal.Normalize();

			ColliderManager.Instance.Platforms.Add(this);
			_lineTex = DebugManager.Instance.LineTex;
		}

		public override void Update(double deltaTime)
		{
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			DebugManager.Instance.DrawLine(spriteBatch,
			new Vector2(CurrentPosition.X - (Width / 2), (CurrentPosition.Y + LHeight)),
			new Vector2(CurrentPosition.X + (Width / 2), (CurrentPosition.Y + RHeight))
			);

			DebugManager.Instance.DrawLine(spriteBatch,
			new Vector2(CurrentPosition.X - (Width / 2), (CurrentPosition.Y + LHeight + PlatformDepth)),
			new Vector2(CurrentPosition.X + (Width / 2), (CurrentPosition.Y + RHeight + PlatformDepth))
			);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class Renderable : Component
	{
		protected Drawable _d;

		public Drawable Drawable
		{
			get { return _d; }
			set { _d = value; }
		}

		public Renderable(Entity owner, Texture2D sprite) : base(owner)
		{
			_d = new Drawable(sprite);
			_d._sprite = sprite;
			_d._spriteSize = 32;

			_d._origin = new Vector2(16, 16);
			_position = new Vector2(0, 0);
			_scale = new Vector2(1, 1);
			_d._layer = 0.1f;
			_d._colour = new Color(255, 255, 255, 255);
			_d._width = 32;
			_d._height = 32;
			_d._sourceRectangle = new Rectangle(0, 0, _d._width, _d._height);
			_d._animations = new Dictionary<int, Animation>();
			_d._currentAnim = 0;
			_d._transition = false;
			_d._nextAnim = 0;
		}



		public override void Update(double deltaTime)
		{
			_d.Animate(deltaTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 inv = new Vector2(1, -1);
			spriteBatch.Draw(_d._sprite, (_position + _owner.GetPosition()) * inv * _d._spriteSize, _d._sourceRectangle, _d._colour, _owner.GetRotation() + _rotation, _d._origin, _scale, _d._spriteEffects, _d._layer);
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{
			
		}
	}
}

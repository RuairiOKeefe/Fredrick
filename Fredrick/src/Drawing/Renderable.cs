using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	[Serializable]
	public class Renderable : Component
	{
		protected Drawable _drawable;
		protected bool _facingRight;

		public Drawable Drawable
		{
			get { return _drawable; }
			set { _drawable = value; }
		}

		public Renderable()
		{
			_owner = null;
			Position = new Vector2(0);
			Scale = new Vector2(1);
		}

		public Renderable(Entity owner, string id, string spriteName, Vector2 origin, Vector2 position, Vector2 scale, int width = 32, int height = 32, float layer = 0.1f) : base(owner, id)
		{
			_drawable = new Drawable(spriteName, origin, width, height, layer);
			Position = position;
			Scale = scale;
			_facingRight = true;
		}

		public void Flip(bool faceRight)
		{
			if (faceRight != _facingRight)
			{
				_facingRight = !_facingRight;
				if (_facingRight)
				{
					_drawable._spriteEffects = SpriteEffects.None;
				}
				else
				{
					_drawable._spriteEffects = SpriteEffects.FlipHorizontally;
				}
			}
		}

		public override void Load(ContentManager content)
		{
			_drawable.Load(content);
		}

		public override void Unload()
		{

		}

		public override void Update(double deltaTime)
		{
			_drawable.Animate(deltaTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 inv = new Vector2(1, -1);
			spriteBatch.Draw(ResourceManager.Instance.Textures[_drawable._spriteName], (Position + _owner.Position) * inv * _drawable._spriteSize, _drawable._sourceRectangle, _drawable._colour, _owner.Rotation + Rotation, _drawable._origin, Scale, _drawable._spriteEffects, _drawable._layer);
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{

		}
	}
}

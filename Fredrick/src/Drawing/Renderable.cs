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

		public Drawable Drawable
		{
			get { return _drawable; }
			set { _drawable = value; }
		}

		public Renderable()
		{
			_owner = null;
			_position = new Vector2(0);
			_scale = new Vector2(1);
		}

		public Renderable(Entity owner, ContentManager content, String spriteName, Vector2 origin, Vector2 position, Vector2 scale, int width = 32, int height = 32, float layer = 0.1f) : base(owner)
		{
			_drawable = new Drawable(content, spriteName, origin, width, height, layer);
			_position = position;
			_scale = scale;
		}

		public override void Load(ContentManager content)
		{
			_drawable.Load(content);
		}

		public override void Update(double deltaTime)
		{
			_drawable.Animate(deltaTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 inv = new Vector2(1, -1);
			spriteBatch.Draw(ResourceManager.Instance.Textures[_drawable._spriteName], (_position + _owner.GetPosition()) * inv * _drawable._spriteSize, _drawable._sourceRectangle, _drawable._colour, _owner.GetRotation() + _rotation, _drawable._origin, _scale, _drawable._spriteEffects, _drawable._layer);
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{

		}
	}
}

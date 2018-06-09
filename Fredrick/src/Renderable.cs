using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	internal class Renderable : Component
	{
		private Texture2D sprite;
		private Rectangle sourceRectangle;//The region of the sprite sheet that will be used
		private Vector2 origin;//The centerpoint of the sprite
		private SpriteEffects spriteEffects; //just controls flipping from the look of it leave as 0 for none
		private float layer;
		private Color color;

		Renderable(ComponentOwner owner) : base(owner)
		{

		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(sprite, position, sourceRectangle, color, rotation, origin, scale, spriteEffects, layer);
		}

		public override void Update(double delta)
		{

		}
	}
}

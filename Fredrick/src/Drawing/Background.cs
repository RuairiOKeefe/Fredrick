using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public class Background
	{
		struct Layer
		{
			public Drawable m_drawable;

			//For parallax values: 0 = moves with camera, 1 = stationary
			public float m_parallaxHorizontal;
			public float m_parallaxVertical;

			public Layer(Drawable drawable, float parallaxHorizontal, float parallaxVertical)
			{
				m_drawable = drawable;
				m_parallaxHorizontal = parallaxHorizontal;
				m_parallaxVertical = parallaxVertical;
			}

			public void Load(ContentManager content)
			{
				m_drawable.Load(content);
			}

			public void Draw(SpriteBatch spriteBatch, Vector2 position)
			{
				Vector2 drawnPosition = new Vector2(position.X * (1 - m_parallaxHorizontal), position.Y * (1 - m_parallaxVertical)) * m_drawable._spriteSize;
				drawnPosition *= new Vector2(1, -1);
				position *= new Vector2(1, -1);
				drawnPosition.X = drawnPosition.X % m_drawable._spriteWidth;
				drawnPosition.Y = drawnPosition.Y % m_drawable._spriteWidth;
				Rectangle sourceRect = new Rectangle((int)(drawnPosition.X), (int)(drawnPosition.Y), m_drawable._spriteWidth, m_drawable._spriteHeight);
				spriteBatch.Draw(ResourceManager.Instance.Textures[m_drawable._spriteName], position * m_drawable._spriteSize, sourceRect, m_drawable._colour, 0.0f, m_drawable._origin, new Vector2(1), m_drawable._spriteEffects, m_drawable._layer);
			}
		}

		private List<Layer> layers = new List<Layer>();

		public void AddLayer(Drawable drawable, float parallaxHorizontal, float parallaxVertical)
		{
			layers.Add(new Layer(drawable, parallaxHorizontal, parallaxVertical));
		}

		public void Load(ContentManager content)
		{
			foreach (Layer layer in layers)
			{
				layer.Load(content);
			}
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 position)
		{
			foreach (Layer layer in layers)
			{
				layer.Draw(spriteBatch, position);
			}
		}
	}
}

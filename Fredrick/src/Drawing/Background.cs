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
			public Color m_fogColour;
			public float m_fogDensity;
			public float m_offsetHorizontal;
			public float m_offsetVertical;

			public Layer(Drawable drawable, float parallaxHorizontal, float parallaxVertical, Color fogColour, float fogDensity = 0.0f, float offsetHorizontal = 0, float offsetVertical = 0)
			{
				m_drawable = drawable;
				m_parallaxHorizontal = parallaxHorizontal;
				m_parallaxVertical = parallaxVertical;
				m_fogColour = fogColour;
				m_fogDensity = fogDensity;
				m_offsetHorizontal = offsetHorizontal;
				m_offsetVertical = offsetVertical;
			}

			public void Load(ContentManager content)
			{
				m_drawable.Load(content);
			}

			public void Draw(SpriteBatch spriteBatch, Vector2 position)
			{
				Vector2 drawnPosition = new Vector2((position.X * (1 - m_parallaxHorizontal)) + m_offsetHorizontal, (position.Y * (1 - m_parallaxVertical)) + m_offsetVertical) * m_drawable._spriteSize;
				drawnPosition *= new Vector2(1, -1);
				position *= new Vector2(1, -1);
				drawnPosition.X = drawnPosition.X % m_drawable._spriteWidth;
				drawnPosition.Y = drawnPosition.Y % m_drawable._spriteWidth;
				Rectangle sourceRect = new Rectangle((int)(drawnPosition.X), (int)(drawnPosition.Y), m_drawable._spriteWidth, m_drawable._spriteHeight);
				Color baseColour = (m_drawable._colour * (1 - m_fogDensity));
				Color fogColour = (m_fogColour * m_fogDensity);
				Color CombinedColour = new Color(baseColour.R + fogColour.R, baseColour.G + fogColour.G, baseColour.B + fogColour.B, baseColour.A + fogColour.A);
				spriteBatch.Draw(ResourceManager.Instance.Textures[m_drawable._spriteName], position * m_drawable._spriteSize, sourceRect, CombinedColour, 0.0f, m_drawable._origin, new Vector2(1), m_drawable._spriteEffects, m_drawable._layer);
			}
		}

		private List<Layer> layers = new List<Layer>();

		public void AddLayer(Drawable drawable, float parallaxHorizontal, float parallaxVertical, Color fogColour, float fogDensity, float offsetHorizontal = 0, float offsetVertical = 0)
		{
			layers.Add(new Layer(drawable, parallaxHorizontal, parallaxVertical, fogColour, fogDensity, offsetHorizontal, offsetVertical));
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

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
			public float m_offsetHorizontal;
			public float m_offsetVertical;

			public Vector4 m_fogColour;
			public float m_fog;



			public Layer(Drawable drawable, float parallaxHorizontal, float parallaxVertical, Vector4 fogColour, float fog, float offsetHorizontal = 0, float offsetVertical = 0)
			{
				m_drawable = drawable;
				m_parallaxHorizontal = parallaxHorizontal;
				m_parallaxVertical = parallaxVertical;
				m_offsetHorizontal = offsetHorizontal;
				m_offsetVertical = offsetVertical;

				m_fogColour = fogColour;
				m_fog = fog;
			}

			public void Load(ContentManager content)
			{
				m_drawable.Load(content);
			}

			public void Draw(SpriteBatch spriteBatch, Vector2 position, Effect effect = null)
			{
				Vector2 drawnPosition = new Vector2((position.X * (1 - m_parallaxHorizontal)) + m_offsetHorizontal, (position.Y * (1 - m_parallaxVertical)) + m_offsetVertical) * m_drawable._spriteSize;
				drawnPosition *= new Vector2(1, -1);
				position *= new Vector2(1, -1);
				drawnPosition.X = drawnPosition.X % m_drawable._spriteWidth;
				drawnPosition.Y = drawnPosition.Y % m_drawable._spriteHeight;
				Rectangle sourceRect = new Rectangle((int)(drawnPosition.X), (int)(drawnPosition.Y), m_drawable._spriteWidth, m_drawable._spriteHeight);
				spriteBatch.Draw(ResourceManager.Instance.Textures[m_drawable._spriteName], position, sourceRect, m_drawable._colour, 0.0f, m_drawable._origin, new Vector2(1), m_drawable._spriteEffects, m_drawable._layer);
			}
		}

		private List<Layer> layers = new List<Layer>();

		public void AddLayer(Drawable drawable, float parallaxHorizontal, float parallaxVertical, Vector4 fogColour, float fog, float offsetHorizontal = 0, float offsetVertical = 0)
		{
			layers.Add(new Layer(drawable, parallaxHorizontal, parallaxVertical, fogColour, fog, offsetHorizontal, offsetVertical));
		}

		public void Load(ContentManager content)
		{
			foreach (Layer layer in layers)
			{
				layer.Load(content);
			}
		}

		public void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, Camera cam, Effect fog = null)
		{
			Vector2 position = cam.Position;
			foreach (Layer layer in layers)
			{
				fog.Parameters["FogColour"].SetValue(layer.m_fogColour);
				fog.Parameters["Fog"].SetValue(layer.m_fog);

				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, null, null, fog, cam.Get_Transformation(GraphicsDevice));
				layer.Draw(spriteBatch, position, fog);
				spriteBatch.End();
			}
		}
	}
}

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
	public class DrawManager
	{
		public struct DrawInfo
		{
			SpriteSortMode SortMode;
			BlendState BlendState;
			SamplerState SamplerState;
			DepthStencilState stencilState;
			RasterizerState RasterizerState;
		}

		private Dictionary<string, Effect> m_shaders;
		private Dictionary<string, List<Component>> m_drawComponents;//Key is shaderId
		private Dictionary<string, List<Component>> m_batchComponents;//Key is Component Id, Components are grouped by this and the render info for the first is used

		public DrawManager()
		{
			m_shaders = new Dictionary<string, Effect>();
			m_drawComponents = new Dictionary<string, List<Component>>();
			m_batchComponents = new Dictionary<string, List<Component>>();

			m_shaders.Add("", null);
		}

		public void SetLights(Effect shader, Lighting lighting)
		{
			Light[] fixedLights = new Light[16];
			lighting.GetFixedLights(out fixedLights);

			if (shader != null)
			{
				shader.Parameters["positions"].SetValue(LightingUtils.GetPositions(fixedLights));
				shader.Parameters["colours"].SetValue(LightingUtils.GetColours(fixedLights));
			}
		}

		public void Load(ContentManager content)
		{
			foreach (KeyValuePair<string, List<Component>> shaderComponentPair in m_drawComponents)
			{
				string shaderId = shaderComponentPair.Key;
				if (!m_shaders.ContainsKey(shaderId))
				{
					Effect shader = content.Load<Effect>(shaderId);
					m_shaders.Add(shaderId, shader);
				}
			}
		}

		public void AddComponents(Entity entity)
		{
			foreach (Component component in entity.Components)
			{
				if (component.IsDrawn)
				{
					AddComponent(component);
				}
			}
		}

		/// <summary>
		///  Adds a compoment to the m_drawComponents dictionary.
		///  If the shaderId already exists in the dictionary, the component is added to the existing list, otherewise a new key is created.
		///  NOTE: This method should never be called after a Load()
		/// </summary>
		/// <param name="component"></param>
		public void AddComponent(Component component)
		{
			if (component.DrawBatched)
			{
				string id = component.GetType().ToString() + component.Id;
				if (m_batchComponents.ContainsKey(id))
				{
					m_batchComponents[id].Add(component);
				}
				else
				{
					m_batchComponents.Add(id, new List<Component> { component });
				}
			}
			else
			{
				string shaderId = component.ShaderId;
				if (m_drawComponents.ContainsKey(shaderId))
				{
					m_drawComponents[shaderId].Add(component);
				}
				else
				{
					m_drawComponents.Add(shaderId, new List<Component> { component });
				}
			}
		}

		public void RemoveComponent(Component component)
		{

		}

		public void DrawComponents(SpriteBatch spriteBatch, Matrix transformationMatrix, Lighting lighting)
		{
			foreach (KeyValuePair<string, List<Component>> shaderComponentPair in m_drawComponents)
			{
				List<Component> components = shaderComponentPair.Value;
				Effect shader = m_shaders[shaderComponentPair.Key];
				// shader should be replaced with a class that can hold global uniforms ect
				foreach (Component component in components)
				{
					SetLights(shader, lighting);
					component.Draw(spriteBatch, shader, transformationMatrix);
				}
			}
		}

		public void DrawBatch(SpriteBatch spriteBatch, Matrix transformationMatrix, Lighting lighting)
		{

		}

		public void Draw(SpriteBatch spriteBatch, Matrix transformationMatrix, Lighting lighting)
		{
			DrawComponents(spriteBatch, transformationMatrix, lighting);
			DrawBatch(spriteBatch, transformationMatrix, lighting);
		}
	}
}

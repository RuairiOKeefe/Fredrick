using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Fredrick.src
{
	public class Entity : Transform
	{
		private bool _active { get; set; }

		public string Id { get; }

		public List<Component> Components = new List<Component>();

		public Entity()
		{
			_active = true;
		}

		public bool GetActive() { return _active; }

		public T GetComponent<T>() where T : Component
		{
			var component = Components.FirstOrDefault(c => c.GetType() == typeof(T));
			return (T)component;
		}

		public void Load(ContentManager content)
		{
			foreach (var c in Components)
			{
				c.Load(content);
			}
		}

		public void Update(double deltaTime)//Add remove check here for if component is to be removed
		{
			foreach (var c in Components)
			{
				c.Update(deltaTime);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var c in Components)
			{
				c.Draw(spriteBatch);
			}
		}

		public void DebugDraw(SpriteBatch spriteBatch)
		{
			{
				foreach (var c in Components)
				{
					c.DebugDraw(spriteBatch);
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	class Entity : Transform, ComponentOwner
	{
		private bool active;

		public string Id { get; }

		IList<Component> components;

		public bool GetActive() { return active; }

		public T GetComponent<T>() where T : Component
		{
			var component = components.FirstOrDefault(c => c.GetType() == typeof(T));
			return (T)component;
		}

		public void Update(double delta)//Add remove check here for if component is to be removed
		{
			foreach (var c in components)
			{
				c.Update(delta);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var c in components)
			{
				c.Draw(spriteBatch);
			}
		}


	}
}

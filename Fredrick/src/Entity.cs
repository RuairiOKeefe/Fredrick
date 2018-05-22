using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Fredrick
{


	abstract class Component : Transform
	{
		protected bool active;
		protected Entity parent;

		public bool GetActive() { return active; }

		public abstract void Update(double delta);
		public abstract void Draw();
	}

	class Entity : Transform
	{
		private bool active;
		Dictionary<String, Component> components;

		public bool GetActive() { return active; }

		public void Update(double delta)
		{
			foreach (Component c in components.Values)
			{
				c.Update(delta);
			}
		}

		public void Draw()
		{
			foreach (Component c in components.Values)
			{
				c.Draw();
			}
		}
	}
}

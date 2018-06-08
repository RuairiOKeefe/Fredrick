using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	abstract class Component : Transform
	{
		protected ComponentOwner owner;
		protected bool active;
		protected Entity parent;

		protected Component(ComponentOwner owner)
		{
			this.owner = owner;
		}

		public bool GetActive() { return active; }

		public abstract void Update(double delta);
		public abstract void Draw(SpriteBatch spriteBatch);
	}
}

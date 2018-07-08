using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public abstract class Component : Transform
	{
		protected Entity _owner;
		protected bool _active;

		public Component(Entity owner)
		{
			this._owner = owner;
		}

		public bool GetActive() { return _active; }

		public abstract void Update(double deltaTime);
		public abstract void Draw(SpriteBatch spriteBatch);
	}
}

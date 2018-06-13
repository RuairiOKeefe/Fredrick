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
		protected ComponentOwner _owner;
		protected bool _active;
		protected Entity _parent;

		protected Component(ComponentOwner owner)
		{
			this._owner = owner;
		}

		public bool GetActive() { return _active; }

		public abstract void Update(double deltaTime);
		public abstract void Draw(SpriteBatch spriteBatch);
	}
}

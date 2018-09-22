using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;

namespace Fredrick.src
{
	[XmlRoot(Namespace = "Fredrick.src")]
	[XmlInclude(typeof(Renderable))]
	[XmlInclude(typeof(AABBCollider))]
	public abstract class Component : Transform
	{
		protected Entity _owner;
		protected bool _active;

		public Component()
		{
		}

		public Component(Entity owner)
		{
			_owner = owner;
		}

		public Entity GetOwner()
		{
			return _owner;
		}

		public bool GetActive() { return _active; }

		public abstract void Load(ContentManager content);
		public abstract void Update(double deltaTime);
		public abstract void Draw(SpriteBatch spriteBatch);
		public abstract void DebugDraw(SpriteBatch spriteBatch);
	}
}

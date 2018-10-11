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
	[Serializable]
	[XmlRoot(Namespace = "Fredrick.src")]
	[XmlInclude(typeof(Renderable))]
	[XmlInclude(typeof(AABBCollider))]
	public abstract class Component : Transform
	{
		protected Entity _owner;
		public string Id { get; set; }
		public bool Active { get; set; }
		public List<string> Tags { get; set; }

		public Component()
		{
		}

		public Component(Entity owner, string id = null, bool active = true)
		{
			_owner = owner;
			Id = id;
			Active = active;
			Tags = new List<string>();
		}

		public Entity Owner
		{
			get { return _owner; }
			set { _owner = value; }
		}

		public abstract void Load(ContentManager content);
		public abstract void Unload();
		public abstract void Update(double deltaTime);
		public abstract void Draw(SpriteBatch spriteBatch);
		public abstract void DebugDraw(SpriteBatch spriteBatch);
	}
}

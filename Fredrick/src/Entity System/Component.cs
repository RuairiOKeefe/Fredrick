using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Fredrick.src.Colliders;

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
		public String ShaderId { get; protected set; }//Should be set in Load()
		public bool DrawBatched { get; set; }//Batched components are grouped based on the Id of the component

		public virtual bool IsDrawn { get { return false; } }

		public Component()
		{
		}

		public Component(Entity owner, string id = null, List<string> tags = null, bool active = true, bool drawBatched = false)
		{
			_owner = owner;
			Id = id;
			Active = active;
			ShaderId = "";
			DrawBatched = drawBatched;
			Tags = new List<string>();
			if (tags != null)
			{
				foreach (string tag in tags)
				{
					Tags.Add(tag);
				}
			}
		}

		public Component(Entity owner, Component original, bool active = true)
		{
			_owner = owner;
			Id = original.Id;
			Active = active;
			ShaderId = "";
			DrawBatched = original.DrawBatched;
			Tags = new List<string>();
			if (original.Tags != null)
			{
				foreach (string tag in original.Tags)
				{
					Tags.Add(tag);
				}
			}
			Position = new Vector2(original.Position.X, original.Position.Y);
			Rotation = original.Rotation;
			Scale = new Vector2(original.Scale.X, original.Scale.Y);
		}

		public Entity Owner
		{
			get { return _owner; }
			set { _owner = value; }
		}

		public abstract void Load(ContentManager content);
		public abstract void Unload();
		public abstract void Update(double deltaTime);
		public virtual void Draw(SpriteBatch spriteBatch, Effect shader, Matrix transformationMatrix) { }//MAKE ABSTRACT AFTER TESTING
		public abstract void DrawBatch(SpriteBatch spriteBatch);
		public abstract void DebugDraw(SpriteBatch spriteBatch);
		public abstract Component Copy(Entity owner);
	}
}

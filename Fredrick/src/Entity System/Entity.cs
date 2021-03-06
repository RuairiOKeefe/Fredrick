﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Fredrick.src
{
	[Serializable]
	public class Entity : Transform
	{
		public bool Active { get; set; }

		public string Id { get; set; }

		public List<string> Tags { get; set; }

		public List<Component> Components { get; set; }

		public Entity()
		{
			Active = true;
			Id = null;
			Tags = new List<string>();
			Components = new List<Component>();
		}

		public Entity(bool active, string id)
		{
			Active = active;
			Id = id;
			Tags = new List<string>();
			Components = new List<Component>();
		}

		public Entity(Entity original)
		{
			Id = original.Id;
			Active = original.Active;
			Tags = original.Tags;
			Components = new List<Component>();
			foreach (Component c in original.Components)
			{
				Components.Add(c.Copy(this));
				Components[Components.Count - 1].Owner = this;
			}
		}

		public T GetComponent<T>(string id = null, string tag = null) where T : Component
		{
			var component = Components.FirstOrDefault(c => c.GetType() == typeof(T) && (id == null || c.Id == id) && (tag == null || c.Tags.Contains(tag)));
			return (T)component;
		}

		public T GetDerivedComponent<T>() where T : Component
		{
			var component = Components.FirstOrDefault(c => c.GetType().BaseType == typeof(T));
			return (T)component;
		}

		public Component GetComponentWithId(string id)
		{
			var component = Components.FirstOrDefault(c => c.Id == id);
			return component;
		}

		public Component GetComponentWithTag(string tag)
		{
			var component = Components.FirstOrDefault(c => c.Tags.Contains(tag));
			return component;
		}

		public void Load(ContentManager content)
		{
			foreach (var c in Components)
			{
				c.Load(content);
			}
		}

		public void Unload()
		{
			foreach (var c in Components)
			{
				c.Unload();
			}
		}

		public void Update(double deltaTime)//Add remove check here for if component is to be removed
		{
			if (Active)
			{
				for (int i = Components.Count - 1; i >= 0; i--)
				{
					Component c = Components[i];
					if (c.Active)
						c.Update(deltaTime);
				}
			}
		}

		public void DrawBatch(SpriteBatch spriteBatch)
		{
			if (Active)
			{
				foreach (var c in Components)
				{
					if (c.Active)
						c.DrawBatch(spriteBatch);
				}
			}
		}

		public void DebugDraw(SpriteBatch spriteBatch)
		{
			if (Active)
			{
				foreach (var c in Components)
				{
					c.DebugDraw(spriteBatch);
				}
			}
		}
	}
}

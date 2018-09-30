using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public class LevelEditor
	{
		Entity indicator;
		List<Entity> entities;
		int index;
		bool editing;
		bool gridLock;
		Vector2 position;

		public LevelEditor()
		{
			indicator = new Entity();
			entities = new List<Entity>();
			index = 0;
			editing = false;
			gridLock = false;

			Entity e = new Entity(true, "Block");
			e.Tags.Add("Terrain");
			Renderable r = new Renderable(e, "Block", "testSheet", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			r.Drawable.AddAnimation(0, 0, 1, 30);
			AABBCollider c = new AABBCollider(e, new Vector2(0), 1, 1);
			e.Components.Add(r);
			e.Components.Add(c);
			entities.Add(e);

			e = new Entity(true, "Slope");
			e.Tags.Add("Terrain");
			r = new Renderable(e, "Slock", "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			r.Drawable.AddAnimation(0, 0, 1, 30);
			Platform p = new Platform(e, new Vector2(0), 1, 1, 0, 0, -0.5f, 0.5f, -0.3f);
			e.Components.Add(r);
			e.Components.Add(p);
			entities.Add(e);

			e = new Entity(true, "Slope");
			e.Tags.Add("Terrain");
			r = new Renderable(e, "Slope", "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			r.Drawable.AddAnimation(32, 0, 1, 30);
			p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, -0.5f, -0.3f);
			e.Components.Add(r);
			e.Components.Add(p);
			entities.Add(e);

			e = new Entity(true, "Slope");
			e.Tags.Add("Terrain");
			r = new Renderable(e, "Slope", "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			r.Drawable.AddAnimation(64, 0, 1, 30);
			p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, -0.5f, 0.3f);
			e.Components.Add(r);
			e.Components.Add(p);
			entities.Add(e);

			e = new Entity(true, "Slope");
			e.Tags.Add("Terrain");
			r = new Renderable(e, "Slope", "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			r.Drawable.AddAnimation(96, 0, 1, 30);
			p = new Platform(e, new Vector2(0), 1, 1, 0, 0, -0.5f, 0.5f, 0.3f);
			e.Components.Add(r);
			e.Components.Add(p);
			entities.Add(e);
		}

		public Entity SelectEntity(int i)
		{
			Entity e = new Entity();
			switch (i)
			{
				case (0):
					e = new Entity(true, "Block");
					e.Tags.Add("Terrain");
					Renderable r = new Renderable(e, "Block", "TestSheet", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
					r.Drawable.AddAnimation(0, 0, 1, 30);
					AABBCollider c = new AABBCollider(e, new Vector2(0), 1, 1);
					e.Components.Add(r);
					e.Components.Add(c);
					entities.Add(e);
					break;
				case (1):
					e = new Entity(true, "Slope");
					e.Tags.Add("Terrain");
					r = new Renderable(e, "Slope", "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
					r.Drawable.AddAnimation(0, 0, 1, 30);
					Platform p = new Platform(e, new Vector2(0), 1, 1, 0, 0, -0.5f, 0.5f, -0.3f);
					e.Components.Add(r);
					e.Components.Add(p);
					entities.Add(e);
					break;
				case (2):
					e = new Entity(true, "Slope");
					e.Tags.Add("Terrain");
					r = new Renderable(e, "Slope", "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
					r.Drawable.AddAnimation(32, 0, 1, 30);
					p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, -0.5f, -0.5f);
					e.Components.Add(r);
					e.Components.Add(p);
					entities.Add(e);
					break;
				case (3):
					e = new Entity(true, "Slope");
					e.Tags.Add("Terrain");
					r = new Renderable(e, "Slope", "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
					r.Drawable.AddAnimation(64, 0, 1, 30);
					p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, -0.5f, 0.5f);
					e.Components.Add(r);
					e.Components.Add(p);
					entities.Add(e);
					break;
				case (4):
					e = new Entity(true, "Slope");
					e.Tags.Add("Terrain");
					r = new Renderable(e, "Slope", "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
					r.Drawable.AddAnimation(96, 0, 1, 30);
					p = new Platform(e, new Vector2(0), 1, 1, 0, 0, -0.5f, 0.5f, 0.5f);
					e.Components.Add(r);
					e.Components.Add(p);
					entities.Add(e);
					break;
			}

			e.Position = new Vector2(position.X, position.Y);

			return e;
		}

		public void Load(ContentManager content)
		{
			foreach (Entity e in entities)
			{
				e.Load(content);
			}
		}

		public List<Entity> Update(double deltaTime, ref List<Entity> terrain, ContentManager content)
		{
			if (InputHandler.Instance.IsKeyPressed(InputHandler.Action.Editor))
			{
				editing = !editing;
			}
			if (editing)
			{
				if (InputHandler.Instance.IsKeyPressed(InputHandler.Action.NextEnt))
				{
					index++;
				}
				if (InputHandler.Instance.IsKeyPressed(InputHandler.Action.PrevEnt))
				{
					index--;
				}
				if (InputHandler.Instance.IsKeyPressed(InputHandler.Action.GridLock))
				{
					gridLock = !gridLock;
				}

				if (index > entities.Count - 1)
					index = 0;
				if (index < 0)
					index = entities.Count - 1;

				indicator = entities[index];

				position = InputHandler.Instance.WorldMousePosition;
				if (gridLock)
				{
					position = new Vector2((float)Math.Floor(position.X), (float)Math.Floor(position.Y));
				}
				indicator.Position = position;
				indicator.Update(deltaTime);

				if (InputHandler.Instance.IsLeftMousePressed())
				{
					terrain.Add(SelectEntity(index));
					terrain.Last<Entity>().Load(content);
				}
			}
			return terrain;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (editing)
			{
				indicator.Draw(spriteBatch);
			}
		}
	}
}

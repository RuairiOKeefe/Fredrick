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
			r.Drawable.AddAnimation(0, 0, 1, 30, Animation.OnEnd.Loop, 0);
			AABBCollider c = new AABBCollider(e, new Vector2(0), 1, 1);
			e.Components.Add(r);
			e.Components.Add(c);
			entities.Add(e);

			e = new Entity(true, "Slope");
			e.Tags.Add("Terrain");
			r = new Renderable(e, "Slock", "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			r.Drawable.AddAnimation(0, 0, 1, 30, Animation.OnEnd.Loop, 0);
			Platform p = new Platform(e, new Vector2(0), 1, 1, 0, 0, -0.5f, 0.5f, -0.3f);
			e.Components.Add(r);
			e.Components.Add(p);
			entities.Add(e);

			e = new Entity(true, "Slope");
			e.Tags.Add("Terrain");
			r = new Renderable(e, "Slope", "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			r.Drawable.AddAnimation(32, 0, 1, 30, Animation.OnEnd.Loop, 0);
			p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, -0.5f, -0.3f);
			e.Components.Add(r);
			e.Components.Add(p);
			entities.Add(e);

			e = new Entity(true, "Slope");
			e.Tags.Add("Terrain");
			r = new Renderable(e, "Slope", "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			r.Drawable.AddAnimation(64, 0, 1, 30, Animation.OnEnd.Loop, 0);
			p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, -0.5f, 0.3f);
			e.Components.Add(r);
			e.Components.Add(p);
			entities.Add(e);

			e = new Entity(true, "Slope");
			e.Tags.Add("Terrain");
			r = new Renderable(e, "Slope", "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			r.Drawable.AddAnimation(96, 0, 1, 30, Animation.OnEnd.Loop, 0);
			p = new Platform(e, new Vector2(0), 1, 1, 0, 0, -0.5f, 0.5f, 0.3f);
			e.Components.Add(r);
			e.Components.Add(p);
			entities.Add(e);

			e = new Entity(true, "Platform");
			e.Tags.Add("Terrain");
			r = new Renderable(e, "Block", "grass", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			r.Drawable.AddAnimation(0, 0, 1, 30, Animation.OnEnd.Loop, 0);
			p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, 0.5f, -0.3f);
			e.Components.Add(r);
			e.Components.Add(p);
			entities.Add(e);

			e = new Entity(true, "Emptyblock");
			e.Tags.Add("Terrain");
			r = new Renderable(e, "Block", "testSheet", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			r.Drawable.AddAnimation(0, 0, 1, 30, Animation.OnEnd.Loop, 0);
			e.Components.Add(r);
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
					Renderable r = new Renderable(e, "Block", "Dirt", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
					r.Drawable.AddAnimation(0, 0, 1, 30, Animation.OnEnd.Loop, 0);
					AABBCollider c = new AABBCollider(e, new Vector2(0), 1, 1);
					e.Components.Add(r);
					e.Components.Add(c);
					break;
				case (1):
					e = new Entity(true, "Slope");
					e.Tags.Add("Terrain");
					r = new Renderable(e, "Slope", "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
					r.Drawable.AddAnimation(0, 0, 1, 30, Animation.OnEnd.Loop, 0);
					Platform p = new Platform(e, new Vector2(0), 1, 1, 0, 0, -0.5f, 0.5f, -0.3f);
					e.Components.Add(r);
					e.Components.Add(p);
					break;
				case (2):
					e = new Entity(true, "Slope");
					e.Tags.Add("Terrain");
					r = new Renderable(e, "Slope", "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
					r.Drawable.AddAnimation(32, 0, 1, 30, Animation.OnEnd.Loop, 0);
					p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, -0.5f, -0.5f);
					e.Components.Add(r);
					e.Components.Add(p);
					break;
				case (3):
					e = new Entity(true, "Slope");
					e.Tags.Add("Terrain");
					r = new Renderable(e, "Slope", "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
					r.Drawable.AddAnimation(64, 0, 1, 30, Animation.OnEnd.Loop, 0);
					p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, -0.5f, 0.5f);
					e.Components.Add(r);
					e.Components.Add(p);
					break;
				case (4):
					e = new Entity(true, "Slope");
					e.Tags.Add("Terrain");
					r = new Renderable(e, "Slope", "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
					r.Drawable.AddAnimation(96, 0, 1, 30, Animation.OnEnd.Loop, 0);
					p = new Platform(e, new Vector2(0), 1, 1, 0, 0, -0.5f, 0.5f, 0.5f);
					e.Components.Add(r);
					e.Components.Add(p);
					break;
				case (5):
					e = new Entity(true, "Platform");
					e.Tags.Add("Terrain");
					r = new Renderable(e, "Block", "grass", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
					r.Drawable.AddAnimation(0, 0, 1, 30, Animation.OnEnd.Loop, 0);
					p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, 0.5f, -0.3f);
					e.Components.Add(r);
					e.Components.Add(p);
					break;
				case (6):
					e = new Entity(true, "Emptyblock");
					e.Tags.Add("Terrain");
					r = new Renderable(e, "Block", "Dirt", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
					r.Drawable.AddAnimation(0, 0, 1, 30, Animation.OnEnd.Loop, 0);
					e.Components.Add(r);
					break;
			}

			e.Position = new Vector2(position.X, position.Y);

			return e;
		}

		public void SwitchBlocks()
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

			indicator = SelectEntity(index);
		}

		public void AddBlock(ref List<Entity> terrain, ContentManager content)
		{
			terrain.Add(SelectEntity(index));
			terrain.Last<Entity>().Load(content);
		}

		public void RemoveBlock(ref List<Entity> terrain)
		{
			List<Entity> localEntities = ColliderManager.Instance.Terrain[(int)Math.Floor(position.X + 0.5), (int)Math.Floor(position.Y + 0.5)];

			int count = localEntities.Count - 1;
			for (int i = count; i >= 0; i--)
			{
				Entity e = localEntities[i];
				if (e.GetComponent<Renderable>() != null)
				{
					Renderable r = e.GetComponent<Renderable>();

					Vector2 pos = e.Position + r.Position;
					float halfWidth = ((float)r.Drawable._width / (float)r.Drawable._spriteSize) / 2;
					float halfHeight = ((float)r.Drawable._height / (float)r.Drawable._spriteSize) / 2;

					if (position.X < pos.X + halfWidth && position.X > pos.X - halfWidth && position.Y < pos.Y + halfHeight && position.Y > pos.Y - halfHeight)
					{
						e.Unload();
						terrain.Remove(e);
						localEntities.Remove(e);
					}
				}
			}
		}

		public void Load(ContentManager content)
		{
			foreach (Entity e in entities)
			{
				e.Load(content);
			}
		}

		public void Update(double deltaTime, ref List<Entity> terrain, ContentManager content)
		{
			if (InputHandler.Instance.IsKeyPressed(InputHandler.Action.Editor))
			{
				editing = !editing;
			}

			if (editing)
			{
				SwitchBlocks();
				position = InputHandler.Instance.WorldMousePosition;
				if (gridLock)
				{
					position = new Vector2((float)Math.Floor(position.X + 0.5), (float)Math.Floor(position.Y + 0.5));
				}
				indicator.Position = position;
				indicator.Update(deltaTime);

				if (InputHandler.Instance.IsLeftMousePressed())
				{
					AddBlock(ref terrain, content);
				}

				if (InputHandler.Instance.IsRightMousePressed())
				{
					RemoveBlock(ref terrain);
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (editing)
			{
				indicator.Draw(spriteBatch);
			}
		}

		public void DebugDraw(SpriteBatch spriteBatch)
		{
			if (editing)
			{
				indicator.DebugDraw(spriteBatch);
			}
		}
	}
}

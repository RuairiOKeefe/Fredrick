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

		public LevelEditor()
		{
			indicator = new Entity();
			entities = new List<Entity>();

			Entity e = new Entity("block");
			e.Tags.Add("block");
			Renderable r = new Renderable(e, "testSheet", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			r.Drawable.AddAnimation(0, 0, 1, 30);
			AABBCollider c = new AABBCollider(e, new Vector2(0), 1, 1);
			e.Components.Add(r);
			e.Components.Add(c);
			entities.Add(e);

			e = new Entity("slope");
			e.Tags.Add("block");
			r = new Renderable(e, "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			r.Drawable.AddAnimation(0, 0, 1, 30);
			Platform p = new Platform(e, new Vector2(0), 1, 1, 0, 0, -0.5f, 0.5f, -0.3f);
			e.Components.Add(r);
			e.Components.Add(p);
			entities.Add(e);

			e = new Entity("slope");
			e.Tags.Add("block");
			r = new Renderable(e, "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			r.Drawable.AddAnimation(32, 0, 1, 30);
			 p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, -0.5f, -0.3f);
			e.Components.Add(r);
			e.Components.Add(p);
			entities.Add(e);

			e = new Entity("slope");
			e.Tags.Add("block");
			r = new Renderable(e, "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			r.Drawable.AddAnimation(64, 0, 1, 30);
			 p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, -0.5f, 0.3f);
			e.Components.Add(r);
			e.Components.Add(p);
			entities.Add(e);

			e = new Entity("slope");
			e.Tags.Add("block");
			r = new Renderable(e, "tempSlope", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			r.Drawable.AddAnimation(96, 0, 1, 30);
			 p = new Platform(e, new Vector2(0), 1, 1, 0, 0, -0.5f, 0.5f, 0.3f);
			e.Components.Add(r);
			e.Components.Add(p);
			entities.Add(e);
		}

		public void Load(ContentManager content)
		{
			foreach (Entity e in entities)
			{
				e.Load(content);
			}
		}

		public void Update(double deltaTime)
		{

		}

		public void Draw(SpriteBatch spriteBatch)
		{

		}
	}
}

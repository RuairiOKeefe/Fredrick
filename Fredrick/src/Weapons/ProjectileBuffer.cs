using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Fredrick.src
{
	public sealed class ProjectileBuffer
	{
		private static ProjectileBuffer _instance = null;
		private static readonly object _padlock = new object();

		public static ProjectileBuffer Instance
		{
			get
			{
				lock (_padlock)
				{
					if (_instance == null)
						_instance = new ProjectileBuffer();
					return _instance;
				}
			}
		}

		public const int NUM_PROJECTILES = 10000;

		private Stack<Entity> _inactiveProjectiles;


		public Stack<Entity> InactiveProjectiles
		{
			get { return _inactiveProjectiles; }
			set { _inactiveProjectiles = value; }
		}

		public void Load(String spriteName, ContentManager content)
		{
			for (int i = 0; i < NUM_PROJECTILES; i++)
			{
				Entity e = new Entity();
				Projectile p = new Projectile(e);
				CircleCollider cc = new CircleCollider(e);
				Renderable r = new Renderable(e, spriteName, new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
				r.Drawable.AddAnimation(0, 0, 1, 1);
				r.Drawable.AddAnimation(32, 0, 1, 1);
				Emitter emitter = new Emitter(e, spriteName, false, 1000, 300, new Vector2(0, 0), 0, 0, 8.0f, 0.5);

				e.Components.Add(p);
				e.Components.Add(cc);
				e.Components.Add(r);
				e.Components.Add(emitter);

				_inactiveProjectiles.Push(e);//need to improve

				e.Load(content);
			}
		}

		public ProjectileBuffer()
		{
			_inactiveProjectiles = new Stack<Entity>(NUM_PROJECTILES);

		}
	}
}
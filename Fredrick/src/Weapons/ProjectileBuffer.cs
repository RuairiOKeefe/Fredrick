using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using Resources = Fredrick.src.ResourceManagement.Resources;

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

		public Stack<Entity> InactiveProjectiles;
		public List<Entity> ActiveProjectiles;

		public ProjectileBuffer()
		{
			InactiveProjectiles = new Stack<Entity>(NUM_PROJECTILES);
			ActiveProjectiles = new List<Entity>(NUM_PROJECTILES);
		}

		public void Load(ContentManager content)
		{
			for (int i = 0; i < NUM_PROJECTILES; i++)
			{
				Entity e = new Entity(Resources.Instance.ProjectileEntities["FragNade"]);
				InactiveProjectiles.Push(e);

				e.Load(content);
			}
		}

		public void Update(double deltaTime)
		{
			foreach (Entity e in ActiveProjectiles)
			{
				if (e.GetComponent<CircleCollider>() != null)
				{
					e.GetComponent<CircleCollider>().UpdatePosition();
				}
			}

			for (int i = (ActiveProjectiles.Count - 1); i >= 0; i--)
			{
				Entity e = ActiveProjectiles[i];

				e.Update(deltaTime);
				if (e.GetComponent<Projectile>().Dead)
				{
					InactiveProjectiles.Push(e);
					ActiveProjectiles.RemoveAt(i);
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (Entity e in ActiveProjectiles)
			{
				e.DrawBatch(spriteBatch);
			}
		}
	}
}
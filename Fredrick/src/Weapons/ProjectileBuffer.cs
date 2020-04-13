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

		public Dictionary<string, Dictionary<string, Stack<Entity>>> InactiveProjectiles;
		public Dictionary<string, Dictionary<string, List<Entity>>> ActiveProjectiles;//There is an assumption that projectiles only contain 1 renderable

		public ProjectileBuffer()
		{
			InactiveProjectiles = new Dictionary<string, Dictionary<string, Stack<Entity>>>();
			ActiveProjectiles = new Dictionary<string, Dictionary<string, List<Entity>>>();

			foreach (Entity e in Resources.Instance.ProjectileEntities.Values)
			{
				string shader = "";
				Renderable r = e.GetComponent<Renderable>();
				if (r != null)
				{
					if (r.Drawable.ShaderInfo != null)
					{
						shader = r.Drawable.ShaderInfo.ShaderId;
					}
					string projectile = e.Id;
					if (!InactiveProjectiles.ContainsKey(shader))
					{
						InactiveProjectiles.Add(shader, new Dictionary<string, Stack<Entity>>());
					}
					if (!InactiveProjectiles[shader].ContainsKey(projectile))
					{
						InactiveProjectiles[shader].Add(projectile, new Stack<Entity>(NUM_PROJECTILES));
					}
					if (!ActiveProjectiles.ContainsKey(shader))
					{
						ActiveProjectiles.Add(shader, new Dictionary<string, List<Entity>>());
					}
					if (!ActiveProjectiles[shader].ContainsKey(projectile))
					{
						ActiveProjectiles[shader].Add(projectile, new List<Entity>(NUM_PROJECTILES));
					}
				}
			}
		}

		public Entity Pop(string projectile)
		{
			foreach (var v in InactiveProjectiles)
			{
				if (v.Value.ContainsKey(projectile))
				{
					return v.Value[projectile].Pop();
				}
			}
			return null;//Should throw
		}

		public void Add(string projectile, Entity e)
		{
			foreach (var v in ActiveProjectiles)
			{
				if (v.Value.ContainsKey(projectile))
				{
					v.Value[projectile].Add(e);
				}
			}
		}

		public void Load(ContentManager content)
		{
			foreach (Entity e in Resources.Instance.ProjectileEntities.Values)
			{
				string shader = "";
				Renderable r = e.GetComponent<Renderable>();
				if (r != null)
				{
					if (r.Drawable.ShaderInfo != null)
					{
						shader = r.Drawable.ShaderInfo.ShaderId;
					}
					string projectile = e.Id;
					for (int i = 0; i < NUM_PROJECTILES; i++)
					{
						Entity projectileEntity = new Entity(e);
						InactiveProjectiles[shader][projectile].Push(projectileEntity);
						projectileEntity.Load(content);
					}
				}
			}
		}

		public void Update(double deltaTime)
		{
			foreach (string shader in ActiveProjectiles.Keys)
			{
				foreach (string projectile in ActiveProjectiles[shader].Keys)
				{
					for (int i = (ActiveProjectiles[shader][projectile].Count - 1); i >= 0; i--)
					{
						Entity e = ActiveProjectiles[shader][projectile][i];

						e.Update(deltaTime);

						if (e.GetComponent<CircleCollider>() != null)
						{
							e.GetComponent<CircleCollider>().UpdatePosition();
						}

						if (e.GetComponent<Projectile>().Dead)
						{
							InactiveProjectiles[shader][projectile].Push(e);
							ActiveProjectiles[shader][projectile].RemoveAt(i);
						}
					}
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch, Effect shader, Matrix transformMatrix)
		{
			string shaderName = "";
			if (shader != null)
			{
				shaderName = shader.Name;
			}
			if (ActiveProjectiles.ContainsKey(shaderName))
			{
				foreach (string projectile in ActiveProjectiles[shaderName].Keys)
				{
					if (ActiveProjectiles[shaderName][projectile].Count > 0)
					{
						if (ActiveProjectiles[shaderName][projectile][0].GetComponent<Renderable>().DrawBatched)
						{
							if (ActiveProjectiles[shaderName][projectile][0].GetComponent<Renderable>().Drawable.ShaderInfo != null)
							{
								ActiveProjectiles[shaderName][projectile][0].GetComponent<Renderable>().Drawable.ShaderInfo.SetUniforms(shader);
							}
						}
						else
						{
							foreach (Entity e in ActiveProjectiles[shaderName][projectile])
							{
								Renderable r = e.GetComponent<Renderable>();
								r.Draw(spriteBatch, shader, transformMatrix);
							}
						}
					}
				}
			}
		}
	}
}
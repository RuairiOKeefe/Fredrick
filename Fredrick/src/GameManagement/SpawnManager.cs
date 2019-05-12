using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	class SpawnManager
	{
		private static SpawnManager instance = null;
		private static readonly object padlock = new object();

		public static SpawnManager Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new SpawnManager();
					}
					return instance;
				}
			}
		}

		public struct Spawnable
		{
			public Entity Actor;
			public Timer SpawnTimer;
			public Vector2 RespawnPoint;

			public Spawnable(ref Entity actor, Timer spawnTimer, Vector2 respawnPoint)
			{
				Actor = actor;
				SpawnTimer = spawnTimer;
				RespawnPoint = respawnPoint;
			}

			public void Spawn()
			{
				Actor.Position = RespawnPoint;
				Actor.Active = true;
				if (Actor.GetComponent<Damageable>() != null)
				{
					Actor.GetComponent<Damageable>().Spawn();
				}
				SpawnTimer.Restart();
			}

			public void Update(Double deltaTime)
			{
				if (Actor.GetComponent<Damageable>() != null)
				{
					if (Actor.GetComponent<Damageable>().IsDead)
					{
						SpawnTimer.Update(deltaTime);
						if (SpawnTimer.IsTimeUp)
						{
							Spawn();
						}
					}
				}
			}
		}

		public Dictionary<Entity, Spawnable> Respawnables { get; private set; } = new Dictionary<Entity, Spawnable>();

		public void AddSpawnable(ref Entity actor, Timer spawnTime, Vector2 respawnPoint)
		{
			Respawnables.Add(actor, new Spawnable(ref actor, spawnTime, respawnPoint));
		}

		public void RemoveSpawnable(Entity actor)
		{
			Respawnables.Remove(actor);
		}

		public void Spawn(Entity actor)
		{
			Respawnables[actor].Spawn();
		}

		public void Load()
		{

		}

		public void Update(double deltaTime)
		{
			foreach (Spawnable respawnable in Respawnables.Values)
			{
				respawnable.Update(deltaTime);
			}
		}
	}
}

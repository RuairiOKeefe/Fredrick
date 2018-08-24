using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision;

namespace Fredrick.src
{
	public sealed class ColliderManager
	{
		private static ColliderManager instance = null;
		private static readonly object padlock = new object();

		public static ColliderManager Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new ColliderManager();
					}
					return instance;
				}
			}
		}

		const int WIDTH = 1000;
		const int HEIGHT = 1000;

		private World _world;

		private List<Entity>[,] _terrain;
		private List<AABBCollider> _colliders;
		private List<Platform> _platforms;

		public World World
		{
			get { return _world; }
			set { value = _world; }
		}

		public List<AABBCollider> Colliders
		{
			get { return _colliders; }
			set { _colliders = value; }
		}

		public List<Platform> Platforms
		{
			get { return _platforms; }
			set { _platforms = value; }
		}

		public List<Entity>[,] Terrain
		{
			get { return _terrain; }
			set { _terrain = value; }
		}

		public ColliderManager()
		{
			_terrain = new List<Entity>[WIDTH, HEIGHT];

			for (int x = 0; x < WIDTH; x++)
				for (int y = 0; y < HEIGHT; y++)
					_terrain[x, y] = new List<Entity>();

			_colliders = new List<AABBCollider>();
			_platforms = new List<Platform>();
			_world = new World(new Vector2(0, -9.8f));
		}

		public void Load()
		{

		}
		public void Update(double deltaTime)
		{
			_world.Step((float)deltaTime);
		}
	}
}

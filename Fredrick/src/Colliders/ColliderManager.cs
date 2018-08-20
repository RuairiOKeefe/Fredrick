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

		private World _world;

		private List<AABBCollider> _colliders;
		private List<Platform> _platforms;

		public World World
		{
			get { return _world; }
			set { value = _world; }
		}

		public List<AABBCollider> Colliders
		{
			get
			{
				return _colliders;
			}
			set
			{
				_colliders = value;
			}
		}

		public List<Platform> Platforms
		{
			get
			{
				return _platforms;
			}
			set
			{
				_platforms = value;
			}
		}

		public ColliderManager()
		{
			_colliders = new List<AABBCollider>();
			_platforms = new List<Platform>();
			_world = new World(new Vector2(0,-9.8f));
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

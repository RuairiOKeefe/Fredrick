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

		public void Load(ContentManager content)
		{
			for (int i = 0; i < NUM_PROJECTILES; i++)
			{
				Entity e = new Entity(Resources.Instance.ProjectileEntities["FragNade"]);
				_inactiveProjectiles.Push(e);

				e.Load(content);
			}
		}

		public ProjectileBuffer()
		{
			_inactiveProjectiles = new Stack<Entity>(NUM_PROJECTILES);

		}
	}
}
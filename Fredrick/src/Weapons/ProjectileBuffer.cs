using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public class ProjectileBuffer
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

		public const int NUM_PROJECTILES = 2000000;

		private List<Projectile> _activeProjectiles;
		private Stack<Projectile> _inactiveProjectiles;

		public List<Projectile> ActiveProjectiles
		{
			get { return _activeProjectiles; }
			set { _activeProjectiles = value; }
		}

		public Stack<Projectile> InactiveProjectiles
		{
			get { return _inactiveProjectiles; }
			set { _inactiveProjectiles = value; }
		}

		public void CreateProjectile()
		{
			Projectile p = _inactiveProjectiles.Pop();
			_activeProjectiles.Add(p);
		}

		public void Update()
		{

		}

		public ProjectileBuffer()
		{
			_inactiveProjectiles = new Stack<Projectile>(NUM_PROJECTILES);
			for (int i = 0; i < NUM_PROJECTILES; i++)
			{
				_inactiveProjectiles.Push(new Projectile());
			}
		}
	}
}
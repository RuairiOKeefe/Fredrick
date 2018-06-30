using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		private List<RectangleF> _colliders;
		private List<Platform> _platforms;

		public List<RectangleF> Colliders
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
			_colliders = new List<RectangleF>();
			_platforms = new List<Platform>();
			Platform p = new Platform(new Microsoft.Xna.Framework.Vector2(5, -1), 1, 1, 0, 0, 0, 1, 0.5f);
			_platforms.Add(p);
		}
	}
}

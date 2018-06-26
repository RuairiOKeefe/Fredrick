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

		public ColliderManager()
		{
			_colliders = new List<RectangleF>();
		}
	}
}

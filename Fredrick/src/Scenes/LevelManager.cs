using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src.Scenes
{
	/// <summary>
	/// This class may not be necessary later, however for now this is required to access lights
	/// </summary>
	public sealed class LevelManager
	{
		private static LevelManager _instance = null;
		private static readonly object _padlock = new object();

		public static LevelManager Instance
		{
			get
			{
				lock (_padlock)
				{
					if (_instance == null)
						_instance = new LevelManager();
					return _instance;
				}
			}
		}

		public Level CurrentLevel;

	}
}

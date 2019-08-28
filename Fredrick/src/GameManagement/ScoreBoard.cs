using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	class ScoreBoard
	{
		public List<ScoreTracker> ScoreTrackers;

		public ScoreBoard()
		{
			ScoreTrackers = new List<ScoreTracker>();

		}

		public void Update()
		{
			foreach(ScoreTracker scoreTracker in ScoreTrackers)
			{
				scoreTracker.Update();
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public class ScoreTracker
	{
		public int Score { get; set; }
		public List<PointTracker> PointTrackers;
		List<string> metadata;

		public ScoreTracker()
		{
			Score = 0;
			PointTrackers = new List<PointTracker>();
			metadata = new List<string>();
		}

		public void Update()
		{
			foreach (PointTracker tracker in PointTrackers)
			{
				foreach (string s in tracker.Activations)
				{
					Score += tracker.Change;
					metadata.Add(s);
				}
				tracker.Activations.Clear();
			}
		}
	}
}

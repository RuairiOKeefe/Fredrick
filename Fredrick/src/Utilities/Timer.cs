using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public class Timer
	{
		public double Duration;
		public bool IsTimeUp;
		public double RemainingTime;

		public Timer(double duration)
		{
			Duration = duration;
			Restart();
		}

		public void Update(double deltaTime)
		{
			if (!IsTimeUp)
			{
				RemainingTime -= deltaTime;
				if (RemainingTime <= 0)
				{
					IsTimeUp = true;
				}
			}
		}

		public void Restart()
		{
			RemainingTime = Duration;
			IsTimeUp = false;
		}
	}
}

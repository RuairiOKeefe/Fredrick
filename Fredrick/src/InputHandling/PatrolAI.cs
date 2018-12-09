using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	[Serializable]
	public class PatrolAI : Controller
	{
		private double _timer;

		public PatrolAI(Entity owner, String id, bool active = true) : base(owner, id, active)
		{

		}

		protected override void SetMovement()
		{
			_timer+=0.02;
			Movement = 0;
			if (_timer > 2)
				_timer = -2;
			if (_timer > 0)
			{
				Movement--;
			}
			else
			{
				Movement++;
			}
		}
	}
}

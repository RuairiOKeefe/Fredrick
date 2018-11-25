using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public abstract class StatusEffect
	{
		public float Magnitude { get; set; }
		public double Duration { get; set; }

		public abstract void Begin(ref Entity target);
		public abstract void End(ref Entity target);
		public abstract void Tick(ref Entity target);
	}
}

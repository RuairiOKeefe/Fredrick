using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public class Attack
	{
		public enum DamageType
		{
			Kinetic,
			Fire,
			Electrical
		}

		public enum DamageEffect
		{
			Instant,
			OverTime,
		}

		public DamageType Type;
		public DamageEffect Effect;
		public float Damage;
		public float Duration;
	}
}

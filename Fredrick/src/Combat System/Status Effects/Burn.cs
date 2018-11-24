using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public class Burn : StatusEffect
	{
		new public const float Magnitude = 10.0f;
		new public const double Duration = 5.0;

		public override void Begin(ref Entity target)
		{

		}

		public override void End(ref Entity target)
		{

		}

		public override void Tick(ref Entity target)
		{
			if (target.GetComponent<Damageable>() != null)
			{
				target.GetComponent<Damageable>().DealDamage(new Attack(Attack.DamageType.Fire, null, Magnitude));
			}
		}
	}
}

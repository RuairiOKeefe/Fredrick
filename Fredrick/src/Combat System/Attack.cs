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

		public DamageType Type { get; set; }
		public List<StatusEffect> StatusEffects { get; set; }
		public float Damage { get; set; }

		public Attack(DamageType type, List<StatusEffect> statusEffects, float damage)
		{
			Type = type;
			StatusEffects = new List<StatusEffect>();
			if(statusEffects !=null)
			{
				StatusEffects = statusEffects;
			}
			Damage = damage;
		}
	}
}

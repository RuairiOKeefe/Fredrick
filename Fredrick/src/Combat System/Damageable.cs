using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	[Serializable]
	public class Damageable : Component
	{
		[Serializable]
		public struct Resistances//Multipliers applied to each attack of a given type
		{
			public float Kinetic;
			public float Fire;
			public float Electrical;

			public Resistances(float kinetic, float fire, float electrical)
			{
				Kinetic = kinetic;
				Fire = fire;
				Electrical = electrical;
			}
		}

		public float Health { get; set; }
		public float MaxHealth { get; set; }
		public float HealthRegen { get; set; }
		public double HealthRegenDelay { get; set; }
		public double HealthRegenTimer { get; set; }
		public Resistances BaseResistance { get; set; }//Only used if armour is depleted

		public float ArmourThreshold { get; set; }//How much damage must be dealt in order to deal health damage
		public float ArmourHealth { get; set; }
		public float ArmourHealthMax { get; set; }
		public float ArmourRegen { get; set; }
		public double ArmourRegenDelay { get; set; }
		public double ArmourRegenTimer { get; set; }
		public Resistances ArmourResistance { get; set; }//Used instead of base resistances if armour is active

		public float ShieldThreshold { get; set; }//Shields should have a negative threshold, providing a base amount of damage that will only be dealt to the sheild each time damage is dealt
		public float ShieldHealth { get; set; }
		public float ShieldHealthMax { get; set; }
		public float ShieldRegen { get; set; }
		public double ShieldRegenDelay { get; set; }
		public double ShieldRegenTimer { get; set; }
		public Resistances ShieldResistance { get; set; }

		public Damageable()
		{
			BaseResistance = new Resistances();
			ArmourResistance = new Resistances();
			ShieldResistance = new Resistances();
		}

		public Damageable(Entity owner, string id) : base(owner, id)
		{
			BaseResistance = new Resistances();
			ArmourResistance = new Resistances();
			ShieldResistance = new Resistances();
		}

		public Damageable(Entity owner, Damageable original) : base(owner, original.Id)
		{
			Health = original.Health;
			MaxHealth = original.MaxHealth;
			HealthRegen = original.HealthRegen;
			HealthRegenDelay = original.HealthRegenDelay;
			HealthRegenTimer = original.HealthRegenTimer;
			BaseResistance = original.BaseResistance;
			ArmourThreshold = original.ArmourThreshold;
			ArmourHealth = original.ArmourHealth;
			ArmourHealthMax = original.ArmourHealthMax;
			ArmourRegen = original.ArmourRegen;
			ArmourRegenDelay = original.ArmourRegenDelay;
			ArmourRegenTimer = original.ArmourRegenTimer;
			ArmourResistance = original.ArmourResistance;
			ShieldThreshold = original.ShieldThreshold;
			ShieldHealth = original.ShieldHealth;
			ShieldHealthMax = original.ShieldHealthMax;
			ShieldRegen = original.ShieldRegen;
			ShieldRegenDelay = original.ShieldRegenDelay;
			ShieldRegenTimer = original.ShieldRegenTimer;
			ShieldResistance = original.ShieldResistance;
		}

		private float Resist(Resistances resistances, Attack.DamageType type, float damage)
		{
			switch (type)
			{
				case (Attack.DamageType.Kinetic):
					return damage *= resistances.Kinetic;
				case (Attack.DamageType.Fire):
					return damage *= resistances.Fire;
				case (Attack.DamageType.Electrical):
					return damage *= resistances.Electrical;
				default:
					return damage;
			}
		}

		public void DealDamage(Attack attack)
		{
			float remainingDamage = attack.Damage;
			float actualDamage;

			if (ShieldHealth > 0)
			{
				actualDamage = Resist(ShieldResistance, attack.Type, remainingDamage);
				if (actualDamage > 0)
				{
					if (ShieldThreshold < 0)
					{
						ShieldHealth += ShieldThreshold;
						ShieldHealth = ShieldHealth < 0 ? 0 : ShieldHealth;//Don't allow threshold damage to overflow 
						ShieldHealth -= actualDamage;
					}
					else
					{
						float unblockedDamage = actualDamage - ShieldThreshold;
						unblockedDamage = unblockedDamage < 0 ? unblockedDamage : 0;//If damage did not surpass threshold, no damage is taken
						ShieldHealth -= unblockedDamage;
					}
				}
				remainingDamage = ShieldHealth < 0 ? -ShieldHealth : 0;
			}

			if (ArmourHealth > 0)
			{
				actualDamage = Resist(ArmourResistance, attack.Type, remainingDamage);
				if (actualDamage > 0)
				{
					if (ArmourThreshold < 0)//If armour is negative the resistances will still apply and armour is broken in the same manner but no health damage is "blocked"
					{
						ArmourHealth += ArmourThreshold;
						Health -= actualDamage;
					}
					else
					{
						float unblockedDamage = actualDamage - ArmourThreshold;
						unblockedDamage = unblockedDamage < 0 ? unblockedDamage : 0;//If damage did not surpass threshold, no damage is taken
						float blockedDamage = actualDamage - unblockedDamage;
						ArmourHealth -= blockedDamage;//Armour takes damage equal to the amount blocked
						Health -= unblockedDamage;
					}
				}
			}
			else//Take pure health damage
			{
				actualDamage = Resist(BaseResistance, attack.Type, remainingDamage);
				if (actualDamage > 0)
				{
					Health -= actualDamage;
				}
			}

			if (attack.StatusEffects.Count > 0 && _owner.GetComponent<StatusHandler>() != null)
			{
				foreach (StatusEffect s in attack.StatusEffects)
					_owner.GetComponent<StatusHandler>().AddStatus(s.Copy());
			}
		}

		public override void Load(ContentManager content)
		{

		}

		public override void Unload()
		{

		}

		public override void Update(double deltaTime)
		{



		}

		public override void Draw(SpriteBatch spriteBatch)
		{

		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{

		}

		public override Component Copy(Entity owner)
		{
			return new Damageable(owner, this);
		}
	}
}

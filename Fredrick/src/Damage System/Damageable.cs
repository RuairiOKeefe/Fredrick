using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	class Damageable : Component
	{
		public struct Resistances//Multipliers applied to each attack of a given type
		{
			public float Kinetic;
			public float Fire;
			public float Electrical;
		}

		private float _health;
		private float _maxHealth;
		private float _healthRegen;
		private double _healthRegenDelay;
		private double _healthRegenTimer;
		private Resistances _baseResistance;//Only used if armour is depleted

		private float _armourThreshold;//How much damage must be dealt in order to deal health damage
		private float _armourHealth;
		private float _armourHealthMax;
		private float _armourRegen;
		private double _armourRegenDelay;
		private double _armourRegenTimer;
		private Resistances _armourResistance;//Used instead of base resistances if armour is active

		private float _sheildThreshold;//Sheilds should have a negative threshold, providing a base amount of damage that will only be dealt to the sheild each time damage is dealt
		private float _sheildHealth;
		private float _sheildHealthMax;
		private float _sheildRegen;
		private double _sheildRegenDelay;
		private double _sheildRegenTimer;
		private Resistances _sheildResistance;

		private List<Attack> DoTAttacks;

		public Damageable(Entity owner, string id) : base(owner, id)
		{
			_baseResistance = new Resistances();
			_armourResistance = new Resistances();
			_sheildResistance = new Resistances();
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

		public void DealDamage(Attack attack)//currently only for direct damage
		{
			float remainingDamage = attack.Damage;
			float actualDamage;

			if (_sheildHealth > 0)
			{
				actualDamage = Resist(_sheildResistance, attack.Type, remainingDamage);
				if (actualDamage > 0)
				{
					if (_sheildThreshold < 0)
					{
						_sheildHealth += _sheildThreshold;
						_sheildHealth = _sheildHealth < 0 ? 0 : _sheildHealth;//Don't allow threshold damage to overflow 
						_sheildHealth -= actualDamage;
					}
					else
					{
						float unblockedDamage = actualDamage - _sheildThreshold;
						unblockedDamage = unblockedDamage < 0 ? unblockedDamage : 0;//If damage did not surpass threshold, no damage is taken
						_sheildHealth -= unblockedDamage;
					}
				}
				remainingDamage = _sheildHealth < 0 ? -_sheildHealth : 0;
			}

			if (_armourHealth > 0)
			{
				actualDamage = Resist(_armourResistance, attack.Type, remainingDamage);
				if (actualDamage > 0)
				{
					if (_armourThreshold < 0)//If armour is negative the resistances will still apply and armour is broken in the same manner but no health damage is "blocked"
					{
						_armourHealth += _armourThreshold;
						_health -= actualDamage;
					}
					else
					{
						float unblockedDamage = actualDamage - _armourThreshold;
						unblockedDamage = unblockedDamage < 0 ? unblockedDamage : 0;//If damage did not surpass threshold, no damage is taken
						float blockedDamage = actualDamage - unblockedDamage;
						_armourHealth -= blockedDamage;//Armour takes damage equal to the amount blocked
						_health -= unblockedDamage;
					}
				}

			}
			else//Take pure health damage
			{
				actualDamage = Resist(_baseResistance, attack.Type, remainingDamage);
				if (actualDamage > 0)
				{
					_health -= actualDamage;
				}
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
	}
}

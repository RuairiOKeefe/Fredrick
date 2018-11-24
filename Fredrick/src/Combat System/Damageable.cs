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

		private float _shieldThreshold;//Shields should have a negative threshold, providing a base amount of damage that will only be dealt to the sheild each time damage is dealt
		private float _shieldHealth;
		private float _shieldHealthMax;
		private float _shreldRegen;
		private double _shieldRegenDelay;
		private double _shieldRegenTimer;
		private Resistances _shieldResistance;

		private Dictionary<string, Attack> _DoTAttacks;

		public Damageable(Entity owner, string id) : base(owner, id)
		{
			_baseResistance = new Resistances();
			_armourResistance = new Resistances();
			_shieldResistance = new Resistances();
			_DoTAttacks = new Dictionary<string, Attack>();
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

			if (_shieldHealth > 0)
			{
				actualDamage = Resist(_shieldResistance, attack.Type, remainingDamage);
				if (actualDamage > 0)
				{
					if (_shieldThreshold < 0)
					{
						_shieldHealth += _shieldThreshold;
						_shieldHealth = _shieldHealth < 0 ? 0 : _shieldHealth;//Don't allow threshold damage to overflow 
						_shieldHealth -= actualDamage;
					}
					else
					{
						float unblockedDamage = actualDamage - _shieldThreshold;
						unblockedDamage = unblockedDamage < 0 ? unblockedDamage : 0;//If damage did not surpass threshold, no damage is taken
						_shieldHealth -= unblockedDamage;
					}
				}
				remainingDamage = _shieldHealth < 0 ? -_shieldHealth : 0;
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

		public void DealDamageOverTime(Attack attack, double deltaTime)
		{
			float remainingDamage = attack.Damage;
			float actualDamage;

			if (_shieldHealth > 0)
			{
				actualDamage = Resist(_shieldResistance, attack.Type, remainingDamage);
				if (actualDamage > 0)
				{
					if (_shieldThreshold < 0)
					{
						_shieldHealth += _shieldThreshold;
						_shieldHealth = _shieldHealth < 0 ? 0 : _shieldHealth;//Don't allow threshold damage to overflow 
						_shieldHealth -= actualDamage;
					}
					else
					{
						float unblockedDamage = actualDamage - _shieldThreshold;
						unblockedDamage = unblockedDamage < 0 ? unblockedDamage : 0;//If damage did not surpass threshold, no damage is taken
						_shieldHealth -= unblockedDamage;
					}
				}
				remainingDamage = _shieldHealth < 0 ? -_shieldHealth : 0;
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

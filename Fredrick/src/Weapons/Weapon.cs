using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Fredrick.src
{
	[Serializable]
	public class Weapon : Component
	{
		public Drawable WeaponDrawable { get; set; }

		public List<StatusEffect> ImpactEffects { get; set; }

		public List<StatusEffect> AreaEffects { get; set; }

		protected Vector2 _spotSpawn;
		protected Vector2 _weaponPosition;//The arm has uses the base transform
		protected Vector2 _transformedWeaponPosition;

		protected bool m_continuous;

		protected string m_projectile;

		protected bool m_facingRight;

		protected double m_fireRate;//How long between shots

		protected Attack m_impactAttack;

		protected Attack m_areaAttack;

		protected float m_projectileSpeed;

		protected float m_areaOfEffectRadius;

		protected float m_impactKnockback;

		protected float m_areaKnockback;

		protected double m_fuseTimer;

		protected bool m_objectImpactTrigger;

		protected bool m_actorImpactTrigger;

		private double m_nextfire;//Counter till next shot can be fired

		public Weapon()
		{

		}

		public Weapon(Entity owner, string id, Vector2 shotSpawn, Vector2 weaponPosition, bool continuous = true, List<string> tags = null, bool active = true) : base(owner, id, tags, active)
		{
			_spotSpawn = shotSpawn;
			_weaponPosition = weaponPosition;

			m_continuous = continuous;

			Scale = new Vector2(1);

			m_facingRight = true;
		}

		public Weapon(Entity owner, Weapon original) : base(owner, original.Id, original.Tags, original.Active)
		{
			Position = original.Position;
			_spotSpawn = original._spotSpawn;
			_weaponPosition = original._weaponPosition;

			m_impactAttack = original.m_impactAttack;
			m_areaAttack = original.m_areaAttack;
			m_fireRate = original.m_fireRate;
			m_projectileSpeed = original.m_projectileSpeed;
			m_areaOfEffectRadius = original.m_areaOfEffectRadius;
			m_impactKnockback = original.m_impactKnockback;
			m_areaKnockback = original.m_areaKnockback;
			m_fuseTimer = original.m_fuseTimer;
			m_objectImpactTrigger = original.m_objectImpactTrigger;
			m_actorImpactTrigger = original.m_actorImpactTrigger;

			m_continuous = original.m_continuous;

			Scale = original.Scale;

			WeaponDrawable = original.WeaponDrawable;

			m_facingRight = true;
		}

		public void InitialiseAttack(Attack impactAttack, Attack areaAttack, float fireRate, float projectileSpeed, float areaOfEffectRadius, float impactKnockback, float areaKnockback, double fuseTimer, bool objectImpactTrigger, bool actorImpactTrigger)
		{
			m_impactAttack = impactAttack;

			m_areaAttack = areaAttack;

			m_fireRate = fireRate;

			m_projectileSpeed = projectileSpeed;

			m_areaOfEffectRadius = areaOfEffectRadius;

			m_impactKnockback = impactKnockback;

			m_areaKnockback = areaKnockback;

			m_fuseTimer = fuseTimer;

			m_objectImpactTrigger = objectImpactTrigger;

			m_actorImpactTrigger = actorImpactTrigger;
		}

		public void Fire(Vector2 direction, float sin, float cos, CharacterRig armsRig)
		{
			Entity e = ProjectileBuffer.Instance.InactiveProjectiles.Pop();

			float tssx = _spotSpawn.X;
			float tssy = _spotSpawn.Y;

			Vector2 transformedShotSpawn = new Vector2((cos * tssx) - (sin * tssy), (sin * tssx) + (cos * tssy));

			e.Position = _owner.Position + Position + transformedShotSpawn;

			Vector2 shotVelocity = direction * m_projectileSpeed;
			e.GetComponent<Projectile>().InitialiseAttack(m_impactAttack, m_areaAttack, m_projectileSpeed, m_areaOfEffectRadius, m_impactKnockback, m_areaKnockback, m_fuseTimer, m_objectImpactTrigger, m_actorImpactTrigger);
			e.GetComponent<Projectile>().Revive(shotVelocity, m_fuseTimer);

			ProjectileBuffer.Instance.ActiveProjectiles.Add(e);

			m_nextfire = m_fireRate;


			if (armsRig != null)
			{
				armsRig.RestartAnim();
				armsRig.SetOverrideRotation(Rotation, false);
			}
		}

		public override void Load(ContentManager content)
		{
			WeaponDrawable.Load(content);
		}

		public override void Unload()
		{

		}

		public override void Update(double deltaTime)
		{
			bool fireCommand = false;

			CharacterRig armsRig = Owner.GetComponent<CharacterRig>(null, "Arms");
			Vector2 origin = armsRig != null ? armsRig.Position : new Vector2(0);

			Vector2 direction;
			if (_owner.GetDerivedComponent<Controller>() != null)
			{
				direction = _owner.GetDerivedComponent<Controller>().GetAim(origin);
				if (m_continuous)
					fireCommand = _owner.GetDerivedComponent<Controller>().FireHeld;
				else
					fireCommand = _owner.GetDerivedComponent<Controller>().FirePressed;
			}
			else
			{
				direction = new Vector2(1, 0);
				fireCommand = false;
			}

			direction.Normalize();

			Rotation = (float)Math.Atan2(-direction.Y, direction.X);

			float sin = (float)Math.Sin(-Rotation);
			float cos = (float)Math.Cos(-Rotation);

			float twpx = _weaponPosition.X;
			float twpy = _weaponPosition.Y;

			_transformedWeaponPosition = new Vector2((cos * twpx) - (sin * twpy), (sin * twpx) + (cos * twpy));

			if (m_nextfire <= 0)
			{
				if (fireCommand)
				{
					Fire(direction, sin, cos, armsRig);
				}
			}
			else
			{
				m_nextfire -= deltaTime;
			}
		}

		public override void DrawBatch(SpriteBatch spriteBatch)
		{
			Vector2 inv = new Vector2(1, -1);
			//spriteBatch.Draw(ResourceManager.Instance.Textures[WeaponDrawable._spriteName], (_transformedWeaponPosition + Position + _owner.Position) * inv * WeaponDrawable._spriteSize, WeaponDrawable._sourceRectangle, WeaponDrawable._colour, _owner.Rotation + Rotation, WeaponDrawable._origin, Scale, WeaponDrawable._spriteEffects, WeaponDrawable._layer);
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{

		}

		public override Component Copy(Entity owner)
		{
			return new Weapon(owner, this);
		}
	}
}

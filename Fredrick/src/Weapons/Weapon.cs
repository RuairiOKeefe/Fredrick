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

		protected float m_impactDamage;

		protected float m_areaDamage;

		protected float m_projectileSpeed;

		protected float m_areaOfEffectRadius;

		protected bool m_objectImpactTrigger;

		protected bool m_actorImpactTrigger;

		private double m_nextfire;//Counter till next shot can be fired

		public Weapon()
		{

		}

		public Weapon(Entity owner, string id, Vector2 shotSpawn, Vector2 weaponPosition, bool continuous = true, bool active = true) : base(owner, id, active)
		{
			_spotSpawn = shotSpawn;
			_weaponPosition = weaponPosition;

			m_continuous = continuous;

			Scale = new Vector2(1);

			m_facingRight = true;
		}

		public Weapon(Entity owner, Weapon original) : base(owner, original.Id, original.Active)
		{
			_spotSpawn = original._spotSpawn;
			_weaponPosition = original._weaponPosition;
			m_fireRate = original.m_fireRate;
			m_impactDamage = original.m_impactDamage;
			m_areaDamage = original.m_areaDamage;
			m_projectileSpeed = original.m_projectileSpeed;

			m_continuous = original.m_continuous;

			Scale = original.Scale;

			WeaponDrawable = original.WeaponDrawable;

			m_facingRight = true;
		}

		public void InitialiseAttack(float impactDamage, float areaDamage, float fireRate, float projectileSpeed, float areaOfEffectRadius, bool objectImpactTrigger, bool actorImpactTrigger)
		{
			m_impactDamage = impactDamage;

			m_areaDamage = areaDamage;

			m_fireRate = fireRate;

			m_projectileSpeed = projectileSpeed;

			m_areaOfEffectRadius = areaOfEffectRadius;

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
			e.GetComponent<Projectile>().Revive(shotVelocity, 2.0, true, false, 10.0f, 20.0f, 3.0f, 1.0f);

			ProjectileBuffer.Instance.ActiveProjectiles.Add(e);

			m_nextfire = m_fireRate;


			if (armsRig != null)
			{
				armsRig.RestartAnim();
				armsRig.SetOverrideRotation("Throwing", Rotation, 0, 1);
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

		public override void Draw(SpriteBatch spriteBatch)
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

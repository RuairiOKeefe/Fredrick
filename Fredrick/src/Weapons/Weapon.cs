using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Fredrick.src.Rigging;

namespace Fredrick.src
{
	[Serializable]
	public class Weapon : Component
	{
		public override bool IsDrawn { get { return true; } }

		public Drawable WeaponDrawable { get; set; }

		public List<StatusEffect> ImpactEffects { get; set; }

		public List<StatusEffect> AreaEffects { get; set; }

		public string Projectile { get; set; }

		public List<Emitter> FireEmitters = new List<Emitter>();

		protected Vector2 _spotSpawn;
		protected Vector2 _weaponPosition;//The arm has uses the base transform
		protected Vector2 _handPosition;//The positon that hands should be placed at
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

		protected float m_screenshake;

		protected double m_fuseTimer;

		protected bool m_objectImpactTrigger;

		protected bool m_actorImpactTrigger;

		private double m_nextfire;//Counter till next shot can be fired

		private Random m_rng = new Random();

		private int m_shots = 1;

		private float m_spread = 0.02f;

		private float m_perShotRecoil = 0.02f;

		private float m_maxRecoil = 0.03f;

		private float m_recoilDecay = 0.02f;

		private float m_currentRecoil;

		public Weapon()
		{

		}

		public Weapon(Entity owner, string id, string projectile, Vector2 shotSpawn, Vector2 weaponPosition, Vector2 handPosition, bool continuous = true, List<string> tags = null, bool active = true) : base(owner, id, tags, active)
		{
			Projectile = projectile;
			_spotSpawn = shotSpawn;
			_weaponPosition = weaponPosition;
			_handPosition = handPosition;

			m_continuous = continuous;

			Scale = new Vector2(1);

			m_facingRight = true;
		}

		public Weapon(Entity owner, Weapon original) : base(owner, original.Id, original.Tags, original.Active)
		{
			Position = original.Position;
			Projectile = original.Projectile;
			_spotSpawn = original._spotSpawn;
			_weaponPosition = original._weaponPosition;
			_handPosition = original._handPosition;

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

			foreach (Emitter e in original.FireEmitters)
			{
				FireEmitters.Add(new Emitter(owner, e));
			}
		}

		public void InitialiseAttack(Attack impactAttack, Attack areaAttack, float fireRate, float projectileSpeed, float areaOfEffectRadius, float impactKnockback, float areaKnockback, float screenshake, double fuseTimer, bool objectImpactTrigger, bool actorImpactTrigger)
		{
			m_impactAttack = impactAttack;

			m_areaAttack = areaAttack;

			m_fireRate = fireRate;

			m_projectileSpeed = projectileSpeed;

			m_areaOfEffectRadius = areaOfEffectRadius;

			m_impactKnockback = impactKnockback;

			m_areaKnockback = areaKnockback;

			m_screenshake = screenshake;

			m_fuseTimer = fuseTimer;

			m_objectImpactTrigger = objectImpactTrigger;

			m_actorImpactTrigger = actorImpactTrigger;
		}

		public void Fire(Vector2 direction, float sin, float cos, CharacterRig armsRig)
		{

			float tssx = _spotSpawn.X;
			float tssy = _spotSpawn.Y;

			Vector2 transformedShotSpawn = new Vector2((cos * tssx) - (sin * tssy), (sin * tssx) + (cos * tssy));

			for (int i = 0; i < m_shots; i++)
			{
				Entity e = ProjectileBuffer.Instance.Pop(Projectile);

				e.Position = _owner.Position + Position + transformedShotSpawn;


				double angle = ((m_rng.NextDouble() * 2) - 1) * (m_spread + m_currentRecoil);

				float s = (float)Math.Sin(angle);
				float c = (float)Math.Cos(angle);

				Vector2 fireDirection = new Vector2((c * direction.X) - (s * direction.Y), (s * direction.X) + (c * direction.Y));

				Vector2 shotVelocity = fireDirection * m_projectileSpeed;
				e.GetComponent<Projectile>().InitialiseAttack(m_impactAttack, m_areaAttack, m_projectileSpeed, m_areaOfEffectRadius, m_impactKnockback, m_areaKnockback, m_screenshake, m_fuseTimer, m_objectImpactTrigger, m_actorImpactTrigger);
				e.GetComponent<Projectile>().Revive(shotVelocity, m_fuseTimer);

				foreach (Component component in e.Components)
				{
					if (component is Emitter)
						(component as Emitter).Revive();
				}


				ProjectileBuffer.Instance.Add(Projectile, e);
			}
			foreach (Emitter emitter in FireEmitters)
			{
				emitter.Position = Position + transformedShotSpawn;
				emitter.Rotation = Rotation;
				emitter.Emit();
			}

			m_currentRecoil += m_perShotRecoil;
			m_currentRecoil = m_currentRecoil <= m_maxRecoil ? m_currentRecoil : m_maxRecoil;

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
			ShaderId = WeaponDrawable.ShaderInfo.ShaderId;
			foreach (Emitter e in FireEmitters)
			{
				e.Owner = this._owner;
				e.Load(content);
			}
		}

		public override void Unload()
		{

		}

		public override void Update(double deltaTime)
		{
			bool fireCommand = false;

			m_currentRecoil -= m_recoilDecay * (float)deltaTime;
			m_currentRecoil = m_currentRecoil > 0 ? m_currentRecoil : 0;

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

			float gunRecoil = (float)(m_nextfire / m_fireRate) * m_maxRecoil;

			Rotation = (float)Math.Atan2(direction.Y, direction.X) + gunRecoil;

			float sin = (float)Math.Sin(Rotation);
			float cos = (float)Math.Cos(Rotation);

			float twpx = _weaponPosition.X - gunRecoil;
			float twpy = _weaponPosition.Y + gunRecoil;

			_transformedWeaponPosition = new Vector2((cos * twpx) - (sin * twpy), (sin * twpx) + (cos * twpy));

			float hpx = _handPosition.X - gunRecoil;
			float hpy = _handPosition.Y + gunRecoil;

			Vector2 transformedHandPosition = new Vector2((cos * hpx) - (sin * hpy), (sin * hpx) + (cos * hpy));

			if (_owner.GetComponent<IKSolver>() != null)
			{
				_owner.GetComponent<IKSolver>().SetTarget(_transformedWeaponPosition + transformedHandPosition + Position + _owner.Position);
			}

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

		public override void Draw(SpriteBatch spriteBatch, Effect shader, Matrix transformationMatrix)
		{
			if (WeaponDrawable.ShaderInfo != null)
				WeaponDrawable.ShaderInfo.SetUniforms(shader, _owner.Rotation + Rotation);

			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, shader, transformationMatrix);
			Vector2 inv = new Vector2(1, -1);
			spriteBatch.Draw(ResourceManager.Instance.Textures[WeaponDrawable._spriteName], (_transformedWeaponPosition + Position + _owner.Position) * inv * WeaponDrawable._spriteSize, WeaponDrawable._sourceRectangle, WeaponDrawable._colour, -(_owner.Rotation + Rotation), WeaponDrawable._origin, Scale, WeaponDrawable._spriteEffects, WeaponDrawable._layer);
			spriteBatch.End();
		}

		public override void DrawBatch(SpriteBatch spriteBatch)
		{
			Vector2 inv = new Vector2(1, -1);
			spriteBatch.Draw(ResourceManager.Instance.Textures[WeaponDrawable._spriteName], (_transformedWeaponPosition + Position + _owner.Position) * inv * WeaponDrawable._spriteSize, WeaponDrawable._sourceRectangle, WeaponDrawable._colour, -(_owner.Rotation + Rotation), WeaponDrawable._origin, Scale, WeaponDrawable._spriteEffects, WeaponDrawable._layer);
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

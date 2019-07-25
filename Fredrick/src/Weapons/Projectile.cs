using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace Fredrick.src
{
	[Serializable]
	public class Projectile : Movable
	{
		//For explosion aoe
		private Body _body;
		private CircleShape _circle;
		private Fixture _fixture;


		protected Attack m_impactAttack;

		protected Attack m_areaAttack;

		protected float m_projectileSpeed;

		protected float m_areaOfEffectRadius;

		protected bool m_objectImpactTrigger;

		protected bool m_actorImpactTrigger;

		protected float m_impactKnockback;

		protected float m_areaKnockback;

		protected double m_fuseTimer;

		/// <summary>
		/// Is the projectile finished
		/// </summary>
		public bool Dead { get; set; }

		public Projectile(Entity owner) : base(owner)
		{
			Position = new Vector2();
			Velocity = new Vector2();
		}

		public Projectile(Entity owner, string id, Vector2 velocity, double fuseTimer) : base(owner, id)
		{
			Velocity = velocity;
			m_fuseTimer = fuseTimer;
		}

		public Projectile(Entity owner, Projectile original) : base(owner, original.Id)
		{
			Velocity = original.Velocity;
			m_fuseTimer = original.m_fuseTimer;
		}

		public void InitialiseAttack(Attack impactAttack, Attack areaAttack, float projectileSpeed, float areaOfEffectRadius, float impactKnockback, float areaKnockback, double fuseTimer, bool objectImpactTrigger, bool actorImpactTrigger)
		{
			m_impactAttack = impactAttack;

			m_areaAttack = areaAttack;

			m_projectileSpeed = projectileSpeed;

			m_areaOfEffectRadius = areaOfEffectRadius;

			m_impactKnockback = impactKnockback;

			m_areaKnockback = areaKnockback;

			m_fuseTimer = fuseTimer;

			m_objectImpactTrigger = objectImpactTrigger;

			m_actorImpactTrigger = actorImpactTrigger;
		}

		public void Revive(Vector2 velocity, double fuseTimer)
		{

			m_fuseTimer = fuseTimer;

			if (_owner.GetComponent<CircleCollider>() != null)
			{
				_owner.GetComponent<CircleCollider>().Revive();
				_owner.GetComponent<CircleCollider>().ApplyForce(velocity);

			}
			else
			{
				Velocity = velocity;
			}

			Dead = false;

			_body = new Body(ColliderManager.Instance.World, _owner.Position, 0, BodyType.Dynamic);
			_circle = new CircleShape(m_areaOfEffectRadius, 1.0f);
			_circle.Position = Position;
			_fixture = _body.CreateFixture(_circle);

			_body.BodyType = BodyType.Dynamic;
			_body.UserData = _owner;
			_body.Awake = true;
			_body.Position = _owner.Position;

			_fixture.IsSensor = true;

			if (_owner.GetComponent<Renderable>() != null)
			{
				_owner.GetComponent<Renderable>().Drawable.TransitionAnim(0);
			}
		}

		public void MoveProjectile(double deltaTime)
		{
			_body.Position = _owner.Position;

			ResolveMotion(deltaTime);

			if (_owner.GetComponent<CircleCollider>() == null)
			{
				if (Velocity.Length() > 0)
				{
					Vector2 v = Velocity;
					v.Normalize();
					Rotation = (float)Math.Atan2(-v.Y, v.X);
				}
				else
				{
					Rotation = 0;
				}
				_owner.Rotation = Rotation;
			}
		}

		public void ImpactAttack(Entity target)
		{
			if (target.GetComponent<Damageable>() != null)
			{
				target.GetComponent<Damageable>().DealDamage(m_impactAttack);
			}
		}

		public void AreaAttack()
		{
			ContactEdge c = _body.ContactList;
			while (c != null && c.Next != null)
			{
				if (c.Contact.IsTouching)
				{
					Entity e = (Entity)c.Other.UserData;
					if (e.GetComponent<CircleCollider>() != null)
					{
						Vector2 force = e.Position - _owner.Position;
						force.Normalize();
						force *= m_areaKnockback;
						e.GetComponent<CircleCollider>().ApplyForce(force, _owner.Position);
					}
					if (e.GetComponent<Damageable>() != null)
					{
						e.GetComponent<Damageable>().DealDamage(m_areaAttack);
					}
				}
				c = c.Next;
			}
		}

		public void Detonate()
		{
			_owner.GetComponent<CircleCollider>().Kill();
			foreach (Component c in _owner.Components)
			{
				if (c is Emitter)
				{
					Emitter e = c as Emitter;
					e.Emit();
				}
			}
			if (_owner.GetComponent<Renderable>() != null)
			{
				_owner.GetComponent<Renderable>().Drawable.TransitionAnim(1);
			}

			AreaAttack();

			Dead = true;
		}

		public override void Update(double deltaTime)
		{
			MoveProjectile(deltaTime);

			m_fuseTimer -= deltaTime;

			bool detonate = false;

			if (_owner.GetComponent<CircleCollider>() != null)
			{
				CircleCollider collider = _owner.GetComponent<CircleCollider>();
				ContactEdge c = collider.Body.ContactList;

				while (c != null && c.Next != null)
				{
					if (c.Contact.IsTouching)
					{
						Entity e = (Entity)c.Other.UserData;
						if (e != Owner)
						{
							if (e.Tags.Contains("Actor") && m_actorImpactTrigger)
							{
								detonate = true;
							}
							if (!e.Tags.Contains("Actor") && m_objectImpactTrigger)
							{
								detonate = true;
							}

							ImpactAttack(e);
						}
					}
					c = c.Next;

				}
			}

			if (m_fuseTimer < 0)
			{
				detonate = true;
			}

			if (detonate)
			{
				Detonate();
			}

		}

		public override void Draw(SpriteBatch spriteBatch)
		{

		}

		public override Component Copy(Entity owner)
		{
			return new Projectile(owner, this);
		}
	}
}

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
		/// <summary>
		/// Does the projectile deal aoe damage when it terminates?
		/// </summary>
		private bool _explosive;
		/// <summary>
		/// Will the projectile terminate upon contact?
		/// </summary>
		private bool _contactTermination;

		private bool _detonated;

		private float _damage;
		private float _aoeDamage;

		//For explosion aoe
		private Body _body;
		private CircleShape _circle;
		private Fixture _fixture;

		private float _radius;
		private float _knockback;

		public double LifeTime { get; set; }

		/// <summary>
		/// Is the projectile finished
		/// </summary>
		public bool Dead { get; set; }

		public Attack Attack { get; set; }

		public Projectile(Entity owner) : base(owner)
		{
			Position = new Vector2();
			Velocity = new Vector2();
		}

		public Projectile(Entity owner, string id, Vector2 velocity, double lifeTime, bool explosive = false, bool contactTermination = true, float damage = 5.0f, float aoeDamage = 5.0f, float radius = 3.0f, float knockback = 1.0f) : base(owner, id)
		{
			Velocity = velocity;
			LifeTime = lifeTime;
			_explosive = explosive;
			_contactTermination = contactTermination;

			_damage = damage;
			_aoeDamage = aoeDamage;
			_radius = radius;
			_knockback = knockback;
		}

		public Projectile(Entity owner, Projectile original) : base(owner, original.Id)
		{
			Velocity = original.Velocity;
			LifeTime = original.LifeTime;
			_explosive = original._explosive;
			_contactTermination = original._contactTermination;

			_damage = original._damage;
			_aoeDamage = original._aoeDamage;
			_radius = original._radius;
			_knockback = original._knockback;
			Attack = original.Attack;
		}

		public void Revive(Vector2 velocity, double lifeTime, bool explosive = false, bool contactTermination = true, float damage = 5.0f, float aoeDamage = 5.0f, float radius = 3.0f, float knockback = 1.0f)
		{

			LifeTime = lifeTime;
			_explosive = explosive;
			_contactTermination = contactTermination;

			_damage = damage;
			_aoeDamage = aoeDamage;
			_radius = radius;
			_knockback = knockback;

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
			_detonated = false;

			_body = new Body(ColliderManager.Instance.World, _owner.Position, 0, BodyType.Dynamic);
			_circle = new CircleShape(_radius, 1.0f);
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

		public override void Update(double deltaTime)
		{
			LifeTime -= deltaTime;
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


			if (LifeTime < 0)
			{
				if (!_detonated)
				{
					if (_owner.GetComponent<CircleCollider>() != null)
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
									force *= _knockback;
									e.GetComponent<CircleCollider>().ApplyForce(force, _owner.Position);
								}
								if (e.GetComponent<Damageable>() != null)
								{
									e.GetComponent<Damageable>().DealDamage(Attack);
								}
							}
							c = c.Next;
						}

						_owner.GetComponent<CircleCollider>().Kill();
					}
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
					_detonated = true;
				}

				if (_owner.GetComponent<Emitter>() != null)
				{
					bool dead = true;
					foreach (Component c in _owner.Components)
					{
						if (c is Emitter)
						{
							Emitter e = c as Emitter;
							if (e.Particles.Count != 0)
							{
								dead = false;
							}
						}
					}
					Dead = dead;

				}
				else
				{
					Dead = true;
				}
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

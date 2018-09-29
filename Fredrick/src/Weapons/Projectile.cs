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

using System.Diagnostics;

namespace Fredrick.src
{
	[Serializable]
	public class Projectile : Movable
	{
		private double _lifeTime;
		/// <summary>
		/// Does the projectile deal aoe damage when it terminates?
		/// </summary>
		private bool _explosive;
		/// <summary>
		/// Will the projectile terminate upon contact?
		/// </summary>
		private bool _contactTermination;
		/// <summary>
		/// Is the projectile finished
		/// </summary>
		private bool _dead;
		private bool _detonated;

		float _damage;

		//For explosion aoe
		Body _body;
		CircleShape _circle;
		Fixture _fixture;

		float _radius;
		float _knockback;

		public double LifeTime
		{
			get { return _lifeTime; }
			set { _lifeTime = value; }
		}

		public bool Dead
		{
			get { return _dead; }
			set { _dead = value; }
		}

		public Projectile(Entity owner) : base(owner)
		{
			_position = new Vector2();
			_velocity = new Vector2();
		}

		public Projectile(Entity owner, Vector2 velocity, double lifeTime, bool explosive = false, bool contactTermination = true, float damage = 5.0f, float radius = 1.0f, float knockback = 1.0f) : base(owner)
		{
			_velocity = velocity;
			_lifeTime = lifeTime;
			_explosive = explosive;
			_contactTermination = contactTermination;

			_damage = damage;
			_radius = radius;
			_knockback = knockback;
		}

		public void Revive(Vector2 velocity, double lifeTime, bool explosive = false, bool contactTermination = true, float damage = 5.0f, float radius = 1.0f, float knockback = 1)
		{

			_lifeTime = lifeTime;
			_explosive = explosive;
			_contactTermination = contactTermination;

			_damage = damage;
			_radius = radius;
			_knockback = knockback;

			if (_owner.GetComponent<CircleCollider>() != null)
			{
				_owner.GetComponent<CircleCollider>().Revive();
				_owner.GetComponent<CircleCollider>().ApplyForce(velocity);

			}
			else
			{
				_velocity = velocity;
			}

			_dead = false;
			_detonated = false;

			_body = new Body(ColliderManager.Instance.World, _owner.Position, 0, BodyType.Dynamic);
			_circle = new CircleShape(_radius, 1.0f);
			_circle.Position = _position;
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
			_lifeTime -= deltaTime;
			_body.Position = _owner.Position;

			ResolveMotion(deltaTime);

			if (_owner.GetComponent<CircleCollider>() == null)
			{
				if (_velocity.Length() > 0)
				{
					Vector2 v = _velocity;
					v.Normalize();
					_rotation = (float)Math.Atan2(-v.Y, v.X);
				}
				else
				{
					_rotation = 0;
				}
				_owner.Rotation = _rotation;
			}


			if (_lifeTime < 0)
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
									Debug.Write(force.Length() + "\n");
									force.Normalize();
									force *= _knockback;
									e.GetComponent<CircleCollider>().ApplyForce(force, _owner.Position);
								}
							}
							c = c.Next;
						}

						_owner.GetComponent<CircleCollider>().Kill();
					}
					if (_owner.GetComponent<Emitter>() != null)
					{
						_owner.GetComponent<Emitter>().Emit();
					}
					if (_owner.GetComponent<Renderable>() != null)
					{
						_owner.GetComponent<Renderable>().Drawable.TransitionAnim(1);
					}
					_detonated = true;
				}

				if (_owner.GetComponent<Emitter>() != null)
				{
					if (_owner.GetComponent<Emitter>().Particles.Count == 0)
					{
						_dead = true;
					}
				}
				else
				{
					_dead = true;
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{

		}
	}
}

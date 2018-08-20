using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
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

		public Projectile(Entity owner, Vector2 velocity, double lifeTime, bool explosive = false, bool contactTermination = true) : base(owner)
		{
			_velocity = velocity;
			_lifeTime = lifeTime;
			_explosive = explosive;
			_contactTermination = contactTermination;
		}

		public void Revive(Vector2 velocity, double lifeTime, bool explosive = false, bool contactTermination = true)
		{

			_lifeTime = lifeTime;
			_explosive = explosive;
			_contactTermination = contactTermination;

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
		}

		public override void Update(double deltaTime)
		{
			_lifeTime -= deltaTime;

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
				_owner.SetRotation(_rotation);
			}


			if (_lifeTime < 0)
			{
				if (!_detonated)
				{
					if (_owner.GetComponent<CircleCollider>() != null)
					{
						_owner.GetComponent<CircleCollider>().Kill();
					}
					if (_owner.GetComponent<Emitter>() != null)
					{
						_owner.GetComponent<Emitter>().Emit();
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

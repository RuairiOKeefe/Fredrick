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
		protected Vector2 _spotSpawn;

		protected double _fireRate;//How long between shots
		protected double _nextfire;//Counter till next shot can be fired

		protected float _damage;
		protected float _aoeDamage;
		protected float _shotSpeed;

		protected bool _continuous;



		public Drawable _drawable;

		List<Entity> _projectiles;

		public double FireRate
		{
			get { return _fireRate; }
			set { _fireRate = value; }
		}

		public float Damage
		{
			get { return _damage; }
			set { _damage = value; }
		}

		public float AOEDamage
		{
			get { return _aoeDamage; }
			set { _aoeDamage = value; }
		}

		public float ShotSpeed
		{
			get { return _shotSpeed; }
			set { _shotSpeed = value; }
		}

		public Weapon(Entity owner, string id, Vector2 shotSpawn, double fireRate = 0.5, float damage = 10.0f, float aoeDamage = 20.0f, float shotSpeed = 5.0f, bool continuous = true) : base(owner, id)
		{
			_spotSpawn = shotSpawn;
			_fireRate = fireRate;
			_damage = damage;
			_aoeDamage = aoeDamage;
			_shotSpeed = shotSpeed;

			_continuous = continuous;

			_projectiles = new List<Entity>();
		}

		public void Fire()
		{
			Entity e = ProjectileBuffer.Instance.InactiveProjectiles.Pop();

			Vector2 shotVector = InputHandler.Instance.WorldMousePosition - _owner.Position;
			shotVector.Normalize();

			e.Position = _spotSpawn + _owner.Position + shotVector * 0.8f;

			Vector2 shotVelocity = shotVector * _shotSpeed;
			e.GetComponent<Projectile>().Revive(shotVelocity, 2.0, true, false, 10.0f, 20.0f, 2.0f, 1.0f);
			_projectiles.Add(e);

			_nextfire = _fireRate;
		}

		public void UpdateProjectilePos()
		{
			foreach (Entity e in _projectiles)
			{
				if (e.GetComponent<CircleCollider>() != null)
				{
					e.GetComponent<CircleCollider>().UpdatePosition();
				}
			}
		}

		public override void Load(ContentManager content)
		{

		}

		public override void Update(double deltaTime)
		{
			if (_nextfire <= 0)
			{
				if (_continuous)
				{
					if (InputHandler.Instance.IsLeftMouseHeld())
					{
						Fire();
					}
				}
				else
				{
					if (InputHandler.Instance.IsLeftMousePressed())
					{
						Fire();
					}
				}
			}
			else
			{
				_nextfire -= deltaTime;
			}

			for (int i = (_projectiles.Count - 1); i >= 0; i--)
			{
				Entity e = _projectiles[i];
				e.Update(deltaTime);
				if (e.GetComponent<Projectile>().Dead)
				{
					ProjectileBuffer.Instance.InactiveProjectiles.Push(e);
					_projectiles.Remove(e);
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 inv = new Vector2(1, -1);
			foreach (Entity e in _projectiles)
			{
				e.Draw(spriteBatch);
			}
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{

		}
	}
}

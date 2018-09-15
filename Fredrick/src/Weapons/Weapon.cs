using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Fredrick.src
{
	public class Weapon : Component
	{
		bool _continuous;

		protected double _fireRate;//How long between shots
		protected double _nextfire;
		protected float _damage;
		protected float _shotSpeed;

		protected Vector2 _spotSpawn;
		protected Vector2 _shotVector;

		public Drawable _d;

		List<Entity> _projectiles;

		public double FireRate
		{
			get { return _fireRate; }
			set { value = _fireRate; }
		}

		public float Damage
		{
			get { return _damage; }
			set { value = _damage; }
		}

		public Weapon(Entity owner) : base(owner)
		{
			_fireRate = 0.1;
			_spotSpawn = new Vector2(0, 0);
			_shotVector = new Vector2(1, 0);
			_shotSpeed = 4.0f;
			_projectiles = new List<Entity>();
			_scale = new Vector2(1);

			_continuous = true;
		}

		public void Fire()
		{
			Entity e = ProjectileBuffer.Instance.InactiveProjectiles.Pop();
			_shotVector = InputHandler.Instance.WorldMousePosition - _owner.GetPosition();// - _owner.GetPosition();
			_shotVector.Normalize();

			e.SetPosition(_spotSpawn + _owner.GetPosition() + _shotVector * 0.8f);

			Vector2 shotVelocity = _shotVector * _shotSpeed;
			//Debug.Write(_owner.GetPosition()+"\n");
			e.GetComponent<Projectile>().Revive(shotVelocity, 2.0, true, false, 10.0f, 2.0f, 1.0f);
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

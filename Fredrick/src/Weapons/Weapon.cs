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

		List<Projectile> _projectiles;

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
			_fireRate = 0.2;
			_spotSpawn = new Vector2(0, 0);
			_shotVector = new Vector2(1, 0);
			_shotSpeed = 50.0f;
			_projectiles = new List<Projectile>();
			_scale = new Vector2(1);

			_continuous = true;
		}

		public void Fire()
		{
			Projectile p = ProjectileBuffer.Instance.InactiveProjectiles.Pop();
			_shotVector = InputHandler.Instance.WorldMousePosition - _owner.GetPosition();// - _owner.GetPosition();
			_shotVector.Normalize();
			Vector2 shotVelocity = _shotVector * _shotSpeed;
			//Debug.Write(_owner.GetPosition()+"\n");
			p.Revive(_spotSpawn + _owner.GetPosition(), shotVelocity, 10.0);
			_projectiles.Add(p);

			_nextfire = _fireRate;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 inv = new Vector2(1, -1);
			foreach (Projectile p in _projectiles)
			{
				Color c = Color.LightGoldenrodYellow;
				//c *= p.Opacity;
				spriteBatch.Draw(_d._sprite, p.GetPosition() * inv * _d._spriteSize, _d._sourceRectangle, c, p.GetRotation(), _d._origin, _scale, _d._spriteEffects, _d._layer);
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
				Projectile p = _projectiles[i];
				p.Update(deltaTime, new Vector2(0, 0));
				if (p.LifeTime < 0)
				{
					ProjectileBuffer.Instance.InactiveProjectiles.Push(p);
					_projectiles.Remove(p);
				}
			}
		}
	}
}

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
		private bool _fireCommand;

		protected Vector2 _spotSpawn;
		protected Vector2 _weaponPosition;//The arm has uses the base transform
		protected Vector2 _transformedWeaponPosition;

		protected double _fireRate;//How long between shots
		protected double _nextfire;//Counter till next shot can be fired

		protected float _damage;
		protected float _aoeDamage;
		protected float _shotSpeed;

		protected bool _continuous;

		protected string projectile;

		protected bool _facingRight;

		public Drawable ArmDrawable { get; set; }
		public Drawable WeaponDrawable { get; set; }

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

		public Weapon(Entity owner, string id, Vector2 shotSpawn, Vector2 weaponPosition, double fireRate = 0.5, float damage = 10.0f, float aoeDamage = 20.0f, float shotSpeed = 5.0f, bool continuous = true) : base(owner, id)
		{
			_spotSpawn = shotSpawn;
			_weaponPosition = weaponPosition;
			_fireRate = fireRate;
			_damage = damage;
			_aoeDamage = aoeDamage;
			_shotSpeed = shotSpeed;

			_continuous = continuous;

			_projectiles = new List<Entity>();

			_scale = new Vector2(1);

			_facingRight = true;
		}

		public void Fire(Vector2 direction, float sin, float cos)
		{
			Entity e = ProjectileBuffer.Instance.InactiveProjectiles.Pop();

			float tssx = _spotSpawn.X;
			float tssy = _spotSpawn.Y;

			Vector2 transformedShotSpawn = new Vector2((cos * tssx) - (sin * tssy), (sin * tssx) + (cos * tssy));

			e.Position = _owner.Position + _position + transformedShotSpawn;

			Vector2 shotVelocity = direction * _shotSpeed;
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

		public void Flip(bool faceRight)
		{
			//needs to be changed to account for position change
			if (faceRight != _facingRight)
			{
				_facingRight = !_facingRight;
				if (_facingRight)
				{
					ArmDrawable._spriteEffects = SpriteEffects.None;
				}
				else
				{
					ArmDrawable._spriteEffects = SpriteEffects.FlipVertically;
				}
			}
		}

		public override void Load(ContentManager content)
		{
			WeaponDrawable.Load(content);
			ArmDrawable.Load(content);
		}

		public override void Unload()
		{

		}

		public override void Update(double deltaTime)
		{

			Vector2 direction;
			if (_owner.GetDerivedComponent<Controller>() != null)
			{
				direction = _owner.GetDerivedComponent<Controller>().Aim;
				if (_continuous)
					_fireCommand = _owner.GetDerivedComponent<Controller>().FireHeld;
				else
					_fireCommand = _owner.GetDerivedComponent<Controller>().FirePressed;
			}
			else
			{
				direction = new Vector2(1, 0);
				_fireCommand = false;
			}

			direction.Normalize();

			_rotation = (float)Math.Atan2(-direction.Y, direction.X);

			float sin = (float)Math.Sin(-_rotation);
			float cos = (float)Math.Cos(-_rotation);

			float twpx = _weaponPosition.X;
			float twpy = _weaponPosition.Y;

			_transformedWeaponPosition = new Vector2((cos * twpx) - (sin * twpy), (sin * twpx) + (cos * twpy));

			if (_nextfire <= 0)
			{
				if (_fireCommand)
				{
					Fire(direction, sin, cos);
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
			spriteBatch.Draw(ResourceManager.Instance.Textures[WeaponDrawable._spriteName], (_transformedWeaponPosition + _position + _owner.Position) * inv * WeaponDrawable._spriteSize, WeaponDrawable._sourceRectangle, WeaponDrawable._colour, _owner.Rotation + _rotation, WeaponDrawable._origin, _scale, WeaponDrawable._spriteEffects, WeaponDrawable._layer);
			spriteBatch.Draw(ResourceManager.Instance.Textures[ArmDrawable._spriteName], (_position + _owner.Position) * inv * ArmDrawable._spriteSize, ArmDrawable._sourceRectangle, ArmDrawable._colour, _owner.Rotation + _rotation, ArmDrawable._origin, _scale, ArmDrawable._spriteEffects, ArmDrawable._layer);

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

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

		public double LifeTime
		{
			get { return _lifeTime; }
			set { _lifeTime = value; }
		}


		public Projectile(Entity owner) : base(owner)
		{
			_position = new Vector2();
			_velocity = new Vector2();
		}

		public Projectile(Entity owner, Vector2 position, Vector2 velocity, double lifeTime) : base(owner)
		{
			_owner.SetPosition(position);
			_velocity = velocity;
			_lifeTime = lifeTime;
		}

		public void Revive(Vector2 position, Vector2 velocity, double lifeTime)
		{
			_owner.SetPosition(position);
			_velocity = velocity;
			_lifeTime = lifeTime;
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

		}

		public override void Draw(SpriteBatch spriteBatch)
		{

		}
	}
}

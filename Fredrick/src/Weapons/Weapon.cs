using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class Weapon : Component
	{
		protected float _fireRate;
		protected float _nextfire;
		protected float _damage;

		public float FireRate
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

		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			
		}

		public override void Update(double deltaTime)
		{
			
		}
	}
}

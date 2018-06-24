using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	abstract class Collider : Component
	{
		protected float _radius;
		protected int _spriteSize; //needs to be global define or something, simply defines scale of units(how many pixels is a meter or whatever);

		public Collider(Entity owner) : base(owner)
		{
			_spriteSize = 32;
		}

		public abstract void CheckCollision(Collider other);
	}
}

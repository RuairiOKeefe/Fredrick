using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Fredrick.src
{
	[Serializable]
	public class Controller
	{
		public float Movement { get; protected set; }
		public Vector2 Aim { get; protected set; }
		public bool Jump { get; protected set; }
		public bool FirePressed { get; protected set; }
		public bool FireHeld { get; protected set; }

		public Controller()
		{

		}

		protected virtual void SetMovement() { }
		protected virtual void SetAim() { }
		protected virtual void SetJump() { }
		protected virtual void SetFire() { }

		public virtual void Update()
		{
			SetMovement();
			SetAim();
			SetJump();
			SetFire();
		}//Update should be called to get any inputs that occur then
	}
}

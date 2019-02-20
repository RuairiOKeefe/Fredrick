using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	[Serializable]
	public class Controller : Component //may want to make abstract
	{
		public float Movement { get; set; }
		public bool Jump { get; set; }
		public bool FirePressed { get; set; }
		public bool FireHeld { get; set; }

		public Controller()
		{

		}

		public Controller(Entity owner, string id, bool active = true) : base(owner, id, active)
		{

		}

		public virtual Vector2 GetAim(Vector2 origin) { return new Vector2(0); }

		protected virtual void SetMovement() { }
		protected virtual void SetJump() { }
		protected virtual void SetFire() { }

		public override void Load(ContentManager content)
		{

		}

		public override void Unload()
		{

		}

		public override void Update(double deltaTime)
		{
			SetMovement();
			SetJump();
			SetFire();
		}

		public override void Draw(SpriteBatch spriteBatch)
		{

		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{

		}

		public override Component Copy(Entity owner)
		{
			return new Controller();
		}
	}
}

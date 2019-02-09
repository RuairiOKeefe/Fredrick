using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Fredrick.src
{
	[Serializable]
	public class PlayerController : Controller
	{
		public bool Keyboard;
		public int ControllerIndex;

		public PlayerController(Entity owner, string id, bool active = true) : base(owner, id, active)
		{

		}

		public override Vector2 GetAim(Vector2 origin)
		{
			return (InputHandler.Instance.WorldMousePosition - (Owner.Position + origin));
		}

		protected override void SetMovement()
		{
			Movement = 0;
			if (Keyboard)
			{

				if (InputHandler.Instance.IsKeyHeld(InputHandler.Action.Left) || InputHandler.Instance.IsKeyPressed(InputHandler.Action.Left))
				{
					Movement--;
				}
				if (InputHandler.Instance.IsKeyHeld(InputHandler.Action.Right) || InputHandler.Instance.IsKeyPressed(InputHandler.Action.Right))
				{
					Movement++;
				}
			}
		}

		protected override void SetJump()
		{
			Jump = false;
			if (Keyboard)
			{
				if (InputHandler.Instance.IsKeyPressed(InputHandler.Action.Jump))
					Jump = true;
			}
		}

		protected override void SetFire()
		{
			FirePressed = InputHandler.Instance.IsLeftMousePressed();
			FireHeld = InputHandler.Instance.IsLeftMouseHeld();
		}
	}
}

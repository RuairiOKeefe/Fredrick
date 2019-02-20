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
		public PlayerInput PlayerInput { get; set; }

		public PlayerController(Entity owner, string id, PlayerInput playerInput, bool active = true) : base(owner, id, active)
		{
			PlayerInput = playerInput;
		}

		public override Vector2 GetAim(Vector2 origin)
		{
			return (InputHandler.Instance.WorldMousePosition - (Owner.Position + origin));
		}

		protected override void SetMovement()
		{
			Movement = PlayerInput.GetMovement();
		}

		protected override void SetJump()
		{
			Jump = PlayerInput.GetJump();
		}

		protected override void SetFire()
		{
			FirePressed = PlayerInput.GetFirePressed();
			FireHeld = PlayerInput.GetFireHeld();
		}
	}
}

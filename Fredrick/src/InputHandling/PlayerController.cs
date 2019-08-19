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

		public PlayerController()
		{
		}

		public PlayerController(Entity owner, string id, PlayerIndex playerIndex = PlayerIndex.One, bool keyboard = true, bool controller = false, List<string> tags = null, bool active = true) : base(owner, id, tags, active)
		{
			PlayerInput = new PlayerInput(playerIndex, keyboard, controller);
		}

		public PlayerController(Entity owner, PlayerController original) : base(owner, original.Id)
		{
			PlayerInput = new PlayerInput(original.PlayerInput.PlayerIndex, original.PlayerInput.Keyboard, original.PlayerInput.Controller);
		}

		public override Vector2 GetAim(Vector2 origin)
		{
			
			if (PlayerInput.GetControllerAim() != new Vector2(0))
			{
				return PlayerInput.GetControllerAim();
			}
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


		public override Component Copy(Entity owner)
		{
			return new PlayerController(owner, this);
		}

		public override void Update(double deltaTime)
		{
			PlayerInput.Update();
			base.Update(deltaTime);
		}
	}
}

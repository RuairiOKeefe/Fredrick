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

		protected void SetMovement()
		{
			Movement = PlayerInput.GetMovement();
		}

		protected void SetJump()
		{
			Jump = PlayerInput.GetJump();
		}

		protected void SetFire()
		{
			FirePressed = PlayerInput.GetFirePressed();
			FireHeld = PlayerInput.GetFireHeld();
		}

		public override void Load(ContentManager content)
		{
		}

		public override void Unload()
		{
		}

		public override void Update(double deltaTime)
		{
			PlayerInput.Update();
			SetMovement();
			SetJump();
			SetFire();
		}

		public override void DrawBatch(SpriteBatch spriteBatch)
		{
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{
		}

		public override Component Copy(Entity owner)
		{
			return new PlayerController(owner, this);
		}
	}
}

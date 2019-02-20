using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	[Serializable]
	public class PlayerInput
	{
		public int PlayerIndex;
		public bool Keyboard;
		public int ControllerIndex;

		public PlayerInput()
		{
			
		}

		public float GetMovement()
		{
			float movement = 0;
			if (Keyboard)
			{

				if (InputHandler.Instance.IsKeyHeld(InputHandler.Action.Left) || InputHandler.Instance.IsKeyPressed(InputHandler.Action.Left))
				{
					movement--;
				}
				if (InputHandler.Instance.IsKeyHeld(InputHandler.Action.Right) || InputHandler.Instance.IsKeyPressed(InputHandler.Action.Right))
				{
					movement++;
				}
			}
			return movement;
		}

		public bool GetJump()
		{
			bool jump = false;
			if (Keyboard)
			{
				if (InputHandler.Instance.IsKeyPressed(InputHandler.Action.Jump))
					jump = true;
			}

			return jump;
		}

		public bool GetFirePressed()
		{
			bool firePressed = false;

			if (Keyboard)
			{
				firePressed = InputHandler.Instance.IsLeftMousePressed();
			}

			return firePressed;
		}

		public bool GetFireHeld()
		{
			bool fireHeld = false;

			if (Keyboard)
			{
				fireHeld = InputHandler.Instance.IsLeftMouseHeld();
			}

			return fireHeld;
		}

		public void Update()
		{

		}
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
		public int m_PlayerIndex;
		public bool Keyboard;

		[NonSerialized]
		private GamePadState m_controllerState;
		[NonSerialized]
		private GamePadState m_previousControllerState;
		private const float triggerDeadZone = 0.15f;
		private const float thumbstickDeadZone = 0.15f;

		public PlayerInput()
		{

		}

		private bool IsButtonPressed(Buttons button)
		{
			if (m_controllerState.IsButtonDown(button) && m_previousControllerState.IsButtonUp(button))
			{
				return true;
			}
			return false;
		}

		private bool IsButtonHeld(Buttons button)
		{
			if (m_controllerState.IsButtonDown(button) && m_previousControllerState.IsButtonDown(button))
			{
				return true;
			}
			return false;
		}

		private bool IsButtonReleased(Buttons button)
		{
			if (m_controllerState.IsButtonUp(button))
			{
				return true;
			}
			return false;
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
			if (m_controllerState.IsConnected)
			{
				movement += m_controllerState.ThumbSticks.Left.X;
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
			if (m_controllerState.IsConnected && !jump)
			{
				jump = IsButtonPressed(Buttons.A);
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
			if (m_controllerState.IsConnected && !firePressed)
			{
				if (m_controllerState.Triggers.Right >= triggerDeadZone && m_previousControllerState.Triggers.Right < triggerDeadZone)
					firePressed = true;
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
			if (m_controllerState.IsConnected)
			{
				fireHeld = m_controllerState.Triggers.Right >= triggerDeadZone ? true : false;
			}

			return fireHeld;
		}

		public Vector2 GetControllerAim()
		{
			if (m_controllerState.ThumbSticks.Right.Length() > thumbstickDeadZone)
				return m_controllerState.ThumbSticks.Right;
			return new Vector2(0);
		}

		public void Update()
		{
			m_previousControllerState = m_controllerState;
			m_controllerState = GamePad.GetState(PlayerIndex.One);
		}
	}
}

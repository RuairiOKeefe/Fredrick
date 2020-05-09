using Fredrick.src.Rigging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	[Serializable]
	public class MovementStateMachine
	{
		public enum Action
		{
			Idle,
			Standing,
			Walking,
			Sprinting,
			Dashing,
			Jumping,
			Falling
		}

		public Action CurrentAction;
		public Character Character;

		public MovementStateMachine()
		{
			CurrentAction = Action.Standing;
		}

		public MovementStateMachine(Character character, Action action = Action.Standing)
		{
			Character = character;
			CurrentAction = action;
		}

		public void TryStanding()
		{
			if (Math.Abs(Character.Velocity.X) < 0.2f && Character.Grounded)
			{
				CharacterRig rig = Character.Owner.GetComponent<CharacterRig>(null, "Legs");
				if (rig != null)
				{
					rig.SwitchToAnim("Standing", true);
				}
				CurrentAction = Action.Standing;
			}
		}

		public void TryWalking()
		{
			if (Math.Abs(Character.Velocity.X) > 0.2f && Character.Grounded)
			{
				CharacterRig rig = Character.Owner.GetComponent<CharacterRig>(null, "Legs");
				if (rig != null)
				{
					rig.SwitchToAnim("Running", true);
				}
				CurrentAction = Action.Walking;
			}
		}

		public void TryJumping()
		{
			if (Character.JumpCommand)
			{
				CharacterRig rig = Character.Owner.GetComponent<CharacterRig>(null, "Legs");
				if (rig != null)
				{
					rig.SwitchToAnim("Jumping", false);
				}
				CurrentAction = Action.Jumping;
			}
		}

		public void TryFalling()
		{
			if (Character.Velocity.Y < 0.0f && !Character.Grounded)
			{
				CharacterRig rig = Character.Owner.GetComponent<CharacterRig>(null, "Legs");
				if (rig != null)
				{
					rig.SwitchToAnim("Falling", true);
				}
				CurrentAction = Action.Falling;
			}
		}


		public void IdleTransistion()
		{

		}

		public void StandingTransistion()
		{
			TryWalking();
			TryJumping();
		}

		public void WalkingTransistion()
		{
			TryStanding();
			TryJumping();
			TryFalling();
		}

		public void SprintingTransistion()
		{

		}

		public void DashingTransistion()
		{

		}

		public void JumpingTransistion()
		{
			//TryStanding();
			//TryWalking();
			TryJumping();
			TryFalling();
		}

		public void FallingTransistion()
		{
			TryStanding();
			TryWalking();
			TryJumping();
		}

		public void Update()
		{
			switch (CurrentAction)
			{
				case (Action.Idle):
					IdleTransistion();
					break;
				case (Action.Standing):
					StandingTransistion();
					break;
				case (Action.Walking):
					WalkingTransistion();
					break;
				case (Action.Sprinting):
					StandingTransistion();
					break;
				case (Action.Dashing):
					DashingTransistion();
					break;
				case (Action.Jumping):
					JumpingTransistion();
					break;
				case (Action.Falling):
					FallingTransistion();
					break;
			}
		}
	}
}

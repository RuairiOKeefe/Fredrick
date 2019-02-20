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
		private Character Character;

		public MovementStateMachine()
		{
			CurrentAction = Action.Standing;
		}

		public MovementStateMachine(Character character, Action action = Action.Standing)
		{
			Character = character;
			CurrentAction = action;
		}

		public void IdleTransistion()
		{

		}

		public void StandingTransistion()
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

		public void WalkingTransistion()
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

		public void SprintingTransistion()
		{

		}

		public void DashingTransistion()
		{

		}

		public void JumpingTransistion()
		{

		}

		public void FallingTransistion()
		{

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

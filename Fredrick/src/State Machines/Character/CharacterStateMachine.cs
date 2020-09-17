using Fredrick.src.State_Machines.ActionSuperStates;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src.State_Machines.Character
{
	[Serializable]
	public abstract class CharacterStateMachine : StateMachine
	{
		public abstract void MoveTo(Vector2 target);
		public abstract void Target(Vector2 target);
		public abstract void Fire();
	}
}

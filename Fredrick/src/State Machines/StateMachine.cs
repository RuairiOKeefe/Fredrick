using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src.State_Machines
{
	[Serializable]
	public abstract class StateMachine
	{
		protected IState m_currentState;

		protected abstract void TransitionState();
		public abstract void ChangeState();
		public abstract void Start();
	}
}

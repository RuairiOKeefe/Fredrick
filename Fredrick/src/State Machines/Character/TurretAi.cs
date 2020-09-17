using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fredrick.src.Ai_Utilities;
using Fredrick.src.InputHandling;
using Fredrick.src.State_Machines.ActionSuperStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Fredrick.src.State_Machines.Character
{
	[Serializable]
	public class TurretAi : Ai
	{

		private static Idle m_idleState;
		private static Assault m_attackState;

		public TurretAi(Entity owner)
		{
			m_owner = owner;
			m_targetAcquisition = new TargetAcquisition(m_owner, 1, 3, 20, 5.0);
		}

		public TurretAi(Entity owner, TurretAi original)
		{
			m_owner = owner;
			m_targetAcquisition = original.m_targetAcquisition.Copy(owner);
		}

		protected override void TransitionState()
		{

		}

		public override void ChangeState()
		{
			IState newState;

			switch (m_currentState)
			{
				case Idle idle:
					if (true)
					{
						newState = m_attackState;
					}
					break;
				case Assault attack:
					if (true)
					{
						newState = m_idleState;
					}
					break;

			}
		}

		private void AcquireTargets(double deltaTime)
		{
			m_targetAcquisition.Update(deltaTime);
			m_spottedHostiles = new List<Entity>(m_targetAcquisition.SpottedEntities); //probably find a way to ignore if no changes
			if (m_spottedHostiles.Count > 0)
			{
				m_target = m_spottedHostiles[0].Position;//basic bitch solution for testing
				m_isTargetting = true;
			}
			else
			{
				m_isTargetting = false;
			}
		}

		public override void Start()
		{

		}

		public override void Load(ContentManager content)
		{
			m_targetAcquisition.Load(content, m_owner);
		}

		public override AiInput Update(double deltaTime)
		{
			AcquireTargets(deltaTime);
			if (m_isTargetting)
			{
				Target(m_target);
			}

			return m_aiInput;
		}

		public override Ai Copy(Entity owner)
		{
			return new TurretAi(owner, this);
		}

		public override void MoveTo(Vector2 target)
		{
			throw new NotImplementedException();
		}

		public override void Target(Vector2 target)
		{
			//This is the aiming "goal", weapon turning speed will be controlled elsewhere, and so the actual weapon direction may differ
			m_aiInput.Target = target;
			if (/*Aiming at target*/true)
			{
				Fire();
			}
		}

		public override void Fire()
		{
			m_aiInput.Fire = true;
		}
	}
}

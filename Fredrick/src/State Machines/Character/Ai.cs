using Fredrick.src.Ai_Utilities;
using Fredrick.src.InputHandling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src.State_Machines.Character
{
	[Serializable]
	public abstract class Ai : CharacterStateMachine
	{
		protected AiInput m_aiInput = new AiInput();
		protected TargetAcquisition m_targetAcquisition;
		protected List<Entity> m_spottedHostiles;
		protected bool m_isTargetting;
		protected Vector2 m_target;
		protected Vector2 m_waypoint;
		protected Entity m_owner; //The owner is used to get info about stuff, but the controller that owns this will have callbacks to actually do things. Maybe make this get only?

		public abstract void Load(ContentManager content);
		public abstract AiInput Update(double deltaTime);
		public abstract Ai Copy(Entity owner);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src.Colliders
{
	[Serializable]
	public abstract class Trigger : Collider
	{
		public Trigger(Entity owner, string id) : base(owner, id)
		{

		}

		public Trigger(Entity owner, Component original) : base(owner, original)
		{

		}

		public void AddCollisionHandler(OnCollisionEventHandler eventHandler)
		{
			m_body.OnCollision += eventHandler;
		}

		public void AddSeparationHandler(OnSeparationEventHandler eventHandler)
		{
			m_body.OnSeparation += eventHandler;
		}
	}
}

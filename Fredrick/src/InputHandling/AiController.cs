using Fredrick.src.State_Machines.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src.InputHandling
{
	[Serializable]
	public class AiController : Controller
	{
		private Ai m_ai; // The ai type used by this character
		protected Vector2 m_target;

		public AiController(Ai ai)
		{
			m_ai = ai ?? throw new ArgumentNullException(nameof(m_ai));
		}

		public AiController(Entity owner, AiController original) : base(owner, original.Id)
		{
			m_ai = original.m_ai.Copy(owner);
		}

		public override Vector2 GetAim(Vector2 origin)
		{
			return (m_target - (Owner.Position + origin));
		}

		public override void Load(ContentManager content)
		{
			m_ai.Load(content);
		}

		public override void Unload()
		{
		}

		public override void Update(double deltaTime)
		{
			AiInput input = m_ai.Update(deltaTime);
			Movement = input.Move;
			m_target = input.Target;
			Jump = input.Jump;
			FirePressed = input.Fire;//Ai will only use semi auto weapons for now, need to split later?
			FireHeld = input.Fire;

		}

		public override void DrawBatch(SpriteBatch spriteBatch)
		{
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{
		}

		public override Component Copy(Entity owner)
		{
			return new AiController(owner, this);
		}
	}
}

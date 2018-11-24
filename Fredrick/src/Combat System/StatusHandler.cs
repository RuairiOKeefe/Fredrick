using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class StatusHandler : Component
	{
		public List<StatusEffect> Statuses { get; set; }

		public StatusHandler(Entity owner, string id) : base(owner, id)
		{

		}

		public override void Load(ContentManager content)
		{

		}

		public override void Unload()
		{

		}

		public override void Update(double deltaTime)
		{

		}

		public override void Draw(SpriteBatch spriteBatch)
		{

		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{

		}
	}
}

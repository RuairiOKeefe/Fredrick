using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	[Serializable]
	public abstract class Controller : Component //may want to make abstract
	{
		public float Movement { get; set; }
		public bool Jump { get; set; }
		public bool FirePressed { get; set; }
		public bool FireHeld { get; set; }

		public Controller()
		{

		}

		public Controller(Entity owner, string id, List<string> tags = null, bool active = true) : base(owner, id, tags, active)
		{

		}

		public abstract Vector2 GetAim(Vector2 origin);
	}
}

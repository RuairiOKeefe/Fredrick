using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src.InputHandling
{
	/// <summary>
	/// AiInput contains the inputs given by an Ai in a frame.
	/// This collection of inputs is used by the AiController to control Ai actors.
	/// </summary>]
	[Serializable]
	public class AiInput
	{
		public float Move { get; set; }
		public Vector2 Target { get; set; }
		public bool Jump { get; set; }
		public bool Fire { get; set; }

		public AiInput()
		{
			Move = 0;
			Target = new Vector2(0);
			Jump = false;
			Fire = false;
		}

		/// <summary>
		/// Clears all states, should be called at the start of each Ai update
		/// </summary>
		public void Clear()
		{
			Move = 0;
			Target = new Vector2(0);
			Jump = false;
			Fire = false;
		}
	}
}

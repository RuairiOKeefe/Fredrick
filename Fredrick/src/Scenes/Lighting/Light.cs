using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public class Light
	{
		public Vector4 Colour { get; private set; }
		public Vector3 Position { get; private set; }
		public float ConstantK { get; private set; }
		public float LinearK { get; private set; }
		public float QuadraticK { get; private set; }
	}
}

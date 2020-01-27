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

		public Light()
		{
			Colour = new Vector4(0);
			Position = new Vector3(0);
			ConstantK = 0.0f;
			LinearK = 0.0f;
			QuadraticK = 0.0f;
		}

		public Light(Vector4 colour, Vector3 position, float constantK, float linearK, float quadraticK)
		{
			Colour = colour;
			Position = position;
			ConstantK = constantK;
			LinearK = linearK;
			QuadraticK = quadraticK;
		}

		public Light(Light original)
		{
			Colour = original.Colour;
			Position = original.Position;
			ConstantK = original.ConstantK;
			LinearK = original.LinearK;
			QuadraticK = original.QuadraticK;
		}

		public Light Copy()
		{
			return new Light(this);
		}
	}
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public static class LightingUtils
	{
		public static Vector4[] GetColours(Light[] lights)
		{
			Vector4[] colours = new Vector4[lights.Length];
			for (int i = 0; i< lights.Length; i++)
			{
				colours[i] = lights[i].Colour;
			}
			return colours;
		}

		public static Vector3[] GetPositions(Light[] lights)
		{
			Vector3[] positions = new Vector3[lights.Length];
			for (int i = 0; i < lights.Length; i++)
			{
				positions[i] = lights[i].Position;
			}
			return positions;
		}

		public static float[] GetConstants(Light[] lights)
		{
			float[] constants = new float[lights.Length];
			for (int i = 0; i < lights.Length; i++)
			{
				constants[i] = lights[i].ConstantK;
			}
			return constants;
		}

		public static float[] GetLinears(Light[] lights)
		{
			float[] linears = new float[lights.Length];
			for (int i = 0; i < lights.Length; i++)
			{
				linears[i] = lights[i].LinearK;
			}
			return linears;
		}

		public static float[] GetQuadratics(Light[] lights)
		{
			float[] quadtratics = new float[lights.Length];
			for (int i = 0; i < lights.Length; i++)
			{
				quadtratics[i] = lights[i].QuadraticK;
			}
			return quadtratics;
		}
	}
}

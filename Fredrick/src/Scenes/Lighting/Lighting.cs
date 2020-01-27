using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public class Lighting
	{
		public List<Light> FixedLights { get; private set; }

		public Lighting()
		{
			FixedLights = new List<Light>();
		}

		public void GetFixedLights(out Light[] lights)
		{
			lights = new Light[16];

			//Get first 16 for now
			for (int i = 0; i < 16; i++)
			{
				if (i < FixedLights.Count)
				{
					lights[i] = FixedLights[i];
				}
				else
				{
					lights[i] = new Light();
				}
			}
		}
	}
}

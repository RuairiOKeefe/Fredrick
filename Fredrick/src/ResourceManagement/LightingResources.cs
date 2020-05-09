using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public sealed class LightingResources
	{
		private static LightingResources instance = null;
		private static readonly object padlock = new object();

		public static LightingResources Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new LightingResources();
					}
					return instance;
				}
			}
		}

		public Dictionary<string, Light> PointLights { get; private set; } = new Dictionary<string, Light>();

		LightingResources()
		{
			InitPointLights();
		}

		public void Load()
		{

		}

		void InitPointLights()
		{
			{
				Vector4 colour = new Vector4(255, 255, 255, 255);
				Vector3 position = new Vector3(10,10,-0.6f);
				float constK = 1.0f;
				float linearK = 1.0f;
				float quadraticK = 1.0f;
				Light light = new Light(colour, position, constK, linearK, quadraticK);

				PointLights.Add("BasicLight", light);
			}
		}

	}
}

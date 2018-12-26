using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public sealed class Resources
	{
		private static Resources instance = null;
		private static readonly object padlock = new object();

		public static Resources Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new Resources();
					}
					return instance;
				}
			}
		}

		public Dictionary<string, Emitter> Emitters { get; private set; }


		Resources()
		{
			InitEmitters();
		}

		private void InitEmitters()
		{
			Emitters = new Dictionary<string, Emitter>();

			List<Tuple<Color, double>> explosionLerp = new List<Tuple<Color, double>>();
			explosionLerp.Add(new Tuple<Color, double>(new Color(255, 192, 45, 255) * 0.8f, 0.0));
			explosionLerp.Add(new Tuple<Color, double>(new Color(255, 86, 25, 255) * 0.8f, 0.15));
			explosionLerp.Add(new Tuple<Color, double>(new Color(112, 103, 97, 255) * 0.4f, 0.2));
			explosionLerp.Add(new Tuple<Color, double>(new Color(61, 59, 58, 255) * 0.1f, 0.5));
			explosionLerp.Add(new Tuple<Color, double>(new Color(61, 59, 58, 255) * 0.0f, 1.0));
			Emitter Explosion = new Emitter(null, "explosion", false, 1000, 600, new Vector2(0, 0), 0, 0, 17.0f, 0.6);
			Explosion.SetLifeTime(0.6, 0, 0);
			Explosion.SetVelocity(0.0f, 1.0f, 17.0f);
			Explosion.SetCollision(true, false);
			Explosion.SetScaling(false, 0.0f);
			Explosion.LerpColours = explosionLerp;

			Emitters.Add("Explosion", Explosion);

			List<Tuple<Color, double>> emberLerp = new List<Tuple<Color, double>>();
			emberLerp.Add(new Tuple<Color, double>(new Color(198, 48, 2, 255) * 0.5f, 0.0));
			emberLerp.Add(new Tuple<Color, double>(new Color(242, 192, 0, 255) * 0.8f, 0.3));
			emberLerp.Add(new Tuple<Color, double>(new Color(250, 243, 67, 255) * 0.8f, 0.4));
			emberLerp.Add(new Tuple<Color, double>(new Color(255, 255, 211, 255) * 0.5f, 0.5));
			emberLerp.Add(new Tuple<Color, double>(new Color(255, 255, 255, 255) * 0.0f, 1.0));
			Emitter Embers = new Emitter(null, "tempSpark", false, 1000, 50, new Vector2(0, -26.0f), 0, 0, 10.0f, 0.5);//need trails
			Embers.ParticleDrawable = new Drawable("tempSpark", new Vector2(8), 16, 16, 0.1f);
			Embers.Scale = new Vector2(0.5f);
			Embers.SetLifeTime(0.0, 0.5, 1.2);
			Embers.SetVelocity(0.0f, 10.5f, 12.0f, true);
			Embers.SetCollision(true, true);
			Embers.SetScaling(true, 0.5f);
			Embers.LerpColours = emberLerp;

			Emitters.Add("Embers", Embers);

		}
	}
}

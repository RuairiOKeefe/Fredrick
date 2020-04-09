using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fredrick.src.ResourceManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;



namespace Fredrick.src
{
	public sealed class ParticleBuffer
	{
		private static ParticleBuffer _instance = null;
		private static readonly object _padlock = new object();

		public static ParticleBuffer Instance
		{
			get
			{
				lock (_padlock)
				{
					if (_instance == null)
						_instance = new ParticleBuffer();
					return _instance;
				}
			}
		}

		public const int NUM_PARTICLES = 2000000;

		public Stack<Particle> InactiveParticles;
		public Dictionary<string, Dictionary<string, List<Particle>>> ActiveParticles = new Dictionary<string, Dictionary<string, List<Particle>>>();

		public ParticleBuffer()
		{
			InactiveParticles = new Stack<Particle>(NUM_PARTICLES);
			foreach (Emitter e in Resources.Instance.Emitters.Values)
			{
				string shader = "";
				if (e.ParticleDrawable.ShaderInfo != null)
				{
					shader = e.ParticleDrawable.ShaderInfo.ShaderId;
				}
				string sprite = e.ParticleDrawable._spriteName;
				if (!ActiveParticles.ContainsKey(shader))
				{
					ActiveParticles.Add(shader, new Dictionary<string, List<Particle>>());
				}
				if (!ActiveParticles[shader].ContainsKey(sprite))
				{
					ActiveParticles[shader].Add(e.ParticleDrawable._spriteName, new List<Particle>(NUM_PARTICLES));
				}
			}
			for (int i = 0; i < NUM_PARTICLES; i++)
			{
				InactiveParticles.Push(new Particle());
			}
		}

		public void Add(Particle particle)
		{
			string shader = "";
			if (particle.Drawable.ShaderInfo != null)
			{
				shader = particle.Drawable.ShaderInfo.ShaderId;
			}
			string sprite = particle.Drawable._spriteName;

			if (!ActiveParticles.ContainsKey(shader))
			{
				ActiveParticles.Add(shader, new Dictionary<string, List<Particle>>());
			}
			if (!ActiveParticles[shader].ContainsKey(sprite))
			{
				ActiveParticles[shader].Add(particle.Drawable._spriteName, new List<Particle>(NUM_PARTICLES));
			}

			ActiveParticles[shader][sprite].Add(particle);
		}

		public void Load(ContentManager content)
		{
			for (int i = 0; i < NUM_PARTICLES; i++)
			{
				InactiveParticles.Push(new Particle());
			}
		}

		public void Update(double deltaTime)
		{
			foreach (string shader in ActiveParticles.Keys)
			{
				foreach (string sprite in ActiveParticles[shader].Keys)
				{
					for (int i = (ActiveParticles[shader][sprite].Count - 1); i >= 0; i--)
					{
						Particle p = ActiveParticles[shader][sprite][i];
						p.Update(deltaTime);
						if (p.Lifetime < 0)
						{
							InactiveParticles.Push(p);
							ActiveParticles[shader][sprite].RemoveAt(i);
						}
					}
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch, Effect shader)
		{
			Vector2 inv = new Vector2(1, -1);
			string shaderName = "";
			if (shader != null)
			{
				shaderName = shader.Name;
			}
			if (ActiveParticles.ContainsKey(shaderName))
			{
				foreach (string sprite in ActiveParticles[shaderName].Keys)
				{
					if (ActiveParticles[shaderName][sprite].Count > 0)
					{
						if (ActiveParticles[shaderName][sprite][0].Drawable.ShaderInfo != null)
						{
							ActiveParticles[shaderName][sprite][0].Drawable.ShaderInfo.SetUniforms(shader);
						}
					}

					foreach (Particle p in ActiveParticles[shaderName][sprite])
					{
						Color c = Color.White;
						if (p.LerpColours.Count == 1)
							c = p.LerpColours[0].Item1;
						if (p.LerpColours.Count > 1)
						{
							double lifeRatio = 1 - (p.Lifetime / p.InitLifetime);
							Color a = new Color();
							Color b = new Color();
							double lerpAmount = 0;

							foreach (Tuple<Color, double> t in p.LerpColours)
							{
								if (lifeRatio <= t.Item2)
								{
									b = t.Item1;
									int i = p.LerpColours.IndexOf(t) - 1;
									if (i < 0)
									{
										a = new Color(0, 0, 0, 0);
										lerpAmount = lifeRatio / t.Item2;
									}
									else
									{
										a = p.LerpColours[i].Item1;
										lerpAmount = (lifeRatio - p.LerpColours[i].Item2) / (t.Item2 - p.LerpColours[i].Item2);
									}
									break;
								}
							}
							c = Color.Lerp(a, b, (float)lerpAmount);
						}

						float layer = p.Drawable._layer;
						if (p.Scale.X > 1.0f)
							layer += 0.01f;
						if (p.Scale.X < 1.0f)
							layer -= 0.01f;

						spriteBatch.Draw(ResourceManager.Instance.Textures[p.Drawable._spriteName], p.Position * inv * p.Drawable._spriteSize, p.Drawable._sourceRectangle, c, p.Rotation, p.Drawable._origin, p.Scale, p.Drawable._spriteEffects, layer);
					}
				}
			}
		}
	}
}
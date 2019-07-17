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
	public class Emitter : Component
	{
		public Drawable ParticleDrawable { get; set; }
		public List<Particle> Particles { get; set; }

		public float SpawnWidth { get; set; }
		public float SpawnHeight { get; set; }
		public int MaxParticles { get; set; }
		public bool Continuous { get; set; }
		public float EmissionTime { get; set; }
		public Vector2 Acceleration { get; set; }

		public float SpawnVelocity { get; set; }
		public float MinVelVar { get; set; }
		public float MaxVelVar { get; set; }
		public bool SqrVelVar { get; set; }

		public bool Collide { get; set; }
		public bool ReduceLifeOnCollision { get; set; }
		public float Restitution { get; set; }

		public double Lifetime { get; set; }
		public double MinLTVar { get; set; }
		public double MaxLTVar { get; set; }

		public bool FakeDepth { get; set; }
		public float ScaleFactor { get; set; }

		/// <summary>
		/// How many particles are emitted per emission if not continuous
		/// </summary>
		public int EmissionCount { get; set; }

		public List<Tuple<Color, double>> LerpColours
		{
			get { return m_lerpColours; }
			set
			{
				m_lerpColours = value;
				m_lerpColours.Sort((x, y) => x.Item2.CompareTo(y.Item2));
			}
		}

		protected List<Tuple<Color, double>> m_lerpColours;//for transparency colours MUST be multiplied by the desired opacity first

		protected Random m_rnd;

		public Emitter(Entity owner, String spriteName, bool continuous, int maxParticles, int emissionCount, Vector2 acceleration, float spawnWidth = 0, float spawnHeight = 0, float spawnVelocity = 3.0f, double lifeTime = 3.0, List<Tuple<Color, double>> lerpColours = null) : base(owner)
		{
			Position = new Vector2(0, 0);
			Rotation = 0;
			Scale = new Vector2(1.0f);

			ParticleDrawable = new Drawable(spriteName, new Vector2(16, 16), 32, 32, 0.1f);


			Particles = new List<Particle>();

			Acceleration = acceleration;

			this.SpawnWidth = spawnWidth;
			SpawnHeight = spawnHeight;

			MaxParticles = maxParticles;
			Continuous = continuous;
			EmissionCount = emissionCount;

			SpawnVelocity = spawnVelocity;
			Lifetime = lifeTime;

			m_lerpColours = new List<Tuple<Color, double>>();
			if (lerpColours != null)
				m_lerpColours = lerpColours;

			m_rnd = new Random();
		}

		public Emitter(Entity owner, Emitter original) : base(owner, original.Id, original.Active)
		{
			Position = original.Position;
			Rotation = original.Rotation;
			Scale = original.Scale;
			ParticleDrawable = original.ParticleDrawable;
			Particles = new List<Particle>();
			SpawnWidth = original.SpawnWidth;
			SpawnHeight = original.SpawnHeight;
			MaxParticles = original.MaxParticles;
			Continuous = original.Continuous;
			EmissionTime = original.EmissionTime;
			Acceleration = original.Acceleration;
			SpawnVelocity = original.SpawnVelocity;
			MinVelVar = original.MinVelVar;
			MaxVelVar = original.MaxVelVar;
			SqrVelVar = original.SqrVelVar;
			Collide = original.Collide;
			ReduceLifeOnCollision = original.ReduceLifeOnCollision;
			Lifetime = original.Lifetime;
			MinLTVar = original.MinLTVar;
			MaxLTVar = original.MaxLTVar;
			FakeDepth = original.FakeDepth;
			ScaleFactor = original.ScaleFactor;
			EmissionCount = original.EmissionCount;
			m_lerpColours = original.LerpColours;
			m_rnd = new Random();
		}

		public void SetVelocity(float spawnVelocity, float minVariance, float maxVariance, bool sqrVelVar = false)
		{
			SpawnVelocity = spawnVelocity;
			MinVelVar = minVariance;
			MaxVelVar = maxVariance;
			SqrVelVar = sqrVelVar;
		}

		public void SetCollision(bool collide = false, bool reduceLifeOnCollision = false)
		{
			Collide = collide;
			ReduceLifeOnCollision = reduceLifeOnCollision;
		}

		public void SetLifeTime(double lifeTime, double minVariance, double maxVariance)
		{
			Lifetime = lifeTime;
			MinLTVar = minVariance;
			MaxLTVar = maxVariance;
		}

		public void SetScaling(bool fakeDepth, float scaleFactor)
		{
			FakeDepth = fakeDepth;
			ScaleFactor = scaleFactor;
		}

		public void Emit()
		{
			for (int i = 0; i < EmissionCount; i++)
			{
				Vector2 spawnPos = new Vector2((float)m_rnd.NextDouble() * SpawnWidth - (SpawnWidth / 2), (float)m_rnd.NextDouble() * SpawnHeight - (SpawnHeight / 2));
				Vector2 spawnVel = new Vector2((float)m_rnd.NextDouble() * 2 - 1, (float)m_rnd.NextDouble() * 2 - 1);
				spawnVel.Normalize();

				float velocityRND = (float)m_rnd.NextDouble();
				if (SqrVelVar)
					velocityRND *= velocityRND;//modifies the distribution so more particles move slowly
				float velocityVar = (velocityRND * (MaxVelVar - MinVelVar)) + MinVelVar;
				spawnVel *= (SpawnVelocity + velocityVar);

				double lifetimeRND = m_rnd.NextDouble();
				double lifetime = Lifetime + ((lifetimeRND * (MaxLTVar - MinLTVar)) + MinLTVar);

				bool forwardMotion = m_rnd.NextDouble() >= 0.5;
				float scaleFactor = (forwardMotion ? (1.0f - velocityRND) : -(1.0f - velocityRND)) * ScaleFactor;

				Particle p = ParticleBuffer.Instance.InactiveParticles.Pop();
				p.Revive(spawnPos + _owner.Position, spawnVel, lifetime, Collide, ReduceLifeOnCollision, Restitution, FakeDepth, scaleFactor);
				Particles.Add(p);
			}
		}

		public override void Load(ContentManager content)
		{
			ParticleDrawable.Load(content);
		}

		public override void Unload()
		{

		}

		public override void Update(double deltaTime)
		{
			if (Continuous)
			{
				if (Particles.Count < MaxParticles)
				{
					Emit();
				}
			}

			//remove dead particles
			for (int i = (Particles.Count - 1); i >= 0; i--)
			{
				Particle p = Particles[i];
				p.Update(deltaTime, Acceleration);
				if (p.Lifetime < 0)
				{
					ParticleBuffer.Instance.InactiveParticles.Push(p);
					Particles.Remove(p);
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 inv = new Vector2(1, -1);

			foreach (Particle p in Particles)
			{
				Color c = Color.White;
				if (m_lerpColours.Count == 1)
					c = m_lerpColours[0].Item1;
				if (m_lerpColours.Count > 1)
				{
					double lifeRatio = 1 - (p.Lifetime / p.InitLifetime);
					Color a = new Color();
					Color b = new Color();
					double lerpAmount = 0;

					foreach (Tuple<Color, double> t in m_lerpColours)
					{
						if (lifeRatio <= t.Item2)
						{
							b = t.Item1;
							int i = m_lerpColours.IndexOf(t) - 1;
							if (i < 0)
							{
								a = new Color(0, 0, 0, 0);
								lerpAmount = lifeRatio / t.Item2;
							}
							else
							{
								a = m_lerpColours[i].Item1;
								lerpAmount = (lifeRatio - m_lerpColours[i].Item2) / (t.Item2 - m_lerpColours[i].Item2);
							}
							break;
						}
					}
					c = Color.Lerp(a, b, (float)lerpAmount);
				}

				float layer = ParticleDrawable._layer;
				if (p.Scale.X > 1.0f)
					layer += 0.01f;
				if (p.Scale.X < 1.0f)
					layer -= 0.01f;

				spriteBatch.Draw(ResourceManager.Instance.Textures[ParticleDrawable._spriteName], p.Position * inv * ParticleDrawable._spriteSize, ParticleDrawable._sourceRectangle, c, p.Rotation, ParticleDrawable._origin, Scale * p.Scale, ParticleDrawable._spriteEffects, layer);
			}
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{

		}

		public override Component Copy(Entity owner)
		{
			return new Emitter(owner, this);
		}
	}
}

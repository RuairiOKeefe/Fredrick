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

		public float SpawnWidth { get; set; }
		public float SpawnHeight { get; set; }
		public int MaxParticles { get; set; }
		public bool IsContinuous { get; set; }
		public bool IsEmitting { get; set; }
		public float EmissionTime { get; set; }
		public Vector2 Acceleration { get; set; }

		public float SpawnVelocity { get; set; }
		public float MinVelVar { get; set; }
		public float MaxVelVar { get; set; }
		public float WidthRatio { get; set; } = 1.0f;
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

		protected List<Tuple<Color, double>> m_lerpColours;

		protected Random m_rnd;

		public Emitter(Entity owner, String spriteName, bool continuous, bool emitting, int maxParticles, int emissionCount, Vector2 acceleration, float spawnWidth = 0, float spawnHeight = 0, float spawnVelocity = 3.0f, double lifeTime = 3.0, List<Tuple<Color, double>> lerpColours = null) : base(owner)
		{
			Position = new Vector2(0, 0);
			Rotation = 0;
			Scale = new Vector2(1.0f);

			ParticleDrawable = new Drawable(spriteName, new Vector2(16, 16), 32, 32, 0, 0, 0.1f);

			Acceleration = acceleration;

			this.SpawnWidth = spawnWidth;
			SpawnHeight = spawnHeight;

			MaxParticles = maxParticles;
			IsContinuous = continuous;
			IsEmitting = emitting;
			EmissionCount = emissionCount;

			SpawnVelocity = spawnVelocity;
			Lifetime = lifeTime;

			m_lerpColours = new List<Tuple<Color, double>>();
			if (lerpColours != null)
				m_lerpColours = lerpColours;

			m_rnd = new Random();
		}

		public Emitter(Entity owner, Emitter original) : base(owner, original, original.Active)
		{
			Position = original.Position;
			Rotation = original.Rotation;
			Scale = original.Scale;
			ParticleDrawable = original.ParticleDrawable;
			SpawnWidth = original.SpawnWidth;
			SpawnHeight = original.SpawnHeight;
			MaxParticles = original.MaxParticles;
			IsContinuous = original.IsContinuous;
			IsEmitting = original.IsEmitting;
			EmissionTime = original.EmissionTime;
			Acceleration = original.Acceleration;
			SpawnVelocity = original.SpawnVelocity;
			MinVelVar = original.MinVelVar;
			MaxVelVar = original.MaxVelVar;
			WidthRatio = original.WidthRatio;
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

		public void SetVelocity(float spawnVelocity, float minVariance, float maxVariance, float widthRatio = 1.0f, bool sqrVelVar = false)
		{
			SpawnVelocity = spawnVelocity;
			MinVelVar = minVariance;
			MaxVelVar = maxVariance;
			WidthRatio = widthRatio;
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

		public void SetScaling(float scale, bool fakeDepth, float scaleFactor)
		{
			Scale = new Vector2(scale);
			FakeDepth = fakeDepth;
			ScaleFactor = scaleFactor;
		}

		public void Emit()
		{
			for (int i = 0; i < EmissionCount; i++)
			{
				if (ParticleBuffer.Instance.InactiveParticles.Count > 0)
				{
					Vector2 spawnPos = new Vector2((float)m_rnd.NextDouble() * SpawnWidth - (SpawnWidth / 2), (float)m_rnd.NextDouble() * SpawnHeight - (SpawnHeight / 2));
					Vector2 spawnVel = new Vector2((float)m_rnd.NextDouble() * 2 - 1, (float)m_rnd.NextDouble() * 2 - 1);
					spawnVel.X *= WidthRatio;
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
					p.Revive(ParticleDrawable, spawnPos + Position + _owner.Position, Scale + Owner.Scale, spawnVel, Acceleration, lifetime, Collide, ReduceLifeOnCollision, Restitution, FakeDepth, scaleFactor, m_lerpColours);
					ParticleBuffer.Instance.Add(p);
				}
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
			if (IsContinuous && IsEmitting)
			{
				Emit();
			}
		}

		public override void DrawBatch(SpriteBatch spriteBatch)
		{

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

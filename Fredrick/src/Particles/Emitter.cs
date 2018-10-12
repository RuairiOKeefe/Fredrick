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
		protected List<Particle> _particles;
		protected Drawable _pD;//Particle Drawable


		protected float _spawnWidth;
		protected float _spawnHeight;
		protected float _maxParticles;
		protected bool _continuous;
		protected float _emissionTime;
		protected Vector2 _acceleration;

		protected float _spawnVelocity;
		protected float _minVelVar;//m
		protected float _maxVelVar;

		protected double _lifetime;
		protected double _minLTVar;
		protected double _maxLTVar;

		bool _sqrVelVar;

		bool _fakeDepth;
		float _scaleFactor;

		private List<Tuple<Color, double>> _lerpColours;//for transparency colours MUST be multiplied by the desired opacity first

		public List<Tuple<Color, double>> LerpColours
		{
			get { return _lerpColours; }
			set
			{
				_lerpColours = value;
				_lerpColours.Sort((x, y) => x.Item2.CompareTo(y.Item2));
			}
		}

		protected Random rnd;
		/// <summary>
		/// How many particles are emitted per emission if not continuous
		/// </summary>
		int _emissionCount;

		public List<Particle> Particles
		{
			get { return _particles; }
			set { _particles = value; }
		}

		public Drawable ParticleDrawable
		{
			get { return _pD; }
			set { _pD = value; }
		}

		public Emitter(Entity owner, String spriteName, bool continuous, int maxParticles, int emissionCount, Vector2 acceleration, float spawnWidth = 0, float spawnHeight = 0, float spawnVelocity = 3.0f, double lifeTime = 3.0) : base(owner)
		{
			_pD = new Drawable(spriteName, new Vector2(16, 16), 32, 32, 0.1f);
			_position = new Vector2(0, 0);
			_scale = new Vector2(1.0f);

			rnd = new Random();

			_particles = new List<Particle>();

			_acceleration = acceleration;

			_spawnWidth = spawnWidth;
			_spawnHeight = spawnHeight;

			_maxParticles = maxParticles;
			_continuous = continuous;
			_emissionCount = emissionCount;

			_spawnVelocity = spawnVelocity;
			_lifetime = lifeTime;

			_lerpColours = new List<Tuple<Color, double>>();
		}

		public void SetVelocity(float spawnVelocity, float minVariance, float maxVariance, bool sqrVelVar = false)
		{
			_spawnVelocity = spawnVelocity;
			_minVelVar = minVariance;
			_maxVelVar = maxVariance;
			_sqrVelVar = sqrVelVar;
		}

		public void SetLifeTime(double lifeTime, double minVariance, double maxVariance)
		{
			_lifetime = lifeTime;
			_minLTVar = minVariance;
			_maxLTVar = maxVariance;
		}

		public void SetScaling(bool fakeDepth, float scaleFactor)
		{
			_fakeDepth = fakeDepth;
			_scaleFactor = scaleFactor;
		}

		public void Emit()
		{
			for (int i = 0; i < _emissionCount; i++)
			{
				Vector2 spawnPos = new Vector2((float)rnd.NextDouble() * _spawnWidth - (_spawnWidth / 2), (float)rnd.NextDouble() * _spawnHeight - (_spawnHeight / 2));
				Vector2 spawnVel = new Vector2((float)rnd.NextDouble() * 2 - 1, (float)rnd.NextDouble() * 2 - 1);
				spawnVel.Normalize();

				float velocityRND = (float)rnd.NextDouble();
				if (_sqrVelVar)
					velocityRND *= velocityRND;//modifies the distribution so more particles move slowly
				float velocityVar = (velocityRND * (_maxVelVar - _minVelVar)) + _minVelVar;
				spawnVel *= (_spawnVelocity + velocityVar);

				double lifetimeRND = rnd.NextDouble();
				double lifetime = _lifetime + ((lifetimeRND * (_maxLTVar - _minLTVar)) + _minLTVar);

				bool forwardMotion = rnd.NextDouble() >= 0.5;
				float scaleFactor = (forwardMotion ? (1.0f - velocityRND) : -(1.0f - velocityRND)) * _scaleFactor;

				Particle p = ParticleBuffer.Instance.InactiveParticles.Pop();
				p.Revive(spawnPos + _owner.Position, spawnVel, lifetime, _fakeDepth, scaleFactor);
				_particles.Add(p);
			}
		}

		public override void Load(ContentManager content)
		{
			_pD.Load(content);
		}

		public override void Unload()
		{

		}

		public override void Update(double deltaTime)
		{
			if (_continuous)
			{
				if (_particles.Count < _maxParticles)
				{
					Emit();
				}
			}

			//remove dead particles
			for (int i = (_particles.Count - 1); i >= 0; i--)
			{
				Particle p = _particles[i];
				p.Update(deltaTime, _acceleration);
				if (p.Lifetime < 0)
				{
					ParticleBuffer.Instance.InactiveParticles.Push(p);
					_particles.Remove(p);
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 inv = new Vector2(1, -1);

			foreach (Particle p in _particles)
			{
				Color c = Color.White;
				if (_lerpColours.Count == 1)
					c = _lerpColours[0].Item1;
				if (_lerpColours.Count > 1)
				{
					double lifeRatio = 1 - (p.Lifetime / p.InitLifetime);
					Color a = new Color();
					Color b = new Color();
					double lerpAmount = 0;

					foreach (Tuple<Color, double> t in _lerpColours)
					{
						if (lifeRatio <= t.Item2)
						{
							b = t.Item1;
							int i = _lerpColours.IndexOf(t) - 1;
							if (i < 0)
							{
								a = new Color(0, 0, 0, 0);
								lerpAmount = lifeRatio / t.Item2;
							}
							else
							{
								a = _lerpColours[i].Item1;
								lerpAmount = (lifeRatio - _lerpColours[i].Item2) / (t.Item2 - _lerpColours[i].Item2);
							}
							break;
						}
					}
					c = Color.Lerp(a, b, (float)lerpAmount);
				}

				float layer = _pD._layer;
				if (p.Scale.X > 1.0f)
					layer += 0.01f;
				if (p.Scale.X < 1.0f)
					layer -= 0.01f;

				spriteBatch.Draw(ResourceManager.Instance.Textures[_pD._spriteName], p.Position * inv * _pD._spriteSize, _pD._sourceRectangle, c, p.Rotation, _pD._origin, p.Scale, _pD._spriteEffects, layer);
			}
		}

		public override void DebugDraw(SpriteBatch spriteBatch)
		{

		}
	}
}

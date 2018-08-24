using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
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
		protected double _lifeTime;

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

		public Emitter(Entity owner, Texture2D sprite) : base(owner)
		{
			_pD = new Drawable(sprite);
			_pD._sprite = sprite;
			_pD._spriteSize = 32;

			_pD._origin = new Vector2(16, 16);
			_position = new Vector2(0, 0);
			_scale = new Vector2(0.2f);
			_pD._layer = 0.0f;
			_pD._colour = new Color(255, 255, 255, 255);
			_pD._width = 32;
			_pD._height = 32;
			_pD._sourceRectangle = new Rectangle(0, 0, _pD._width, _pD._height);
			_pD._animations = new Dictionary<int, Animation>();
			_pD._currentAnim = 0;
			_pD._transition = false;
			_pD._nextAnim = 0;

			rnd = new Random();

			_particles = new List<Particle>();

			_acceleration = new Vector2(0.0f);

			_spawnWidth = 0;
			_spawnHeight = 0;

			_maxParticles = 3000;
			_continuous = true;
			_emissionCount = 50;
			_spawnVelocity = 3.0f;
			_lifeTime = 1.0;
		}

		public Emitter(Entity owner, Texture2D sprite, bool continuous, int maxParticles, int emissionCount, Vector2 acceleration, float spawnWidth = 0, float spawnHeight = 0, float spawnVelocity = 3.0f, double lifeTime = 3.0) : base(owner)
		{
			_pD = new Drawable(sprite);
			_pD._sprite = sprite;
			_pD._spriteSize = 32;

			_pD._origin = new Vector2(16, 16);
			_position = new Vector2(0, 0);
			_scale = new Vector2(0.2f);
			_pD._layer = 0.0f;
			_pD._colour = new Color(255, 255, 255, 255);
			_pD._width = 32;
			_pD._height = 32;
			_pD._sourceRectangle = new Rectangle(0, 0, _pD._width, _pD._height);
			_pD._animations = new Dictionary<int, Animation>();
			_pD._currentAnim = 0;
			_pD._transition = false;
			_pD._nextAnim = 0;

			rnd = new Random();

			_particles = new List<Particle>();

			_acceleration = acceleration;

			_spawnWidth = spawnWidth;
			_spawnHeight = spawnHeight;

			_maxParticles = maxParticles;
			_continuous = continuous;
			_emissionCount = emissionCount;
			_spawnVelocity = spawnVelocity;
			_lifeTime = lifeTime;
		}

		public void Emit()
		{
			for (int i = 0; i < _emissionCount; i++)
			{
				Vector2 spawnPos = new Vector2((float)rnd.NextDouble() * _spawnWidth - (_spawnWidth / 2), (float)rnd.NextDouble() * _spawnHeight - (_spawnHeight / 2));
				Vector2 spawnVel = new Vector2((float)rnd.NextDouble() * 2 - 1, (float)rnd.NextDouble() * 2 - 1);
				spawnVel.Normalize();
				spawnVel *= (_spawnVelocity * (float)rnd.NextDouble());
				Particle p = ParticleBuffer.Instance.InactiveParticles.Pop();
				p.Revive(spawnPos + _owner.GetPosition(), spawnVel, _lifeTime * rnd.NextDouble());
				_particles.Add(p);
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 inv = new Vector2(1, -1);
			foreach (Particle p in _particles)
			{
				Color c = Color.White;
				c *= p.Opacity;
				spriteBatch.Draw(_pD._sprite, p.Position * inv * _pD._spriteSize, _pD._sourceRectangle, c, p.Rotation, _pD._origin, _scale, _pD._spriteEffects, _pD._layer);
			}
		}

		public override void Update(double deltaTime)
		{
			if (_continuous)
			{
				if (_particles.Count < _maxParticles)
				{
					for (int i = 0; i < _emissionCount; i++)
					{
						Vector2 spawnPos = new Vector2((float)rnd.NextDouble() * _spawnWidth - (_spawnWidth / 2), (float)rnd.NextDouble() * _spawnHeight - (_spawnHeight / 2));
						Vector2 spawnVel = new Vector2((float)rnd.NextDouble() * 2 - 1, (float)rnd.NextDouble() * 2 - 1);
						spawnVel.Normalize();
						spawnVel *= (_spawnVelocity * (float)rnd.NextDouble());
						Particle p = ParticleBuffer.Instance.InactiveParticles.Pop();
						p.Revive(spawnPos + _owner.GetPosition(), spawnVel, _lifeTime * rnd.NextDouble());
						_particles.Add(p);
					}
				}
			}

			//remove dead particles
			for (int i = (_particles.Count - 1); i >= 0; i--)
			{
				Particle p = _particles[i];
				p.Update(deltaTime, _acceleration);
				if (p.LifeTime < 0)
				{
					ParticleBuffer.Instance.InactiveParticles.Push(p);
					_particles.Remove(p);
				}
			}
		}
	}
}

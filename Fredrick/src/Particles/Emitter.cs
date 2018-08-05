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

		/// <summary>
		/// How many particles are emitted per emission if not continuous
		/// </summary>
		int _emissionCount;

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

			Random rnd = new Random();

			_particles = new List<Particle>();

			_spawnWidth = 0;
			_spawnHeight = 0;

			_maxParticles = 300;
			_continuous = true;
			_emissionCount = 5;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 inv = new Vector2(1, -1);
			foreach (Particle p in _particles)
			{
				Color c = Color.LightGoldenrodYellow;
				c *= p.Opacity;
				spriteBatch.Draw(_pD._sprite, p.Position * inv * _pD._spriteSize, _pD._sourceRectangle, c, p.Rotation, _pD._origin, _scale, _pD._spriteEffects, _pD._layer);
			}
		}

		public override void Update(double deltaTime)
		{
			Random rnd = new Random();
			if (_continuous)
			{
				if (_particles.Count < _maxParticles)
				{
					for (int i = 0; i < _emissionCount; i++)
					{
						Vector2 spawnPos = new Vector2((float)rnd.NextDouble() * _spawnWidth - (_spawnWidth / 2), (float)rnd.NextDouble() * _spawnHeight - (_spawnHeight / 2));
						Vector2 spawnVel = new Vector2((float)rnd.NextDouble() * 4 - 2, (float)rnd.NextDouble() * 4 - 2);
						spawnVel.Normalize();
						spawnVel *= 2;
						Particle p = ParticleBuffer.Instance.InactiveParticles.Pop();
						p.Revive(spawnPos + _owner.GetPosition(), spawnVel, 3);
						_particles.Add(p);
					}
				}
			}

			//remove dead particles
			for (int i = (_particles.Count - 1); i >= 0; i--)
			{
				Particle p = _particles[i];
				p.Update(deltaTime, new Vector2(0, 0));
				if (p.LifeTime < 0)
				{
					ParticleBuffer.Instance.InactiveParticles.Push(p);
					_particles.Remove(p);
				}
			}
		}
	}
}

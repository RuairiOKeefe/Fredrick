using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fredrick.src
{
	public class Emitter : Renderable
	{
		List<Particle> _particles;
		float _spawnWidth;
		float _spawnHeight;
		float _maxParticles;

		public Emitter(Entity owner, Texture2D sprite) : base(owner, sprite)
		{
			_sprite = sprite;
			_spriteSize = 32;

			_origin = new Vector2(16, 16);
			_position = new Vector2(0, 0);
			_scale = new Vector2(1, 1);
			_layer = 0.2f;
			_colour = new Color(255, 255, 255, 255);
			_width = 32;
			_height = 32;
			_sourceRectangle = new Rectangle(0, 0, _width, _height);
			_animations = new Dictionary<int, Animation>();
			_currentAnim = 0;
			_transition = false;
			_nextAnim = 0;

			Random rnd = new Random();

			_particles = new List<Particle>();
			for (int i = 0; i < 500; i++)
			{
				Vector2 spawnPos = new Vector2((float)rnd.NextDouble() * 10 - 5, (float)rnd.NextDouble() * 4 - 2 + 10);
				Particle p = ParticleBuffer.Instance.InactiveParticles.Pop();
				p.Revive(spawnPos + _owner.GetPosition(), new Vector2((float)rnd.NextDouble(), -10), 2);
				_particles.Add(p);
			}

			_maxParticles = 3000;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 inv = new Vector2(1, -1);
			foreach (Particle p in _particles)
			{
				spriteBatch.Draw(_sprite, p.Position * inv * _spriteSize, _sourceRectangle, Color.LightBlue, _rotation, _origin, 0.05f, _spriteEffects, _layer);
			}
		}

		public override void Update(double deltaTime)
		{
			Random rnd = new Random();
			for (int i = 0; i < 20; i++)
			{
				if (_particles.Count < _maxParticles)
				{
					Vector2 spawnPos = new Vector2((float)rnd.NextDouble() * 30 - 15, (float)rnd.NextDouble() * 4 - 2 + 10);
					Particle p = ParticleBuffer.Instance.InactiveParticles.Pop();
					p.Revive(spawnPos + _owner.GetPosition(), new Vector2((float)rnd.NextDouble(), -10), 2);
					_particles.Add(p);
				}
			}

			for (int i = (_particles.Count - 1); i >= 0; i--)
			{
				Particle p = _particles[i];
				p.Update(deltaTime, new Vector2(0, -10));
				if (p.LifeTime < 0)
				{
					ParticleBuffer.Instance.InactiveParticles.Push(p);
					_particles.Remove(p);
				}
			}
		}
	}
}

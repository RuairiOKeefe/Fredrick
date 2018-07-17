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

		public Emitter(Entity owner, Texture2D sprite) : base(owner, sprite)
		{
			_sprite = sprite;
			_spriteSize = 32;

			_origin = new Vector2(16, 16);
			_position = new Vector2(0, 0);
			_scale = new Vector2(1, 1);
			_layer = 1;
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
				Vector2 p = new Vector2((float)rnd.NextDouble(), (float)rnd.NextDouble());
				_particles.Add(new Particle(p + owner.GetPosition(), new Vector2((float)rnd.NextDouble(), 10)));
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 inv = new Vector2(1, -1);
			foreach (Particle p in _particles)
			{
				spriteBatch.Draw(_sprite, p.Position * inv * _spriteSize, _sourceRectangle, _colour, _rotation, _origin, 0.05f, _spriteEffects, _layer);
			}
		}

		public override void Update(double deltaTime)
		{
			foreach (Particle p in _particles)
			{
				p.Update(deltaTime, new Vector2(0, -10));
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Fredrick.src
{
	public sealed class ProjectileBuffer
	{
		private static ProjectileBuffer _instance = null;
		private static readonly object _padlock = new object();

		public static ProjectileBuffer Instance
		{
			get
			{
				lock (_padlock)
				{
					if (_instance == null)
						_instance = new ProjectileBuffer();
					return _instance;
				}
			}
		}

		public const int NUM_PROJECTILES = 10000;

		private Stack<Entity> _inactiveProjectiles;


		public Stack<Entity> InactiveProjectiles
		{
			get { return _inactiveProjectiles; }
			set { _inactiveProjectiles = value; }
		}

		public void Load(String spriteName, ContentManager content)
		{
			ResourceManager.Instance.AddTexture(content, spriteName);
			ResourceManager.Instance.AddTexture(content, "explosion");
			ResourceManager.Instance.AddTexture(content, "tempParticle");

			List<Tuple<Color, double>> lerpColoursExE = new List<Tuple<Color, double>>();
			List<Tuple<Color, double>> lerpColoursEmE = new List<Tuple<Color, double>>();
			lerpColoursExE.Add(new Tuple<Color, double>(new Color(255, 192, 45, 255) * 0.8f, 0.0));
			lerpColoursExE.Add(new Tuple<Color, double>(new Color(255, 86, 25, 255) * 0.8f, 0.15));
			lerpColoursExE.Add(new Tuple<Color, double>(new Color(112, 103, 97, 255) * 0.4f, 0.2));
			lerpColoursExE.Add(new Tuple<Color, double>(new Color(61, 59, 58, 255) * 0.1f, 0.5));
			lerpColoursExE.Add(new Tuple<Color, double>(new Color(61, 59, 58, 255) * 0.0f, 1.0));

			lerpColoursEmE.Add(new Tuple<Color, double>(new Color(198, 48, 2, 255) * 0.5f, 0.0));
			lerpColoursEmE.Add(new Tuple<Color, double>(new Color(242, 192, 0, 255) * 0.8f, 0.3));
			lerpColoursEmE.Add(new Tuple<Color, double>(new Color(250, 243, 67, 255) * 0.8f, 0.4));
			lerpColoursEmE.Add(new Tuple<Color, double>(new Color(255, 255, 211, 255) * 0.5f, 0.5));
			lerpColoursEmE.Add(new Tuple<Color, double>(new Color(255, 255, 255, 255) * 0.0f, 1.0));
			for (int i = 0; i < NUM_PROJECTILES; i++)
			{
				Entity e = new Entity();
				Projectile p = new Projectile(e);
				CircleCollider cc = new CircleCollider(e);
				Renderable r = new Renderable(e, "Projectile", spriteName, new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
				r.Drawable.AddAnimation(0, 0, 1, 1, Animation.OnEnd.Loop, 0);
				r.Drawable.AddAnimation(32, 0, 1, 1, Animation.OnEnd.Loop, 0);

				Emitter explosionEmitter = new Emitter(e, "explosion", false, 1000, 600, new Vector2(0, 0), 0, 0, 17.0f, 0.6);
				explosionEmitter.SetLifeTime(0.6, 0, 0);
				explosionEmitter.SetVelocity(0.0f, 1.0f, 17.0f);
				explosionEmitter.SetCollision(true, false);
				explosionEmitter.SetScaling(false, 0.0f);
				explosionEmitter.LerpColours = lerpColoursExE;

				Emitter emberEmitter = new Emitter(e, "tempSpark", false, 1000, 50, new Vector2(0, -26.0f), 0, 0, 10.0f, 0.5);//need trails
				emberEmitter.ParticleDrawable = new Drawable("tempSpark", new Vector2(8), 16, 16, 0.1f);
				emberEmitter.Scale = new Vector2(0.5f);
				emberEmitter.SetLifeTime(0.0, 0.5, 1.2);
				emberEmitter.SetVelocity(0.0f, 10.5f, 12.0f, true);
				emberEmitter.SetCollision(true, true);
				emberEmitter.SetScaling(true, 0.5f);
				emberEmitter.LerpColours = lerpColoursEmE;

				e.Components.Add(p);
				e.Components.Add(cc);
				e.Components.Add(r);
				e.Components.Add(explosionEmitter);
				e.Components.Add(emberEmitter);

				_inactiveProjectiles.Push(e);//need to improve

				e.Load(content);
			}
		}

		public ProjectileBuffer()
		{
			_inactiveProjectiles = new Stack<Entity>(NUM_PROJECTILES);

		}
	}
}
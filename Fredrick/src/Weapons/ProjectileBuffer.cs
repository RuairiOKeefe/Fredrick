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

			lerpColoursEmE.Add(new Tuple<Color, double>(new Color(244, 212, 156, 255) * 0.2f, 0.0));
			lerpColoursEmE.Add(new Tuple<Color, double>(new Color(255, 231, 137, 255) * 0.5f, 0.5));
			lerpColoursEmE.Add(new Tuple<Color, double>(new Color(247, 223, 200, 255) * 0.1f, 0.9));
			lerpColoursEmE.Add(new Tuple<Color, double>(new Color(247, 223, 200, 255) * 0.0f, 1.0));
			for (int i = 0; i < NUM_PROJECTILES; i++)
			{
				Entity e = new Entity();
				Projectile p = new Projectile(e);
				CircleCollider cc = new CircleCollider(e);
				Renderable r = new Renderable(e, "Projectile", spriteName, new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
				r.Drawable.AddAnimation(0, 0, 1, 1);
				r.Drawable.AddAnimation(32, 0, 1, 1);
				Emitter explosionEmitter = new Emitter(e, "explosion", false, 1000, 600, new Vector2(0, 0), 0, 0, 17.0f, 0.6, false, false, 1.0f);
				explosionEmitter.LerpColours = lerpColoursExE;
				Emitter emberEmitter = new Emitter(e, "tempParticle", false, 1000, 000, new Vector2(0, -16.0f), 0, 0, 10.0f, 0.5, true, true, 0.1f);
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
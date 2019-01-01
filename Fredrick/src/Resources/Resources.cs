using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src
{
	public sealed class Resources
	{
		private static Resources instance = null;
		private static readonly object padlock = new object();

		public static Resources Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new Resources();
					}
					return instance;
				}
			}
		}

		public Dictionary<string, Renderable> Renderables { get; private set; } = new Dictionary<string, Renderable>();
		public Dictionary<string, Emitter> Emitters { get; private set; } = new Dictionary<string, Emitter>();
		public Dictionary<string, CircleCollider> CircleColliders { get; private set; } = new Dictionary<string, CircleCollider>();
		public Dictionary<string, Projectile> Projectiles { get; private set; } = new Dictionary<string, Projectile>();

		public Dictionary<string, Entity> ProjectileEntities { get; private set; } = new Dictionary<string, Entity>();


		Resources()
		{
			InitComponents();
			InitEntities();
		}

		public void InitComponents()
		{
			InitRenderables();
			InitEmitters();
			InitCircleColliders();
			InitProjectiles();
		}

		public void InitEntities()
		{
			InitProjectileEntities();
		}

		private void InitRenderables()
		{
			Renderable fragNade = new Renderable(null, "Projectile", "FragNade", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
			fragNade.Drawable.AddAnimation(0, 0, 1, 1, Animation.OnEnd.Loop, 0);
			fragNade.Drawable.AddAnimation(32, 0, 1, 1, Animation.OnEnd.Loop, 0);

			Renderables.Add("FragNade", fragNade);

		}

		private void InitEmitters()
		{
			Emitters = new Dictionary<string, Emitter>();

			List<Tuple<Color, double>> explosionLerp = new List<Tuple<Color, double>>();
			explosionLerp.Add(new Tuple<Color, double>(new Color(255, 192, 45, 255) * 0.8f, 0.0));
			explosionLerp.Add(new Tuple<Color, double>(new Color(255, 86, 25, 255) * 0.8f, 0.15));
			explosionLerp.Add(new Tuple<Color, double>(new Color(112, 103, 97, 255) * 0.4f, 0.2));
			explosionLerp.Add(new Tuple<Color, double>(new Color(61, 59, 58, 255) * 0.1f, 0.5));
			explosionLerp.Add(new Tuple<Color, double>(new Color(61, 59, 58, 255) * 0.0f, 1.0));
			Emitter explosion = new Emitter(null, "explosion", false, 1000, 600, new Vector2(0, 0), 0, 0, 17.0f, 0.6);
			explosion.SetLifeTime(0.6, 0, 0);
			explosion.SetVelocity(0.0f, 1.0f, 17.0f);
			explosion.SetCollision(true, false);
			explosion.SetScaling(false, 0.0f);
			explosion.LerpColours = explosionLerp;

			Emitters.Add("Explosion", explosion);

			List<Tuple<Color, double>> emberLerp = new List<Tuple<Color, double>>();
			emberLerp.Add(new Tuple<Color, double>(new Color(198, 48, 2, 255) * 0.5f, 0.0));
			emberLerp.Add(new Tuple<Color, double>(new Color(242, 192, 0, 255) * 0.8f, 0.3));
			emberLerp.Add(new Tuple<Color, double>(new Color(250, 243, 67, 255) * 0.8f, 0.4));
			emberLerp.Add(new Tuple<Color, double>(new Color(255, 255, 211, 255) * 0.5f, 0.5));
			emberLerp.Add(new Tuple<Color, double>(new Color(255, 255, 255, 255) * 0.0f, 1.0));
			Emitter embers = new Emitter(null, "tempSpark", false, 1000, 50, new Vector2(0, -26.0f), 0, 0, 10.0f, 0.5);//need trails
			embers.ParticleDrawable = new Drawable("tempSpark", new Vector2(8), 16, 16, 0.1f);
			embers.Scale = new Vector2(0.5f);
			embers.SetLifeTime(0.0, 0.5, 1.2);
			embers.SetVelocity(0.0f, 10.5f, 12.0f, true);
			embers.SetCollision(true, true);
			embers.SetScaling(true, 0.5f);
			embers.LerpColours = emberLerp;

			Emitters.Add("Embers", embers);

			List<Tuple<Color, double>> fireLerp = new List<Tuple<Color, double>>();
			fireLerp.Add(new Tuple<Color, double>(new Color(215, 48, 2, 255) * 0.5f, 0.0));
			fireLerp.Add(new Tuple<Color, double>(new Color(222, 172, 0, 255) * 0.8f, 0.3));
			fireLerp.Add(new Tuple<Color, double>(new Color(240, 223, 67, 255) * 0.8f, 0.4));
			fireLerp.Add(new Tuple<Color, double>(new Color(155, 155, 111, 255) * 0.5f, 0.5));
			fireLerp.Add(new Tuple<Color, double>(new Color(85, 85, 85, 255) * 0.0f, 1.0));
			Emitter fire = new Emitter(null, "Fire", true, 1000, 50, new Vector2(0, 15.0f), 0, 0, 10.0f, 0.5);
			fire.ParticleDrawable = new Drawable("Fire", new Vector2(8), 16, 16, 0.1f);
			fire.SetLifeTime(0.0, 0.5, 1.2);
			fire.SetVelocity(0.0f, 0.0f, 1.4f, false);
			fire.SetCollision(false, false);
			fire.SetScaling(true, 0.5f);
			fire.LerpColours = fireLerp;

			Emitters.Add("Fire", fire);
		}

		private void InitCircleColliders()
		{
			CircleCollider fragNade = new CircleCollider(null);
			CircleColliders.Add("FragNade", fragNade);
		}

		private void InitProjectiles()
		{
			Projectile fragNade = new Projectile(null);
			fragNade.Attack = new Attack(Attack.DamageType.Fire, new List<StatusEffect>() { new Burn() }, 10);
			Projectiles.Add("FragNade", fragNade);
		}

		private void InitProjectileEntities()
		{
			Entity fragNade = new Entity();
			fragNade.Components.Add(new Projectile(fragNade, Projectiles["FragNade"]));
			fragNade.Components.Add(new CircleCollider(fragNade, CircleColliders["FragNade"]));
			fragNade.Components.Add(new Renderable(fragNade, Renderables["FragNade"]));
			fragNade.Components.Add(new Emitter(fragNade, Emitters["Explosion"]));
			fragNade.Components.Add(new Emitter(fragNade, Emitters["Embers"]));

			ProjectileEntities.Add("FragNade", fragNade);
		}

		public void Load(ContentManager content)
		{
			foreach (KeyValuePair<string, Emitter> e in Emitters)
			{
				e.Value.Load(content);
			}
		}
	}
}

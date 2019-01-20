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
		public Dictionary<string, CharacterRig> CharacterRigs { get; private set; } = new Dictionary<string, CharacterRig>();

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
			InitCharacterRigs();
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

		private void InitCharacterRigs()
		{
			Bone Torso = new Bone("Torso", new Vector2(0), 0, new Vector2(0), new Vector2(0));
			Torso.Drawable = new Drawable("Torso", new Vector2(16), 32, 32, 0.15f);
			Bone Head = new Bone("Head", new Vector2(0), 0, new Vector2(2/32f, 25 / 32f), new Vector2(0, -5 / 32f));
			Head.Drawable = new Drawable("Head", new Vector2(32), 64, 64, 0.14f);
			Torso.AddChild(Head);
			Bone UpperLegFront = new Bone("UpperLegFront", new Vector2(0), 0, new Vector2(0 / 32f, -14 / 32f), new Vector2(0.0f, -4 / 32f));
			UpperLegFront.Drawable = new Drawable("UpperLeg", new Vector2(16), 32, 32, 0.1f);
			Torso.AddChild(UpperLegFront);
			Bone LowerLegFront = new Bone("LowerLegFront", new Vector2(0), 0, new Vector2(0.0f, -6 / 32f), new Vector2(0.0f, -9 / 32f));
			LowerLegFront.Drawable = new Drawable("LowerLeg", new Vector2(16), 32, 32, 0.15f);
			UpperLegFront.AddChild(LowerLegFront);
			Bone FootFront = new Bone("FootFront", new Vector2(0), 0, new Vector2(0.0f, -13 / 32f), new Vector2(0.0f, 0.0f));
			FootFront.Drawable = new Drawable("Foot", new Vector2(16), 32, 32, 0.1f);
			LowerLegFront.AddChild(FootFront);
			Bone ToesFront = new Bone("ToesFront", new Vector2(0), 0, new Vector2(2 / 32f, 0.0f), new Vector2(1 / 32f, 0.0f));
			ToesFront.Drawable = new Drawable("Toes", new Vector2(16), 32, 32, 0.1f);
			FootFront.AddChild(ToesFront);

			Bone UpperLegBack = new Bone("UpperLegBack", new Vector2(0), 0, new Vector2(-0 / 32f, -14 / 32f), new Vector2(0.0f, -4 / 32f));
			UpperLegBack.Drawable = new Drawable("UpperLeg", new Vector2(16), 32, 32, 0.1f);
			Torso.AddChild(UpperLegBack);
			Bone LowerLegBack = new Bone("LowerLegBack", new Vector2(0), 0, new Vector2(0.0f, -6 / 32f), new Vector2(0.0f, -9 / 32f));
			LowerLegBack.Drawable = new Drawable("LowerLeg", new Vector2(16), 32, 32, 0.15f);
			UpperLegBack.AddChild(LowerLegBack);
			Bone FootBack = new Bone("FootBack", new Vector2(0), 0, new Vector2(0.0f, -13 / 32f), new Vector2(0.0f, 0.0f));
			FootBack.Drawable = new Drawable("Foot", new Vector2(16), 32, 32, 0.1f);
			LowerLegBack.AddChild(FootBack);
			Bone ToesBack = new Bone("ToesBack", new Vector2(0), 0, new Vector2(2 / 32f, 0.0f), new Vector2(1 / 32f, 0.0f));
			ToesBack.Drawable = new Drawable("Toes", new Vector2(16), 32, 32, 0.1f);
			FootBack.AddChild(ToesBack);

			List<RigFrame> animation = new List<RigFrame>();
			Dictionary<string, float> playerContactFront = new Dictionary<string, float>();
			playerContactFront.Add("Torso", 0.0f);
			playerContactFront.Add("Head", 0.0f);
			playerContactFront.Add("UpperLegFront", -1.0f);
			playerContactFront.Add("LowerLegFront", -0.2f);
			playerContactFront.Add("FootFront", 0.4f);
			playerContactFront.Add("ToesFront", -0.3f);
			playerContactFront.Add("UpperLegBack", 0.8f);
			playerContactFront.Add("LowerLegBack", 0.6f);
			playerContactFront.Add("FootBack", 0.2f);
			playerContactFront.Add("ToesBack", -0.6f);
			animation.Add(new RigFrame(playerContactFront, new Vector2(0, 0.45f), 0.12));

			Dictionary<string, float> playerDownFront = new Dictionary<string, float>();
			playerDownFront.Add("Torso", 0.0f);
			playerDownFront.Add("Head", 0.0f);
			playerDownFront.Add("UpperLegFront", -0.5f);
			playerDownFront.Add("LowerLegFront", 1.4f);
			playerDownFront.Add("FootFront", -0.4f);
			playerDownFront.Add("ToesFront", -0.1f);
			playerDownFront.Add("UpperLegBack", -0.8f);
			playerDownFront.Add("LowerLegBack", 1.7f);
			playerDownFront.Add("FootBack", 0.4f);
			playerDownFront.Add("ToesBack", -0.2f);
			animation.Add(new RigFrame(playerDownFront, new Vector2(0, 0.3f), 0.12));

			Dictionary<string, float> playerContFirstFront = new Dictionary<string, float>();
			playerContFirstFront.Add("Torso", 0.0f);
			playerContFirstFront.Add("Head", 0.0f);
			playerContFirstFront.Add("UpperLegFront", 0.8f);
			playerContFirstFront.Add("LowerLegFront", 0.3f);
			playerContFirstFront.Add("FootFront", 0.6f);
			playerContFirstFront.Add("ToesFront", -0.3f);
			playerContFirstFront.Add("UpperLegBack", -1.6f);
			playerContFirstFront.Add("LowerLegBack", 1.9f);
			playerContFirstFront.Add("FootBack", 0.5f);
			playerContFirstFront.Add("ToesBack", 0.0f);
			animation.Add(new RigFrame(playerContFirstFront, new Vector2(0, 0.45f), 0.12));

			Dictionary<string, float> playerContSecondFront = new Dictionary<string, float>();
			playerContSecondFront.Add("Torso", 0.0f);
			playerContSecondFront.Add("Head", 0.0f);
			playerContSecondFront.Add("UpperLegFront", 1.4f);
			playerContSecondFront.Add("LowerLegFront", 0.4f);
			playerContSecondFront.Add("FootFront", 0.8f);
			playerContSecondFront.Add("ToesFront", 0.2f);
			playerContSecondFront.Add("UpperLegBack", -1.8f);
			playerContSecondFront.Add("LowerLegBack", 0.7f);
			playerContSecondFront.Add("FootBack", -0.4f);
			playerContSecondFront.Add("ToesBack", -0.3f);
			animation.Add(new RigFrame(playerContSecondFront, new Vector2(0, 0.5f), 0.12));

			Dictionary<string, float> playerContactBack = new Dictionary<string, float>();
			playerContactBack.Add("Torso", 0.0f);
			playerContactBack.Add("Head", 0.0f);
			playerContactBack.Add("UpperLegFront", 0.8f);
			playerContactBack.Add("LowerLegFront", 0.6f);
			playerContactBack.Add("FootFront", 0.2f);
			playerContactBack.Add("ToesFront", -0.6f);
			playerContactBack.Add("UpperLegBack", -1.0f);
			playerContactBack.Add("LowerLegBack", -0.2f);
			playerContactBack.Add("FootBack", 0.4f);
			playerContactBack.Add("ToesBack", -0.3f);
			animation.Add(new RigFrame(playerContactBack, new Vector2(0, 0.45f), 0.12));

			Dictionary<string, float> playerDownBack = new Dictionary<string, float>();
			playerDownBack.Add("Torso", 0.0f);
			playerDownBack.Add("Head", 0.0f);
			playerDownBack.Add("UpperLegFront", -0.8f);
			playerDownBack.Add("LowerLegFront", 1.7f);
			playerDownBack.Add("FootFront", 0.4f);
			playerDownBack.Add("ToesFront", -0.2f);
			playerDownBack.Add("UpperLegBack", -0.5f);
			playerDownBack.Add("LowerLegBack", 1.4f);
			playerDownBack.Add("FootBack", -0.4f);
			playerDownBack.Add("ToesBack", -0.1f);
			animation.Add(new RigFrame(playerDownBack, new Vector2(0, 0.3f), 0.12));

			Dictionary<string, float> playerContFirstBack = new Dictionary<string, float>();
			playerContFirstBack.Add("Torso", 0.0f);
			playerContFirstBack.Add("Head", 0.0f);
			playerContFirstBack.Add("UpperLegFront", -1.6f);
			playerContFirstBack.Add("LowerLegFront", 1.9f);
			playerContFirstBack.Add("FootFront", 0.5f);
			playerContFirstBack.Add("ToesFront", 0.0f);
			playerContFirstBack.Add("UpperLegBack", 0.8f);
			playerContFirstBack.Add("LowerLegBack", 0.3f);
			playerContFirstBack.Add("FootBack", 0.6f);
			playerContFirstBack.Add("ToesBack", -0.3f);
			animation.Add(new RigFrame(playerContFirstBack, new Vector2(0, 0.45f), 0.12));

			Dictionary<string, float> playerContSecondBack = new Dictionary<string, float>();
			playerContSecondBack.Add("Torso", 0.0f);
			playerContSecondBack.Add("Head", 0.0f);
			playerContSecondBack.Add("UpperLegFront", -1.8f);
			playerContSecondBack.Add("LowerLegFront", 0.7f);
			playerContSecondBack.Add("FootFront", -0.4f);
			playerContSecondBack.Add("ToesFront", -0.3f);
			playerContSecondBack.Add("UpperLegBack", 1.4f);
			playerContSecondBack.Add("LowerLegBack", 0.4f);
			playerContSecondBack.Add("FootBack", 0.8f);
			playerContSecondBack.Add("ToesBack", 0.2f);
			animation.Add(new RigFrame(playerContSecondBack, new Vector2(0, 0.5f), 0.12));

			CharacterRigs.Add("Player", new CharacterRig(null, "testrig", Torso, animation));
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

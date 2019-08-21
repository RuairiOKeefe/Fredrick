﻿using Microsoft.Xna.Framework;
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

		//Separate resource classes should be created for components with complex construction 
		private AnimationResources AnimationResources;

		public Dictionary<string, AABBCollider> AABBColliders { get; private set; } = new Dictionary<string, AABBCollider>();
		public Dictionary<string, CharacterRig> CharacterRigs { get; private set; } = new Dictionary<string, CharacterRig>();
		public Dictionary<string, Character> Characters { get; private set; } = new Dictionary<string, Character>();
		public Dictionary<string, CircleCollider> CircleColliders { get; private set; } = new Dictionary<string, CircleCollider>();
		public Dictionary<string, Damageable> Damageables { get; private set; } = new Dictionary<string, Damageable>();
		public Dictionary<string, Emitter> Emitters { get; private set; } = new Dictionary<string, Emitter>();
		public Dictionary<string, Projectile> Projectiles { get; private set; } = new Dictionary<string, Projectile>();
		public Dictionary<string, Renderable> Renderables { get; private set; } = new Dictionary<string, Renderable>();
		public Dictionary<string, StatusHandler> StatusHandlers { get; private set; } = new Dictionary<string, StatusHandler>();
		public Dictionary<string, Weapon> Weapons { get; private set; } = new Dictionary<string, Weapon>();

		public Dictionary<string, Entity> ProjectileEntities { get; private set; } = new Dictionary<string, Entity>();
		public Dictionary<string, Entity> PlayerEntities { get; private set; } = new Dictionary<string, Entity>();

		Resources()
		{
			AnimationResources = new AnimationResources();
			InitComponents();
			InitEntities();
		}

		public void InitComponents()
		{
			InitAABBColliders();
			InitCharacterRigs();
			InitCharacters();
			InitCircleColliders();
			InitDamageables();
			InitEmitters();
			InitProjectiles();
			InitRenderables();
			InitStatusHandlers();
			InitWeapons();
		}

		public void InitEntities()
		{
			InitPlayerEntities();
			InitProjectileEntities();
		}

		private void InitAABBColliders()
		{
			AABBCollider playerCollider = new AABBCollider(null, new Vector2(0), 0.6f, 2.0f);
			AABBColliders.Add("PlayerCollider", playerCollider);
		}

		private void InitCharacterRigs()
		{
			CharacterRigs.Add("PlayerLegs", new CharacterRig(null, AnimationResources.GetPlayerLegs()));
			CharacterRigs.Add("PlayerArms", new CharacterRig(null, AnimationResources.GetPlayerArms()));

			foreach (KeyValuePair<string, CharacterRig> rig in CharacterRigs)
			{
				rig.Value.Root.PopulateRig(ref rig.Value.Bones);
			}
		}

		private void InitCharacters()
		{
			Character player = new Character(null);
			Characters.Add("Player", player);
		}

		private void InitCircleColliders()
		{
			CircleCollider fragNade = new CircleCollider(null);
			CircleColliders.Add("FragNade", fragNade);
		}

		private void InitDamageables()
		{
			Damageable playerDamageable = new Damageable(null, "Health");
			playerDamageable.MaxHealth = 100;
			playerDamageable.BaseResistance = new Damageable.Resistances(1, 1, 1);
			Damageables.Add("PlayerDamageable", playerDamageable);
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
			explosion.SetVelocity(0.0f, 1.0f, 17.0f, 1.0f, false);
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
			Emitter embers = new Emitter(null, "Spark", false, 1000, 50, new Vector2(0, -26.0f), 0, 0, 10.0f, 0.5);//need trails
			embers.ParticleDrawable = new Drawable("Spark", new Vector2(2, 1.5f), 4, 3, 0.1f);
			embers.Scale = new Vector2(0.5f);
			embers.SetLifeTime(0.0, 0.5, 1.2);
			embers.SetVelocity(0.0f, 10.5f, 12.0f, 1.0f, true);
			embers.SetCollision(true, true);
			embers.SetScaling(true, 0.5f);
			embers.Restitution = 0.8f;
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
			fire.SetVelocity(0.0f, 0.0f, 1.4f, 1.0f, false);
			fire.SetCollision(false, false);
			fire.SetScaling(true, 0.5f);
			fire.LerpColours = fireLerp;

			Emitters.Add("Fire", fire);

			List<Tuple<Color, double>> landDustLerp = new List<Tuple<Color, double>>();
			landDustLerp.Add(new Tuple<Color, double>(new Color(200, 200, 200, 255) * 0.1f, 0.0));
			landDustLerp.Add(new Tuple<Color, double>(new Color(200, 200, 200, 255) * 0.0f, 1.0));
			Emitter landDust = new Emitter(null, "explosion", false, 100, 50, new Vector2(0, 0.0f), 0, 0, 10.0f, 0.5);
			landDust.ParticleDrawable = new Drawable("fire", new Vector2(8), 16, 16, 0.1f);
			landDust.SetLifeTime(0.0, 0.6, 0.9);
			landDust.SetVelocity(0.0f, 0.1f, 2.5f, 20.0f, false);
			landDust.SetCollision(false, false);
			landDust.SetScaling(false, 1.0f);
			landDust.LerpColours = landDustLerp;
			landDust.Tags.Add("Landing");

			Emitters.Add("LandDust", landDust);

			List<Tuple<Color, double>> jumpDustLerp = new List<Tuple<Color, double>>();
			jumpDustLerp.Add(new Tuple<Color, double>(new Color(200, 200, 200, 255) * 0.1f, 0.0));
			jumpDustLerp.Add(new Tuple<Color, double>(new Color(200, 200, 200, 255) * 0.0f, 1.0));
			Emitter jumpDust = new Emitter(null, "explosion", false, 100, 50, new Vector2(0, 0.0f), 0, 0, 10.0f, 0.5);
			jumpDust.ParticleDrawable = new Drawable("fire", new Vector2(8), 16, 16, 0.1f);
			jumpDust.SetLifeTime(0.0, 0.6, 0.9);
			jumpDust.SetVelocity(0.0f, 0.1f, 2.5f, 0.2f, false);
			jumpDust.SetCollision(false, false);
			jumpDust.SetScaling(false, 1.0f);
			jumpDust.LerpColours = landDustLerp;
			jumpDust.Tags.Add("Jumping");

			Emitters.Add("JumpDust", jumpDust);
		}

		private void InitProjectiles()
		{
			Projectile fragNade = new Projectile(null);
			Projectiles.Add("FragNade", fragNade);
		}

		private void InitRenderables()
		{
			Renderable fragNade = new Renderable(null, "Projectile", "FragNade", new Vector2(4), new Vector2(0), new Vector2(1), 8, 8, 0.1f);
			fragNade.Drawable.AddAnimation(0, 0, 1, 1, Animation.OnEnd.Loop, 0);
			fragNade.Drawable.AddAnimation(32, 0, 1, 1, Animation.OnEnd.Loop, 0);

			Renderables.Add("FragNade", fragNade);

		}

		private void InitStatusHandlers()
		{
			StatusHandler playerStatus = new StatusHandler(null, "PlayerStatus");
			StatusHandlers.Add("PlayerStatus", playerStatus);
		}

		private void InitWeapons()
		{
			Weapon fragGrenade = new Weapon(null, "FragGrenade", new Vector2(0.5f, 0), new Vector2(0.5f, 0), true);
			fragGrenade.Position = new Vector2(0, 0.5f);
			fragGrenade.WeaponDrawable = new Drawable("fragNade", new Vector2(4), 8, 8, 0.2f);
			fragGrenade.Tags.Add("MotionFlip");
			Attack fragImpactAttack = new Attack(Attack.DamageType.Kinetic, new List<StatusEffect>(), 10.0f);
			Attack fragAreaAttack = new Attack(Attack.DamageType.Kinetic, new List<StatusEffect>(), 40.0f);
			fragGrenade.InitialiseAttack(fragImpactAttack, fragAreaAttack, 0.5f, 0.3f, 5.0f, 0.0f, 0.05f, 2.0, false, true);
			//fragWeapon.Tags.Add("Body");
			Weapons.Add("FragGrenade", fragGrenade);
		}


		void InitPlayerEntities()
		{
			Entity player = new Entity(true, "Player");
			player.Tags.Add("Actor");
			player.Components.Add(new PlayerController(player, "Controller"));
			player.Components.Add(new Character(player, Characters["Player"]));
			player.Components.Add(new AABBCollider(player, AABBColliders["PlayerCollider"]));

			player.Components.Add(new CharacterRig(player, CharacterRigs["PlayerLegs"]));
			player.Components.Add(new CharacterRig(player, CharacterRigs["PlayerArms"]));
			player.Components.Add(new Damageable(player, Damageables["PlayerDamageable"]));
			player.Components.Add(new StatusHandler(player, StatusHandlers["PlayerStatus"]));
			player.Components.Add(new Weapon(player, Weapons["FragGrenade"]));
			player.Components.Add(new Emitter(player, Emitters["LandDust"]));
			player.Components[player.Components.Count - 1].Position = new Vector2(0.3f, -1.0f);
			player.Components.Add(new Emitter(player, Emitters["JumpDust"]));
			player.Components[player.Components.Count - 1].Position = new Vector2(0.3f, -1.0f);
			PlayerEntities.Add("Player", player);
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

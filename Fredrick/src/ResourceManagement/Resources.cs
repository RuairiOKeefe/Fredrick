using Fredrick.src.Colliders;
using Fredrick.src.Rigging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fredrick.src.ResourceManagement
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
		//Lighting should have it's singleton status revoked once levels have been refactored to use resources
		//private LightingResources LightingResources;

		public Dictionary<string, AABBCollider> AABBColliders { get; private set; } = new Dictionary<string, AABBCollider>();
		public Dictionary<string, CharacterRig> CharacterRigs { get; private set; } = new Dictionary<string, CharacterRig>();
		public Dictionary<string, Character> Characters { get; private set; } = new Dictionary<string, Character>();
		public Dictionary<string, CircleCollider> CircleColliders { get; private set; } = new Dictionary<string, CircleCollider>();
		public Dictionary<string, Damageable> Damageables { get; private set; } = new Dictionary<string, Damageable>();
		public Dictionary<string, Emitter> Emitters { get; private set; } = new Dictionary<string, Emitter>();
		public Dictionary<string, OOBBTrigger> OOBBTriggers { get; private set; } = new Dictionary<string, OOBBTrigger>();
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
			InitOOBBTriggers();
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
			ColliderCategory colliderCategory = ColliderCategory.Actor;
			ColliderCategory collidesWith = ColliderCategory.Terrain | ColliderCategory.Actor | ColliderCategory.Bullet;
			AABBCollider playerCollider = new AABBCollider(null, "PlayerCollider", new Vector2(0), 0.6f, 2.0f, colliderCategory, collidesWith);
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
			ColliderCategory colliderCategory = ColliderCategory.Bullet;
			ColliderCategory collidesWith = ColliderCategory.Terrain | ColliderCategory.Actor | ColliderCategory.Bullet;
			CircleCollider fragNade = new CircleCollider(null, "FragNade", 3 / 32f, 0.3f, colliderCategory, collidesWith);
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
			ShaderInfo.Material particleMaterial;
			particleMaterial.Emissive = new Color(0, 0, 0, 255);
			particleMaterial.Diffuse = new Color(255, 255, 255, 255);
			particleMaterial.Specular = new Color(255, 255, 255, 255);
			particleMaterial.Shininess = 0.8f;

			Emitters = new Dictionary<string, Emitter>();

			{
				List<Tuple<Color, double>> explosionLerp = new List<Tuple<Color, double>>();
				explosionLerp.Add(new Tuple<Color, double>(new Color(255, 192, 45, 255) * 0.8f, 0.0));
				explosionLerp.Add(new Tuple<Color, double>(new Color(255, 86, 25, 255) * 0.8f, 0.15));
				explosionLerp.Add(new Tuple<Color, double>(new Color(112, 103, 97, 255) * 0.4f, 0.2));
				explosionLerp.Add(new Tuple<Color, double>(new Color(61, 59, 58, 255) * 0.1f, 0.5));
				explosionLerp.Add(new Tuple<Color, double>(new Color(61, 59, 58, 255) * 0.0f, 1.0));
				Emitter explosion = new Emitter(null, "explosion", false, false, 1000, 600, new Vector2(0, 0), 0, 0, 17.0f, 0.6);
				explosion.SetLifeTime(0.6, 0, 0);
				explosion.SetVelocity(0.0f, 1.0f, 17.0f, 1.0f, false);
				explosion.SetCollision(true, false);
				explosion.SetScaling(1.0f, false, 0.0f);
				explosion.LerpColours = explosionLerp;

				explosion.Tags.Add("Detonation");
				Emitters.Add("Explosion", explosion);
			}
			{
				List<Tuple<Color, double>> emberLerp = new List<Tuple<Color, double>>();
				emberLerp.Add(new Tuple<Color, double>(new Color(198, 48, 2, 255) * 0.5f, 0.0));
				emberLerp.Add(new Tuple<Color, double>(new Color(242, 192, 0, 255) * 0.8f, 0.3));
				emberLerp.Add(new Tuple<Color, double>(new Color(250, 243, 67, 255) * 0.8f, 0.4));
				emberLerp.Add(new Tuple<Color, double>(new Color(255, 255, 211, 255) * 0.5f, 0.5));
				emberLerp.Add(new Tuple<Color, double>(new Color(255, 255, 255, 255) * 0.0f, 1.0));
				Emitter embers = new Emitter(null, "Spark", false, false, 1000, 50, new Vector2(0, -26.0f), 0, 0, 10.0f, 0.5);//need trails
				embers.ParticleDrawable = new Drawable("Spark", new Vector2(2, 1.5f), 4, 3, 0, 0, 0.1f);
				embers.Scale = new Vector2(0.5f);
				embers.SetLifeTime(0.0, 0.5, 1.2);
				embers.SetVelocity(0.0f, 10.5f, 12.0f, 1.0f, true);
				embers.SetCollision(true, true);
				embers.SetScaling(1.0f, true, 0.5f);
				embers.Restitution = 0.8f;
				embers.LerpColours = emberLerp;

				embers.Tags.Add("Detonation");
				Emitters.Add("Embers", embers);
			}
			{
				List<Tuple<Color, double>> fireLerp = new List<Tuple<Color, double>>();
				fireLerp.Add(new Tuple<Color, double>(new Color(215, 48, 2, 255) * 0.5f, 0.0));
				fireLerp.Add(new Tuple<Color, double>(new Color(222, 172, 0, 255) * 0.8f, 0.3));
				fireLerp.Add(new Tuple<Color, double>(new Color(240, 223, 67, 255) * 0.8f, 0.4));
				fireLerp.Add(new Tuple<Color, double>(new Color(155, 155, 111, 255) * 0.5f, 0.5));
				fireLerp.Add(new Tuple<Color, double>(new Color(85, 85, 85, 255) * 0.0f, 1.0));
				Emitter fire = new Emitter(null, "Fire", true, true, 1000, 50, new Vector2(0, 15.0f), 0, 0, 10.0f, 0.5);
				fire.ParticleDrawable = new Drawable("Fire", new Vector2(8), 16, 16, 0, 0, 0.1f);
				fire.SetLifeTime(0.0, 0.5, 1.2);
				fire.SetVelocity(0.0f, 0.0f, 1.4f, 1.0f, false);
				fire.SetCollision(false, false);
				fire.SetScaling(1.0f, true, 0.5f);
				fire.LerpColours = fireLerp;

				Emitters.Add("Fire", fire);
			}
			{
				List<Tuple<Color, double>> AoELerp = new List<Tuple<Color, double>>();
				AoELerp.Add(new Tuple<Color, double>(new Color(255, 255, 255, 255) * 0.0f, 0.0));
				AoELerp.Add(new Tuple<Color, double>(new Color(255, 255, 255, 255) * 0.5f, 0.7));
				AoELerp.Add(new Tuple<Color, double>(new Color(255, 255, 255, 255) * 0.0f, 1.0));
				Emitter areaIndicator = new Emitter(null, "AoECircle", false, false, 1, 1, new Vector2(0), 0, 0, 10.0f, 0.5);
				areaIndicator.ParticleDrawable = new Drawable("AoECircle", new Vector2(127.5f), 256, 256, 0, 0, 0.1f);
				areaIndicator.SetLifeTime(0.5, 0.0, 0.0);
				areaIndicator.SetVelocity(0.0f, 0.0f, 0.0f, 0.0f, false);
				areaIndicator.SetCollision(false, false);
				areaIndicator.SetScaling(1.0f, false, 0.5f);
				areaIndicator.LerpColours = AoELerp;

				areaIndicator.Tags.Add("Detonation");
				areaIndicator.Tags.Add("AreaIndicator");
				Emitters.Add("AreaIndicator", areaIndicator);
			}
			{
				List<Tuple<Color, double>> fragTrailLerp = new List<Tuple<Color, double>>();
				fragTrailLerp.Add(new Tuple<Color, double>(new Color(30, 48, 30, 255) * 0.2f, 0.0));
				fragTrailLerp.Add(new Tuple<Color, double>(new Color(50, 80, 50, 255) * 0.4f, 0.4));
				fragTrailLerp.Add(new Tuple<Color, double>(new Color(10, 10, 10, 255) * 0.0f, 1.0));
				Emitter fragTrail = new Emitter(null, "explosion", true, true, 100, 10, new Vector2(0), 0.2f, 0.2f, 10.0f, 0.5);
				fragTrail.ParticleDrawable = new Drawable("fire", new Vector2(8), 16, 16, 0, 0, 0.3f);
				fragTrail.SetLifeTime(0.3, 0.0, 0.0);
				fragTrail.SetVelocity(0.0f, 0.1f, 0.5f, 1.0f, false);
				fragTrail.SetCollision(false, false);
				fragTrail.SetScaling(0.5f, false, 1.0f);
				fragTrail.LerpColours = fragTrailLerp;

				fragTrail.Tags.Add("Trail");
				Emitters.Add("FragTrail", fragTrail);
			}
			{
				List<Tuple<Color, double>> landDustLerp = new List<Tuple<Color, double>>();
				landDustLerp.Add(new Tuple<Color, double>(new Color(200, 200, 200, 255) * 0.1f, 0.0));
				landDustLerp.Add(new Tuple<Color, double>(new Color(200, 200, 200, 255) * 0.0f, 1.0));
				Emitter landDust = new Emitter(null, "explosion", false, false, 100, 50, new Vector2(0, 0.0f), 0, 0, 10.0f, 0.5);
				landDust.ParticleDrawable = new Drawable("fire", new Vector2(8), 16, 16, 0, 0, 0.1f);
				landDust.ParticleDrawable.ShaderInfo = new LightingInfo("explosionNormal", particleMaterial);
				landDust.SetLifeTime(0.0, 0.6, 0.9);
				landDust.SetVelocity(0.0f, 0.1f, 2.5f, 20.0f, false);
				landDust.SetCollision(false, false);
				landDust.SetScaling(1.0f, false, 1.0f);
				landDust.LerpColours = landDustLerp;

				landDust.Tags.Add("Landing");
				Emitters.Add("LandDust", landDust);
			}
			{
				List<Tuple<Color, double>> landDustLerp = new List<Tuple<Color, double>>();
				landDustLerp.Add(new Tuple<Color, double>(new Color(200, 200, 200, 255) * 0.1f, 0.0));
				landDustLerp.Add(new Tuple<Color, double>(new Color(200, 200, 200, 255) * 0.0f, 1.0));
				Emitter landDust = new Emitter(null, "explosion", false, false, 100, 50, new Vector2(0, 0.0f), 0, 0, 10.0f, 0.5);
				List<Tuple<Color, double>> jumpDustLerp = new List<Tuple<Color, double>>();
				jumpDustLerp.Add(new Tuple<Color, double>(new Color(200, 200, 200, 255) * 0.1f, 0.0));
				jumpDustLerp.Add(new Tuple<Color, double>(new Color(200, 200, 200, 255) * 0.0f, 1.0));
				Emitter jumpDust = new Emitter(null, "explosion", true, false, 50, 10, new Vector2(0, 0.0f), 0, 0, 10.0f, 0.5);
				jumpDust.ParticleDrawable = new Drawable("fire", new Vector2(8), 16, 16, 0, 0, 0.1f);
				jumpDust.ParticleDrawable.ShaderInfo = new LightingInfo("explosionNormal", particleMaterial);
				jumpDust.SetLifeTime(0.0, 0.3, 0.5);
				jumpDust.SetVelocity(0.0f, 0.1f, 1.2f, 1.0f, false);
				jumpDust.SetCollision(false, false);
				jumpDust.SetScaling(1.0f, false, 1.0f);
				jumpDust.LerpColours = landDustLerp;

				jumpDust.Tags.Add("Jumping");
				Emitters.Add("JumpDust", jumpDust);
			}
			{
				List<Tuple<Color, double>> pistolFlashLerp = new List<Tuple<Color, double>>();
				pistolFlashLerp.Add(new Tuple<Color, double>(new Color(255, 192, 45, 255) * 0.8f, 0.0));
				pistolFlashLerp.Add(new Tuple<Color, double>(new Color(255, 86, 25, 255) * 0.8f, 0.15));
				pistolFlashLerp.Add(new Tuple<Color, double>(new Color(112, 103, 97, 255) * 0.4f, 0.2));
				pistolFlashLerp.Add(new Tuple<Color, double>(new Color(61, 59, 58, 255) * 0.1f, 0.5));
				pistolFlashLerp.Add(new Tuple<Color, double>(new Color(61, 59, 58, 255) * 0.0f, 1.0));
				Emitter pistolFlash = new Emitter(null, "explosion", false, false, 100, 40, new Vector2(0, 0), 0, 0, 2.0f, 0.1);
				pistolFlash.SetLifeTime(0.05, 0.0, 0.2);
				pistolFlash.SetVelocity(2.0f, 0.0f, 1.8f, 1.0f, false);
				pistolFlash.SetCollision(true, false);
				pistolFlash.SetScaling(0.1f, false, 0.5f);
				pistolFlash.Cone = true;
				pistolFlash.Angle = 0.6f;
				pistolFlash.LerpColours = pistolFlashLerp;

				Emitters.Add("PistolFlash", pistolFlash);
			}
			{
				List<Tuple<Color, double>> smallRoundLerp = new List<Tuple<Color, double>>();
				smallRoundLerp.Add(new Tuple<Color, double>(new Color(216, 176, 25, 255) * 1.0f, 0.0));
				smallRoundLerp.Add(new Tuple<Color, double>(new Color(216, 176, 25, 255) * 0.5f, 0.8));
				smallRoundLerp.Add(new Tuple<Color, double>(new Color(216, 176, 25, 255) * 0.0f, 1.0));
				Emitter smallRoundTrail = new Emitter(null, "SmallTrail", true, true, 1000, 20, new Vector2(0, 0), 0.0f, 0.0f, 0.0f, 0.1);
				smallRoundTrail.ParticleDrawable = new Drawable("SmallTrail", new Vector2(3.5f), 8, 8);
				smallRoundTrail.SetLifeTime(0.08, 0.0, 0.0);
				smallRoundTrail.SetVelocity(0.0f, 0.0f, 0.0f, 1.0f, false);
				smallRoundTrail.SetCollision(false, false);
				smallRoundTrail.SetScaling(0.4f, true, -0.5f);
				smallRoundTrail.LerpColours = smallRoundLerp;
				smallRoundTrail.Trail = true;

				Emitters.Add("SmallTrail", smallRoundTrail);
			}
		}

		private void InitOOBBTriggers()
		{
			{
				ColliderCategory colliderCategory = ColliderCategory.Bullet;
				ColliderCategory collidesWith = ColliderCategory.Terrain | ColliderCategory.Actor;
				OOBBTrigger smallRound = new OOBBTrigger(null, "smallRoundTrigger", new Vector2(0), 4 / 32f, 2 / 32f, 0f, colliderCategory, collidesWith);
				OOBBTriggers.Add("SmallRound", smallRound);
			}
		}

		private void InitProjectiles()
		{
			{
				Projectile fragNade = new Projectile(null);
				Projectiles.Add("FragNade", fragNade);
			}
		}

		private void InitRenderables()
		{
			{
				ShaderInfo.Material fragMaterial;
				fragMaterial.Emissive = new Color(0, 0, 0, 255);
				fragMaterial.Diffuse = new Color(100, 150, 100, 255);
				fragMaterial.Specular = new Color(255, 255, 255, 255);
				fragMaterial.Shininess = 0.8f;
				LightingInfo fragLighting = new LightingInfo("FragNadeNormal", fragMaterial);
				Renderable fragNade = new Renderable(null, "FragNade", "FragNade", new Vector2(7.5f), new Vector2(0), new Vector2(1), 16, 16, 0, 0, 0.1f);
				fragNade.Drawable.AddAnimation(0, 0, 1, 1, Animation.OnEnd.Loop, 0);
				fragNade.Drawable.AddAnimation(32, 0, 1, 1, Animation.OnEnd.Loop, 0);
				fragNade.Drawable.ShaderInfo = fragLighting;

				Renderables.Add("FragNade", fragNade);
			}
			{
				ShaderInfo.Material smallRoundMaterial;
				smallRoundMaterial.Emissive = new Color(0, 0, 0, 255);
				smallRoundMaterial.Diffuse = new Color(150, 150, 100, 255);
				smallRoundMaterial.Specular = new Color(255, 255, 255, 255);
				smallRoundMaterial.Shininess = 0.9f;
				LightingInfo smallRoundLighting = new LightingInfo("SmallRoundNormal", smallRoundMaterial);
				Renderable smallRound = new Renderable(null, "SmallRound", "SmallRound", new Vector2(8f), new Vector2(0), new Vector2(1), 16, 16, 0, 0, 0.1f);
				smallRound.Drawable.AddAnimation(0, 0, 1, 1, Animation.OnEnd.Loop, 0);
				smallRound.Drawable.AddAnimation(32, 0, 1, 1, Animation.OnEnd.Loop, 0);
				smallRound.Drawable.ShaderInfo = smallRoundLighting;

				Renderables.Add("SmallRound", smallRound);
			}
			{
				ShaderInfo.Material tempMaterial;
				tempMaterial.Emissive = new Color(0, 0, 0, 255);
				tempMaterial.Diffuse = new Color(150, 150, 100, 255);
				tempMaterial.Specular = new Color(255, 255, 255, 255);
				tempMaterial.Shininess = 0.9f;
				LightingInfo tempLighting = new LightingInfo("FogSpriteNormal", tempMaterial);
				Renderable temp = new Renderable(null, "Fog", "FogSprite", new Vector2(7.5f), new Vector2(0), new Vector2(100), 16, 16, 0, 0, 0.9f);
				temp.Drawable.AddAnimation(0, 0, 1, 1, Animation.OnEnd.Loop, 0);
				temp.Drawable.AddAnimation(32, 0, 1, 1, Animation.OnEnd.Loop, 0);
				temp.Drawable.ShaderInfo = tempLighting;

				Renderables.Add("Temp", temp);
			}
		}

		private void InitStatusHandlers()
		{
			StatusHandler playerStatus = new StatusHandler(null, "PlayerStatus");
			StatusHandlers.Add("PlayerStatus", playerStatus);
		}

		private void InitWeapons()
		{
			{
				ShaderInfo.Material fragMaterial;
				fragMaterial.Emissive = new Color(0, 0, 0, 255);
				fragMaterial.Diffuse = new Color(74, 74, 74, 255);
				fragMaterial.Specular = new Color(255, 255, 255, 255);
				fragMaterial.Shininess = 0.9f;

				LightingInfo lightingInfo = new LightingInfo("fragNadeNormal", fragMaterial);
				Weapon fragGrenade = new Weapon(null, "FragGrenade", "FragNade", new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0), true);
				fragGrenade.Position = new Vector2(0, 0.5f);
				fragGrenade.WeaponDrawable = new Drawable("fragNade", new Vector2(4), 8, 8, 0, 0, 0.2f);
				fragGrenade.WeaponDrawable.ShaderInfo = lightingInfo;
				fragGrenade.Tags.Add("MotionFlip");
				Attack fragImpactAttack = new Attack(Attack.DamageType.Kinetic, new List<StatusEffect>(), 10.0f);
				Attack fragAreaAttack = new Attack(Attack.DamageType.Kinetic, new List<StatusEffect>(), 40.0f);
				fragGrenade.InitialiseAttack(fragImpactAttack, fragAreaAttack, 0.5f, 0.3f, 5.0f, 0.0f, 0.05f, 0.6f, 2.0, false, true);
				Weapons.Add("FragGrenade", fragGrenade);
			}
			{
				ShaderInfo.Material pistolMaterial;
				pistolMaterial.Emissive = new Color(0, 0, 0, 255);
				pistolMaterial.Diffuse = new Color(74, 74, 74, 255);
				pistolMaterial.Specular = new Color(255, 255, 255, 255);
				pistolMaterial.Shininess = 0.9f;

				LightingInfo lightingInfo = new LightingInfo("PistolNormal", pistolMaterial);
				Weapon pistol = new Weapon(null, "Pistol", "SmallRound", new Vector2(36 / 32f, 1.5f / 32f), new Vector2(1.0f, 0f), new Vector2(-9.5f / 32f, -4.5f / 32f), true);
				pistol.Position = new Vector2(0f, 0.8f);
				pistol.WeaponDrawable = new Drawable("Pistol", new Vector2(16, 16), 32, 32, 0, 0, 0.1f);
				pistol.WeaponDrawable.ShaderInfo = lightingInfo;
				pistol.FireEmitters.Add(new Emitter(null, Emitters["PistolFlash"]));
				pistol.Tags.Add("MotionFlip");
				Attack pistolImpactAttack = new Attack(Attack.DamageType.Kinetic, new List<StatusEffect>(), 10.0f);
				Attack pistolAreaAttack = new Attack(Attack.DamageType.Kinetic, new List<StatusEffect>(), 40.0f);
				pistol.InitialiseAttack(pistolImpactAttack, pistolAreaAttack, 0.2f, 20.0f, 5.0f, 0.0f, 0.05f, 0.0f, 2.0, true, true);
				Weapons.Add("Pistol", pistol);


			}
		}


		void InitPlayerEntities()
		{
			Entity player = new Entity(true, "Player");
			player.Tags.Add("Actor");
			player.Components.Add(new PlayerController(player, "Controller"));
			player.Components.Add(new Character(player, Characters["Player"]));
			player.Components.Add(new AABBCollider(player, AABBColliders["PlayerCollider"]));
			player.Components.Add(new Weapon(player, Weapons["Pistol"]));
			player.Components.Add(new CharacterRig(player, CharacterRigs["PlayerLegs"]));
			//player.Components.Add(new CharacterRig(player, CharacterRigs["PlayerArms"]));
			player.Components.Add(new IKSolver(player, AnimationResources.GetIKArm()));
			player.Components.Add(new Damageable(player, Damageables["PlayerDamageable"]));
			player.Components.Add(new StatusHandler(player, StatusHandlers["PlayerStatus"]));

			player.Components.Add(new Emitter(player, Emitters["LandDust"]));
			player.Components[player.Components.Count - 1].Position = new Vector2(0.0f, -1.0f);
			player.Components.Add(new Emitter(player, Emitters["JumpDust"]));
			player.Components[player.Components.Count - 1].Position = new Vector2(0.0f, -1.0f);
			player.Components.Add(new Renderable(player, Renderables["Temp"]));
			PlayerEntities.Add("Player", player);
		}

		private void InitProjectileEntities()
		{
			{
				Entity fragNade = new Entity(true, "FragNade");
				fragNade.Components.Add(new Projectile(fragNade, Projectiles["FragNade"]));
				fragNade.Components.Add(new CircleCollider(fragNade, CircleColliders["FragNade"]));
				fragNade.Components.Add(new Emitter(fragNade, Emitters["Explosion"]));
				fragNade.Components.Add(new Emitter(fragNade, Emitters["Embers"]));
				fragNade.Components.Add(new Emitter(fragNade, Emitters["AreaIndicator"]));
				fragNade.Components.Add(new Emitter(fragNade, Emitters["FragTrail"]));
				fragNade.Components.Add(new Renderable(fragNade, Renderables["FragNade"]));

				ProjectileEntities.Add("FragNade", fragNade);
			}
			{
				Entity smallRound = new Entity(true, "SmallRound");
				smallRound.Components.Add(new Projectile(smallRound));
				smallRound.Components.Add(new Renderable(smallRound, Renderables["SmallRound"]));
				smallRound.Components.Add(new OOBBTrigger(smallRound, OOBBTriggers["SmallRound"]));
				smallRound.Components.Add(new Emitter(smallRound, Emitters["SmallTrail"]));

				ProjectileEntities.Add("SmallRound", smallRound);
			}
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision;

namespace Fredrick.src
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		List<Entity> actors = new List<Entity>();
		List<Entity> terrain = new List<Entity>();
		Entity UI = new Entity();
		FollowCamera cam;
		Effect lighting;
		Effect bloom;

		RenderTarget2D sceneTarget;
		RenderTarget2D bloomTarget;

		Serializer serializer;
		LevelEditor levelEditor;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 1600;
			graphics.PreferredBackBufferHeight = 900;
			this.IsMouseVisible = true;
			Content.RootDirectory = "Content";

			//graphics.PreferredBackBufferWidth = 1920;
			//graphics.PreferredBackBufferHeight = 1080;
			//graphics.IsFullScreen = true;

			serializer = new Serializer();
			levelEditor = new LevelEditor();
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			sceneTarget = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
			bloomTarget = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
			// create 1x1 texture for line drawing
			DebugManager.Instance.LineTex = new Texture2D(GraphicsDevice, 1, 1);
			DebugManager.Instance.LineTex.SetData<Color>(new Color[] { Color.White });// fill the texture with white

			lighting = Content.Load<Effect>("Lighting");
			bloom = Content.Load<Effect>("Bloom");

			ColliderManager.Instance.Load();
			ProjectileBuffer.Instance.Load(Content);

			if (true)
			{
				for (int i = 0; i < 2; i++)
				{
					Entity entity = new Entity(true, "Player");
					entity.Position = new Vector2(8, 8);
					actors.Add(entity);

					Renderable renderable = new Renderable(entity, "Legs", "Character", new Vector2(32, 32), new Vector2(0), new Vector2(1), 64, 64, 0.3f);
					renderable.Drawable.AddAnimation(0, 0, 1, 12, Animation.OnEnd.Loop, 0);
					renderable.Drawable.AddAnimation(64, 0, 10, 12, Animation.OnEnd.Loop, 0);
					renderable.Drawable.AddAnimation(192, 128, 3, 12, Animation.OnEnd.LockLastFrame, 0);
					renderable.Drawable.AddAnimation(64, 192, 3, 12, Animation.OnEnd.LockLastFrame, 0);
					renderable.Tags.Add("MotionFlip");
					renderable.Tags.Add("Legs");
					entity.Components.Add(renderable);

					Character character = new Character(entity);
					entity.Components.Add(character);

					PlayerController playerController = new PlayerController(entity, "Controller");
					playerController.Keyboard = true;
					entity.Components.Add(playerController);

					AABBCollider boxCollider = new AABBCollider(entity, new Vector2(0), 0.6f, 2.0f);
					entity.Components.Add(boxCollider);

					Emitter emitter = new Emitter(entity, "tempParticle", true, 3000, 20, new Vector2(0, -0), 0, 0, 2, 0.0);
					emitter.ParticleDrawable = new Drawable("tempParticle", new Vector2(4), 8, 8, 0.1f);
					emitter.ParticleDrawable.AddAnimation(0, 0, 1, 30, Animation.OnEnd.Loop, 0);
					entity.Components.Add(emitter);

					Weapon weapon = new Weapon(entity, "Grenade", new Vector2(0.8f, 0), new Vector2(0.8f, 0), 0.1, 4.0f, 20.0f, 6.0f, true);
					weapon.Position = new Vector2(0, 0.5f);
					weapon.WeaponDrawable = new Drawable("fragNade", new Vector2(16), 32, 32, 0.2f);
					weapon.ArmDrawable = new Drawable("Arm", new Vector2(31, 31), 64, 64, 0.1f);
					weapon.Tags.Add("MotionFlip");
					entity.Components.Add(weapon);

					Damageable damageable = new Damageable(entity, "Health");
					damageable.Health = 100000;
					damageable.BaseResistance = new Damageable.Resistances(1, 1, 1);
					entity.Components.Add(damageable);

					StatusHandler statusHandler = new StatusHandler(entity, "StatusHandler");
					entity.Components.Add(statusHandler);

					Renderable headRenderable = new Renderable(entity, "Head", "Head", new Vector2(32, 32), new Vector2(0), new Vector2(1), 64, 64, 0.3f);
					headRenderable.Position = new Vector2(0, 0.875f);
					headRenderable.Tags.Add("MotionFlip");
					entity.Components.Add(headRenderable);

					//Arm

					SortedDictionary<int, Vector2> aps = new SortedDictionary<int, Vector2>();
					aps.Add(0, new Vector2(0, 0.53125f));
					renderable.Drawable._animations[0].MountPoints.Add(weapon, aps);

					SortedDictionary<int, Vector2> apr = new SortedDictionary<int, Vector2>();
					apr.Add(0, new Vector2(0, 0.53125f));
					apr.Add(3, new Vector2(0, 0.5625f));
					apr.Add(5, new Vector2(0, 0.53125f));
					apr.Add(8, new Vector2(0, 0.5625f));
					renderable.Drawable._animations[1].MountPoints.Add(weapon, apr);

					SortedDictionary<int, Vector2> apj = new SortedDictionary<int, Vector2>();
					apj.Add(0, new Vector2(0, 0.5625f));
					apj.Add(1, new Vector2(0, 0.5f));
					apj.Add(2, new Vector2(0, 0.475f));
					renderable.Drawable._animations[2].MountPoints.Add(weapon, apj);

					SortedDictionary<int, Vector2> apl = new SortedDictionary<int, Vector2>();
					apl.Add(0, new Vector2(0, 0.475f));
					apl.Add(1, new Vector2(0, 0.5f));
					apl.Add(2, new Vector2(0, 0.5625f));
					renderable.Drawable._animations[3].MountPoints.Add(weapon, apl);

					//Head

					SortedDictionary<int, Vector2> hps = new SortedDictionary<int, Vector2>();
					hps.Add(0, new Vector2(0, 0.875f));
					renderable.Drawable._animations[0].MountPoints.Add(headRenderable, hps);

					SortedDictionary<int, Vector2> hpr = new SortedDictionary<int, Vector2>();
					hpr.Add(0, new Vector2(0, 0.875f));
					hpr.Add(3, new Vector2(0, 0.90625f));
					hpr.Add(5, new Vector2(0, 0.875f));
					hpr.Add(8, new Vector2(0, 0.90625f));
					renderable.Drawable._animations[1].MountPoints.Add(headRenderable, hpr);

					SortedDictionary<int, Vector2> hpj = new SortedDictionary<int, Vector2>();
					hpj.Add(0, new Vector2(0, 0.90625f));
					hpj.Add(1, new Vector2(0, 0.84375f));
					hpj.Add(2, new Vector2(0, 0.78125f));
					renderable.Drawable._animations[2].MountPoints.Add(headRenderable, hpj);

					SortedDictionary<int, Vector2> hpl = new SortedDictionary<int, Vector2>();
					hpl.Add(0, new Vector2(0, 0.78125f));
					hpl.Add(1, new Vector2(0, 0.84375f));
					hpl.Add(2, new Vector2(0, 0.90625f));
					renderable.Drawable._animations[3].MountPoints.Add(headRenderable, hpl);
				}
				actors[1].Components.Remove(actors[1].GetDerivedComponent<Controller>());
				PatrolAI cont = new PatrolAI(actors[1], "Controller");
				actors[1].Components.Add(cont);

				actors[1].Position = new Vector2(32, 8);

				for (int i = 0; i < 101; i++)
				{
					for (int j = 0; j < 101; j++)
					{
						if (i < 2 || i > 98 || j < 2 || j > 98)
						{
							Entity e = new Entity(true, "Block");
							e.Position = new Vector2(i, j);
							terrain.Add(e);
							Renderable r = new Renderable(e, "Block", "Dirt", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
							AABBCollider c = new AABBCollider(e, new Vector2(0));
							r.Drawable.AddAnimation(0, 0, 1, 30, Animation.OnEnd.Loop, 0);
							e.Components.Add(r);
							e.Components.Add(c);
						}
					}
				}

				for (int i = 10; i < 68; i++)
				{
					for (int j = 5; j < 6; j++)
					{
						if (i % 8 == 0 || i % 8 == 1 || i % 8 == 2 || i % 8 == 3)
						{
							Entity e = new Entity(true, "Block");
							e.Position = new Vector2(i, j);
							terrain.Add(e);
							Renderable r = new Renderable(e, "Block", "Dirt", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
							AABBCollider c = new AABBCollider(e, new Vector2(0));
							r.Drawable.AddAnimation(0, 0, 1, 30, Animation.OnEnd.Loop, 0);
							e.Components.Add(r);
							e.Components.Add(c);
						}
					}
				}

				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 4; j++)
					{
						if (i == j)
						{
							Entity e = new Entity(true, "Block");
							e.Position = new Vector2(i + 3, j + 2);
							terrain.Add(e);
							Renderable r = new Renderable(e, "Block", "Dirt", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
							AABBCollider c = new AABBCollider(e, new Vector2(0));
							r.Drawable.AddAnimation(0, 0, 1, 30, Animation.OnEnd.Loop, 0);
							e.Components.Add(r);
							e.Components.Add(c);
						}
					}
				}

				serializer.Save("terrainData", terrain);
				serializer.Save("actorsData", actors);
			}
			else
			{
				terrain = serializer.Load("terrainData");
				actors = serializer.Load("actorsData");
			}

			foreach (Entity e in terrain)
			{
				e.Load(Content);
			}

			foreach (Entity e in actors)
			{
				e.Load(Content);
			}
			cam = new FollowCamera(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, actors[0], 1.0f, 1.0f, 0.2f, 2.0f);
			cam.OffsetAmount = new Vector2(4.0f, 1.8f);
			levelEditor.Load(Content);


			Canvas canvas = new Canvas(UI, "UI");
			TextElement debugElement1 = new TextElement(Content.Load<SpriteFont>("Debug"), new Vector2(0), Color.White, 0, TextElement.Justification.Left, 1.0f);
			debugElement1.AddContent("Player Health: ", "", actors[0].GetComponent<Damageable>(), "Health");
			canvas.TextElements.Add(debugElement1);
			TextElement debugElement2 = new TextElement(Content.Load<SpriteFont>("Debug"), new Vector2(0,40), Color.White, 0, TextElement.Justification.Left, 1.0f);
			debugElement2.AddContent("Enemy Health: ", "", actors[1].GetComponent<Damageable>(), "Health");
			canvas.TextElements.Add(debugElement2);
			UI.Components.Add(canvas);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			InputHandler.Instance.Update(cam.Get_Transformation(GraphicsDevice));
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			foreach (var e in actors)
				e.Update(gameTime.ElapsedGameTime.TotalSeconds);
			foreach (var e in terrain)
				e.Update(gameTime.ElapsedGameTime.TotalSeconds);

			ColliderManager.Instance.Update(gameTime.ElapsedGameTime.TotalSeconds);

			foreach (var e in actors)
			{
				if (e.GetComponent<Weapon>() != null)
					e.GetComponent<Weapon>().UpdateProjectilePos();
			}
			foreach (var e in terrain)
			{
				if (e.GetComponent<Weapon>() != null)
					e.GetComponent<Weapon>().UpdateProjectilePos();
			}
			levelEditor.Update(gameTime.ElapsedGameTime.TotalSeconds, ref terrain, Content);
			//cam.Trauma = 1;
			cam.Update(gameTime.ElapsedGameTime.TotalSeconds);
			UI.Update(gameTime.ElapsedGameTime.TotalSeconds);

			if (InputHandler.Instance.IsKeyPressed(InputHandler.Action.Debug))
				DebugManager.Instance.Debug = !DebugManager.Instance.Debug;

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			//aaaaaaaaaaaaa
			PostProcessing p = new PostProcessing();
			p.Draw(spriteBatch, GraphicsDevice, actors);
			//aaaaaaaaaaaaaaaaaa

			GraphicsDevice.SetRenderTarget(sceneTarget);

			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, lighting, cam.Get_Transformation(GraphicsDevice));
			foreach (var e in actors)
				e.Draw(spriteBatch);
			foreach (var e in terrain)
				e.Draw(spriteBatch);
			levelEditor.Draw(spriteBatch);
			spriteBatch.End();

			GraphicsDevice.SetRenderTarget(null);

			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
			spriteBatch.Draw(sceneTarget, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
			spriteBatch.End();

			if (DebugManager.Instance.Debug)
			{
				spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, cam.Get_Transformation(GraphicsDevice));
				foreach (var e in actors)
					e.DebugDraw(spriteBatch);
				foreach (var e in terrain)
					e.DebugDraw(spriteBatch);
				levelEditor.DebugDraw(spriteBatch);
				spriteBatch.End();
			}

			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
			UI.Draw(spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}

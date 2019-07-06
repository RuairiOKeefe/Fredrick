using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision;
using System;

namespace Fredrick.src
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Background background;
		List<PlayerInput> playerInputs = new List<PlayerInput>();
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
			Resources.Instance.Load(Content);
			SpawnManager.Instance.Load();

			background = new Background();
			background.AddLayer(new Drawable("Sky", new Vector2(1920 / 2, 1080 / 2), 1920, 1080, 0), 0.95f, 0.95f);
			background.AddLayer(new Drawable("Mountains", new Vector2(1024 / 2, 578), 1024, 1024, 0), 0.9f, 0.9f);
			background.AddLayer(new Drawable("Hills", new Vector2(1024 / 2, 720), 1024, 1024, 0), 0.8f, 0.8f);
			background.AddLayer(new Drawable("Trees", new Vector2(1024 / 2, 580), 1024, 1024, 0), 0.6f, 0.6f);
			background.AddLayer(new Drawable("Trees", new Vector2((1024 / 2), 580), 1024, 1024, 0), 0.58f, 0.58f, 100, 0);
			background.AddLayer(new Drawable("Trees", new Vector2((1024 / 2), 580), 1024, 1024, 0), 0.56f, 0.56f, 200, 0);
			background.AddLayer(new Drawable("Trees", new Vector2((1024 / 2), 580), 1024, 1024, 0), 0.54f, 0.54f, 300, 0);

			if (true)
			{
				Entity player = new Entity(Resources.Instance.PlayerEntities["Player"]);
				player.Active = false;
				player.GetComponent<PlayerController>().PlayerInput = new PlayerInput(PlayerIndex.One, true, false);
				player.Position = new Vector2(8, 8);
				actors.Add(player);

				Entity player2 = new Entity(Resources.Instance.PlayerEntities["Player"]);
				player2.Active = false;
				player2.GetComponent<PlayerController>().PlayerInput = new PlayerInput(PlayerIndex.One, false, true);
				player2.Position = new Vector2(16, 8);
				actors.Add(player2);

				SpawnManager.Instance.AddSpawnable(ref player, new Timer(3.0), new Vector2(8, 8));
				SpawnManager.Instance.AddSpawnable(ref player2, new Timer(3.0), new Vector2(16, 8));

				SpawnManager.Instance.Spawn(player);
				SpawnManager.Instance.Spawn(player2);

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

			background.Load(Content);

			foreach (Entity e in terrain)
			{
				e.Load(Content);
			}

			foreach (Entity e in actors)
			{
				e.Load(Content);
			}
			cam = new FollowCamera(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, actors[0], 3f, 1.0f, 0.2f, 2.0f);
			cam.OffsetAmount = new Vector2(4.0f, 1.8f);
			levelEditor.Load(Content);


			Canvas canvas = new Canvas(UI, "UI");
			TextElement debugElement1 = new TextElement(Content.Load<SpriteFont>("Debug"), new Vector2(0), Color.White, 0, TextElement.Justification.Left, 1.0f);
			debugElement1.AddContent("Player Health: ", "", actors[0].GetComponent<Damageable>(), "Health");
			canvas.TextElements.Add(debugElement1);
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

			SpawnManager.Instance.Update(gameTime.ElapsedGameTime.TotalSeconds);

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

			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, cam.Get_Transformation(GraphicsDevice));
			background.Draw(spriteBatch, cam.Position);
			spriteBatch.End();

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

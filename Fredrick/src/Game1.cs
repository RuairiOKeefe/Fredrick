using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision;
using System;

using Resources = Fredrick.src.ResourceManagement.Resources;
using Fredrick.src.Scenes;

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
		Effect fog;
		Lighting mainLighting = new Lighting();

		RenderTarget2D sceneTarget;
		RenderTarget2D bloomTarget;

		Serializer serializer;
		LevelEditor levelEditor;

		ScoreBoard scoreBoard = new ScoreBoard();

		DrawManager drawManager;

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

			drawManager = new DrawManager();
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
			fog = Content.Load<Effect>("Fog");

			ColliderManager.Instance.Load();
			ProjectileBuffer.Instance.Load(Content);
			Resources.Instance.Load(Content);
			SpawnManager.Instance.Load();
			LightingResources.Instance.Load();

			Vector4 fogColour = new Vector4(0.2f, 0.3f, 0.4f, 1.0f);
			background = new Background();
			background.AddLayer(new Drawable("Sky", new Vector2(1024 / 2, 1024 / 2), 1024, 1024, 0), 1.0f, 1.0f, fogColour, 0.25f);
			background.AddLayer(new Drawable("Clouds", new Vector2(1024 / 2, 410), 1024, 1024, 0), 0.95f, 0.95f, fogColour, 0.2f);
			background.AddLayer(new Drawable("Clouds", new Vector2(1024 / 2, 410), 1024, 1024, 0), 0.93f, 0.93f, fogColour, 0.15f, 600, 0);
			background.AddLayer(new Drawable("Clouds", new Vector2(1024 / 2, 410), 1024, 1024, 0), 0.91f, 0.91f, fogColour, 0.1f, 300, 0);
			background.AddLayer(new Drawable("Mountains", new Vector2(1024 / 2, 578), 1024, 1024, 0), 0.9f, 0.9f, fogColour, 0.7f);
			background.AddLayer(new Drawable("Clouds", new Vector2(1024 / 2, 470), 1024, 1024, 0), 0.87f, 0.87f, fogColour, 0.05f, 230, 0);
			background.AddLayer(new Drawable("Clouds", new Vector2(1024 / 2, 470), 1024, 1024, 0), 0.85f, 0.85f, fogColour, 0, 760, 0);
			background.AddLayer(new Drawable("Hills", new Vector2(1024 / 2, 720), 1024, 1024, 0), 0.8f, 0.8f, fogColour, 0.6f);
			background.AddLayer(new Drawable("Trees", new Vector2(1024 / 2, 610), 1024, 1024, 0), 0.6f, 0.6f, fogColour, 0.44f);
			background.AddLayer(new Drawable("Trees", new Vector2((1024 / 2), 610), 1024, 1024, 0), 0.58f, 0.58f, fogColour, 0.36f, 100, 0);
			background.AddLayer(new Drawable("Trees", new Vector2((1024 / 2), 610), 1024, 1024, 0), 0.56f, 0.56f, fogColour, 0.28f, 650, 0);
			background.AddLayer(new Drawable("Trees", new Vector2((1024 / 2), 610), 1024, 1024, 0), 0.54f, 0.54f, fogColour, 0.2f, -430, 0);

			if (true)
			{
				terrain = LevelManager.Instance.CurrentLevel.GetBlocks();

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

				/////////////////// test code fix and add to game manager
				PointTracker p1DeathTracker = new PointTracker();
				ScoreTracker p1killTracker = new ScoreTracker();
				PointTracker p2DeathTracker = new PointTracker();
				ScoreTracker p2killTracker = new ScoreTracker();
				p1killTracker.PointTrackers.Add(p2DeathTracker);
				p2killTracker.PointTrackers.Add(p1DeathTracker);
				scoreBoard.ScoreTrackers.Add(p1killTracker);
				scoreBoard.ScoreTrackers.Add(p2killTracker);


				player.GetComponent<Damageable>().Subscribe(scoreBoard.ScoreTrackers[1].PointTrackers[0]);
				player2.GetComponent<Damageable>().Subscribe(scoreBoard.ScoreTrackers[0].PointTrackers[0]);
				///////////////////

				SpawnManager.Instance.AddSpawnable(ref player, new Timer(3.0), new Vector2(8, 8));
				SpawnManager.Instance.AddSpawnable(ref player2, new Timer(3.0), new Vector2(16, 8));

				SpawnManager.Instance.Spawn(player);
				SpawnManager.Instance.Spawn(player2);

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
				drawManager.AddComponents(e);
			}

			foreach (Entity e in actors)
			{
				e.Load(Content);
				drawManager.AddComponents(e);
			}

			Entity cameraPos = new Entity();
			cameraPos.Position = new Vector2(20, 8);
			cam = new FollowCamera(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, actors[0], 2f, 1.0f, 0.2f, 2.0f);
			cam.OffsetAmount = new Vector2(4.0f, 1.8f);
			levelEditor.Load(Content);

			for (int i = 0; i < 1; i++)
			{
				mainLighting.FixedLights.Add(LightingResources.Instance.PointLights["BasicLight"].Copy());
			}


			Canvas canvas = new Canvas(UI, "UI");
			//TextElement debugElement1 = new TextElement(Content.Load<SpriteFont>("Debug"), new Vector2(0), Color.White, 0, TextElement.Justification.Left, 1.0f);
			//debugElement1.AddContent("Player Health: ", "", actors[0].GetComponent<Damageable>(), "Health");
			//canvas.TextElements.Add(debugElement1);

			TextElement player1Score = new TextElement(Content.Load<SpriteFont>("Debug"), new Vector2(graphics.PreferredBackBufferWidth / 2 - 10, 0), Color.Red, 0, TextElement.Justification.Right, 1.0f);
			player1Score.AddContent("", "", scoreBoard.ScoreTrackers[0], "Score");
			canvas.TextElements.Add(player1Score);

			TextElement player2Score = new TextElement(Content.Load<SpriteFont>("Debug"), new Vector2(graphics.PreferredBackBufferWidth / 2 + 10, 0), Color.Blue, 0, TextElement.Justification.Left, 1.0f);
			player2Score.AddContent("", "", scoreBoard.ScoreTrackers[1], "Score");
			canvas.TextElements.Add(player2Score);

			UI.Components.Add(canvas);

			ResourceManager.Instance.AddTexture(Content, "dirtNormal");

			drawManager.Load(Content);
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
			double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;
			InputHandler.Instance.Update(cam.Get_Transformation(GraphicsDevice));
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
			if (!levelEditor.editing)
				foreach (var e in actors)
					e.Update(deltaTime);
			foreach (var e in terrain)
				e.Update(deltaTime);

			ColliderManager.Instance.Update(deltaTime);

			ProjectileBuffer.Instance.Update(deltaTime);
			ParticleBuffer.Instance.Update(deltaTime);

			levelEditor.Update(deltaTime, ref terrain, Content);

			ScreenShakeManager.Instance.Update(deltaTime);
			cam.Update(deltaTime);

			scoreBoard.Update();
			UI.Update(deltaTime);

			SpawnManager.Instance.Update(deltaTime);

			if (InputHandler.Instance.IsKeyPressed(InputHandler.Action.Debug))
				DebugManager.Instance.Debug = !DebugManager.Instance.Debug;

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			Matrix world = Matrix.Identity;
			Matrix view = (cam.Get_Transformation(GraphicsDevice));
			Matrix projection = Matrix.CreateOrthographicOffCenter(0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 0, -1, 1);
			Matrix wvp = world * view * projection;

			GraphicsDevice.SetRenderTarget(sceneTarget);
			GraphicsDevice.Clear(Color.Transparent);

			background.Draw(spriteBatch, GraphicsDevice, cam, fog);
			drawManager.Draw(spriteBatch, cam.Get_Transformation(GraphicsDevice), wvp, mainLighting);

			//Draw editor stuff (which should be moved)
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, lighting, cam.Get_Transformation(GraphicsDevice));
			levelEditor.Draw(spriteBatch);
			spriteBatch.End();

			//Combine layers
			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(Color.Transparent);
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
			spriteBatch.Draw(sceneTarget, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
			spriteBatch.End();

			//Draw debug stuff
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

			//Draw Ui
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
			UI.DrawBatch(spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}

﻿using Microsoft.Xna.Framework;
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

			cam = new FollowCamera(1600, 900);
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
			ProjectileBuffer.Instance.Load("fragNade", Content);

			if (true)
			{
				Entity entity = new Entity(true, "Player");
				entity.Position = new Vector2(8, 8);
				actors.Add(entity);
				Renderable renderable = new Renderable(entity, "Body", "TestSheet", new Vector2(8, 8), new Vector2(0), new Vector2(2), 16, 16, 0.3f);
				Character character = new Character(entity);
				AABBCollider boxCollider = new AABBCollider(entity, new Vector2(0), 1.0f, 1.0f);
				renderable.Drawable.AddAnimation(0, 32, 1, 30);
				renderable.Drawable.AddAnimation(0, 32, 4, 30);
				entity.Components.Add(renderable);
				entity.Components.Add(character);
				entity.Components.Add(boxCollider);
				Emitter emitter = new Emitter(entity, "arrow", true, 3000, 50, new Vector2(0, -10), 0, 0, 8, 0.0);
				emitter.ParticleDrawable.AddAnimation(0, 32, 1, 30);
				entity.Components.Add(emitter);
				Weapon weapon = new Weapon(entity, "Grenade", new Vector2(0.8f, 0), new Vector2(0.8f, 0), 0.1, 4.0f, 20.0f, 6.0f, true);
				weapon.WeaponDrawable = new Drawable("fragNade", new Vector2(16), 32, 32, 0.2f);
				weapon.ArmDrawable = new Drawable("tempArm", new Vector2(3,7), 32, 32, 0.1f);
				entity.Components.Add(weapon);

				for (int i = 0; i < 101; i++)
				{
					for (int j = 0; j < 101; j++)
					{
						if (i < 2 || i > 98 || j < 2 || j > 98)
						{
							Entity e = new Entity(true, "Block");
							e.Position = new Vector2(i, j);
							terrain.Add(e);
							Renderable r = new Renderable(e, "Block", "TestSheet", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
							AABBCollider c = new AABBCollider(e, new Vector2(0));
							r.Drawable.AddAnimation(0, 0, 1, 30);
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
							Renderable r = new Renderable(e, "Block", "TestSheet", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
							AABBCollider c = new AABBCollider(e, new Vector2(0));
							r.Drawable.AddAnimation(0, 0, 1, 30);
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
							Renderable r = new Renderable(e, "Block", "TestSheet", new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
							AABBCollider c = new AABBCollider(e, new Vector2(0));
							r.Drawable.AddAnimation(0, 0, 1, 30);
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
				if (e.GetComponent<Character>() != null)
					cam.SetSubject(e);
			}

			levelEditor.Load(Content);
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
			//terrain[terrain.Count - 1].Load(Content);
			//cam.Trauma = 1;
			cam.Update(gameTime.ElapsedGameTime.TotalSeconds);

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
				spriteBatch.End();
			}

			base.Draw(gameTime);
		}
	}
}

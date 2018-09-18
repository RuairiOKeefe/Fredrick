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
		List<Entity> entities = new List<Entity>();
		FollowCamera cam;
		Effect lighting;
		Effect bloom;

		RenderTarget2D sceneTarget;
		RenderTarget2D bloomTarget;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 1600;
			graphics.PreferredBackBufferHeight = 900;
			this.IsMouseVisible = true;
			Content.RootDirectory = "Content";
			//graphics.IsFullScreen = true;


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

			Texture2D testSheet = Content.Load<Texture2D>("TestSheet");//This texture includes a colour that matches the key colour, not important since its a test sprite but funny none the less
			Texture2D tempSlope = Content.Load<Texture2D>("tempSlope");
			Texture2D tempParticle = Content.Load<Texture2D>("arrow");
			Texture2D grenade = Content.Load<Texture2D>("fragNade");

			ColliderManager.Instance.Load();
			ProjectileBuffer.Instance.Load(grenade);

			Entity entity = new Entity();
			entity.SetPosition(new Vector2(8, 8));
			entities.Add(entity);
			Renderable renderable = new Renderable(entity, testSheet, new Vector2(8, 16), new Vector2(0), new Vector2(2), 16, 32, 0.1f);
			Character character = new Character(entity);
			AABBCollider boxCollider = new AABBCollider(entity, new Vector2(0), 1.0f, 1.9f);
			renderable.Drawable.AddAnimation(0, 0, 32, 1, 30);
			renderable.Drawable.AddAnimation(1, 0, 32, 4, 30);
			entity.Components.Add(renderable);
			entity.Components.Add(character);
			entity.Components.Add(boxCollider);
			Emitter emitter = new Emitter(entity, tempParticle, true, 3000, 50, new Vector2(0, -10), 0, 0, 8, 1.5);
			emitter.ParticleDrawable.AddAnimation(0, 0, 32, 1, 30);
			entity.Components.Add(emitter);
			Weapon weapon = new Weapon(entity);
			weapon._d = new Drawable(tempParticle);
			entity.Components.Add(weapon);

			cam.SetSubject(entity);
			for (int i = 0; i < 101; i++)
			{
				for (int j = 0; j < 101; j++)
				{
					if (i < 2 || i > 98 || j < 2 || j > 98)
					{
						Entity e = new Entity();
						e.SetPosition(new Vector2(i, j));
						entities.Add(e);
						Renderable r = new Renderable(e, testSheet, new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
						AABBCollider c = new AABBCollider(e, new Vector2(0));
						r.Drawable.AddAnimation(0, 0, 0, 1, 30);
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
						Entity e = new Entity();
						e.SetPosition(new Vector2(i, j));
						entities.Add(e);
						Renderable r = new Renderable(e, testSheet, new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
						AABBCollider c = new AABBCollider(e, new Vector2(0));
						r.Drawable.AddAnimation(0, 0, 0, 1, 30);
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
						Entity e = new Entity();
						e.SetPosition(new Vector2(i + 3, j + 2));
						entities.Add(e);
						Renderable r = new Renderable(e, testSheet, new Vector2(16), new Vector2(0), new Vector2(1), 32, 32, 0.1f);
						AABBCollider c = new AABBCollider(e, new Vector2(0));
						r.Drawable.AddAnimation(0, 0, 0, 1, 30);
						e.Components.Add(r);
						e.Components.Add(c);
					}
				}
			}
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

			foreach (var e in entities)
				e.Update(gameTime.ElapsedGameTime.TotalSeconds);

			ColliderManager.Instance.Update(gameTime.ElapsedGameTime.TotalSeconds);

			foreach (var e in entities)
			{
				if (e.GetComponent<Weapon>() != null)
					e.GetComponent<Weapon>().UpdateProjectilePos();
			}

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
			p.Draw(spriteBatch, GraphicsDevice, entities);
			//aaaaaaaaaaaaaaaaaa

			GraphicsDevice.SetRenderTarget(sceneTarget);

			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, lighting, cam.Get_Transformation(GraphicsDevice));
			foreach (var e in entities)
				e.Draw(spriteBatch);
			spriteBatch.End();

			GraphicsDevice.SetRenderTarget(null);

			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
			spriteBatch.Draw(sceneTarget, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
			spriteBatch.End();

			if (DebugManager.Instance.Debug)
			{
				spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, cam.Get_Transformation(GraphicsDevice));
				foreach (var e in entities)
					e.DebugDraw(spriteBatch);
				spriteBatch.End();
			}

			base.Draw(gameTime);
		}
	}
}

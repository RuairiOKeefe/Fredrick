﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
		Camera cam;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 1600;
			graphics.PreferredBackBufferHeight = 900;
			Content.RootDirectory = "Content";
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

			cam = new Camera(1600, 900);
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Texture2D testSheet = Content.Load<Texture2D>("TestSheet");//This texture includes a colour that matches the key colour, not important since its a test sprite but funny none the less
			Entity entity = new Entity();
			entities.Add(entity);
			Renderable renderable = new Renderable(entity, testSheet);
			Character character = new Character(entity);
			AABBCollider boxCollider = new AABBCollider(entity);
			renderable.AddAnimation(0, 0, 32, 1, 30);
			renderable.AddAnimation(1, 0, 32, 4, 30);
			entity.Components.Add(renderable);
			entity.Components.Add(character);
			entity.Components.Add(boxCollider);
			for (int i = -20; i < 21; i++)
			{
				Entity e = new Entity();
				if (i == -20 || i == 20)
					e.SetPosition(new Vector2(i, -1));
				else
					e.SetPosition(new Vector2(i, -2));
				entities.Add(e);
				Renderable r = new Renderable(e, testSheet);
				AABBCollider c = new AABBCollider(e);
				r.AddAnimation(0, 0, 0, 1, 30);
				entity.Components.Add(r);
				entity.Components.Add(c);
			}

			for (int i = 0; i < 20; i++)
			{
				Entity e = new Entity();
				e.SetPosition(new Vector2(-10+i, -5));
				if (i<6)
				{
					e.SetPosition(new Vector2(-10 + i, -1 + i));
					Platform p = new Platform(e, new Vector2(0), 1, 1, 0, 0, -0.5f, 0.5f, 0.2f);
					entity.Components.Add(p);
				}
				else
					if (i>13)
				{
					e.SetPosition(new Vector2(-10 + i, 18 - i));
					Platform p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, -0.5f, 0.2f);
					entity.Components.Add(p);
				}
				else
				{
					e.SetPosition(new Vector2(-10 + i, 4));
					Platform p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, 0.5f, 0.2f);
					entity.Components.Add(p);
				}
				Renderable r = new Renderable(e, testSheet);
				r.AddAnimation(0, 0, 0, 1, 30);
				entity.Components.Add(r);
			}
			// TODO: use this.Content to load your game content here
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
			InputHandler.Instance.Update();
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			foreach (var e in entities)
				e.Update(gameTime.ElapsedGameTime.TotalSeconds);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, cam.get_transformation(GraphicsDevice));
			foreach (var e in entities)
				e.Draw(spriteBatch);
			spriteBatch.End();


			base.Draw(gameTime);
		}
	}
}

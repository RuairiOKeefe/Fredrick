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
		List<Entity> entities = new List<Entity>();
		FollowCamera cam;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 1600;
			graphics.PreferredBackBufferHeight = 900;
			this.IsMouseVisible = true;
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

			cam = new FollowCamera(1600, 900);
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			// create 1x1 texture for line drawing
			DebugManager.Instance.LineTex = new Texture2D(GraphicsDevice, 1, 1);
			DebugManager.Instance.LineTex.SetData<Color>(new Color[] { Color.White });// fill the texture with white

			

			Texture2D testSheet = Content.Load<Texture2D>("TestSheet");//This texture includes a colour that matches the key colour, not important since its a test sprite but funny none the less
			Texture2D tempSlope = Content.Load<Texture2D>("tempSlope");
			Texture2D tempParticle = Content.Load<Texture2D>("arrow");
			Texture2D grenade = Content.Load<Texture2D>("fragNade");

			ColliderManager.Instance.Load();
			ProjectileBuffer.Instance.Load(grenade);

			Entity entity = new Entity();
			entity.SetPosition(new Vector2(8, 5));
			entities.Add(entity);
			Renderable renderable = new Renderable(entity, testSheet);
			Character character = new Character(entity);
			AABBCollider boxCollider = new AABBCollider(entity);
			renderable.Drawable.AddAnimation(0, 0, 32, 1, 30);
			renderable.Drawable.AddAnimation(1, 0, 32, 4, 30);
			entity.Components.Add(renderable);
			entity.Components.Add(character);
			entity.Components.Add(boxCollider);
			Emitter emitter = new Emitter(entity, tempParticle);
			emitter.ParticleDrawable.AddAnimation(0, 0, 32, 1, 30);
			entity.Components.Add(emitter);
			Weapon weapon = new Weapon(entity);
			weapon._d = new Drawable(tempParticle);
			entity.Components.Add(weapon);

			cam.SetSubject(entity);
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
				r.Drawable.AddAnimation(0, 0, 0, 1, 30);
				entity.Components.Add(r);
				entity.Components.Add(c);
			}

			for (int i = -1; i < 5; i++)
			{
				for (int j = i; j < 21 - i; j++)
				{
					Entity e = new Entity();
					e.SetPosition(new Vector2(-10 + j, i));
					Renderable r = new Renderable(e, testSheet);
					if (j < i+1)
					{
						Platform p = new Platform(e, new Vector2(0), 1, 1, 0, 0, -0.5f, 0.5f, -1.0f);
						r = new Renderable(e, tempSlope);
						r.Drawable.AddAnimation(0, 0, 0, 1, 30);
						entity.Components.Add(p);
					}
					else
						if (j > 19 - i)
					{
						Platform p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, -0.5f, -1.0f);
						r = new Renderable(e, tempSlope);
						r.Drawable.AddAnimation(0, 32, 0, 1, 30);
						entity.Components.Add(p);
					}
					else
					{
						if (i < 4)
						{
							r.Drawable.AddAnimation(0, 0, 0, 1, 30);
						}

						else
						{
							Platform p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, 0.5f, -1.0f);
							r.Drawable.AddAnimation(0, 0, 0, 1, 30);
							entity.Components.Add(p);
						}
					}
					entity.Components.Add(r);
				}
			}

			for (int i = 7; i < 12; i++)
			{
				for (int j = 3-i; j < -2 + i; j++)
				{
					Entity e = new Entity();
					e.SetPosition(new Vector2(j, i));
					Renderable r = new Renderable(e, testSheet);
					if (j < 4-i)
					{
						Platform p = new Platform(e, new Vector2(0), 1, 1, 0, 0, 0.5f, -0.5f, 1.0f);
						r = new Renderable(e, tempSlope);
						r.Drawable.AddAnimation(0, 64, 0, 1, 30);
						entity.Components.Add(p);
					}
					else
						if (j > -4 + i)
					{
						Platform p = new Platform(e, new Vector2(0), 1, 1, 0, 0, -0.5f, 0.5f, 1.0f);
						r = new Renderable(e, tempSlope);
						r.Drawable.AddAnimation(0, 96, 0, 1, 30);
						entity.Components.Add(p);
					}
					else
					{
						if (i > 8)
						{
							r.Drawable.AddAnimation(0, 0, 0, 1, 30);
						}

						else
						{
							Platform p = new Platform(e, new Vector2(0), 1, 1, 0, 0, -0.5f, -0.5f, 1.0f);
							r.Drawable.AddAnimation(0, 0, 0, 1, 30);
							entity.Components.Add(p);
						}
					}
					entity.Components.Add(r);
				}
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
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, cam.Get_Transformation(GraphicsDevice));
			foreach (var e in entities)
				e.Draw(spriteBatch);
			spriteBatch.End();


			base.Draw(gameTime);
		}
	}
}

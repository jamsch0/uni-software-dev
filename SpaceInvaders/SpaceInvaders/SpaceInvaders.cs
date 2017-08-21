using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders {

	enum GameState {
		START, MAIN, PAUSE, END
	}

	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class SpaceInvaders : Game {

		#region Constants

		public const int SCREEN_SIZE = 500;
		public const int SCREEN_BORDER_SIZE = 10;

		const int STARTING_LIVES = 3;
		const int MAX_LIVES = 8;

		const int DEFENCE_1_POSITION_X = 61;
		const int DEFENCE_2_POSITION_X = 171;
		const int DEFENCE_3_POSITION_X = 281;
		const int DEFENCE_4_POSITION_X = 391;
		const int DEFENCE_POSITION_Y = SCREEN_SIZE - 124;

		const int FLASHING_TEXT_TICKS = 50;
		const int GAME_OVER_TEXT_TICKS = 100;
		const int PLAYER_DESTRUCTION_TICKS = 40;
		const int UFO_DESTRUCTION_TICKS = 100;

		const int SPACE_TEXT_OFFSET_Y = 40;
		const int INVADERS_TEXT_OFFSET_Y = 70;
		const int INVADER_OFFSET_X = 60;
		const int INVADER_1_OFFSET_Y = 140;
		const int INVADER_2_OFFSET_Y = 170;
		const int INVADER_3_OFFSET_Y = 200;
		const int INVADER_1_SCORE_TEXT_OFFSET_Y = 150;
		const int INVADER_2_SCORE_TEXT_OFFSET_Y = 180;
		const int INVADER_3_SCORE_TEXT_OFFSET_Y = 210;
		const int INVADER_SCORE_TEXT_OFFSET_X = 20;
		const int UFO_OFFSET_X = 62;
		const int UFO_OFFSET_Y = 237;
		const int UFO_SCORE_TEXT_OFFSET_X = 20;
		const int UFO_SCORE_TEXT_OFFSET_Y = 240;
		const int MOVE_CONTROLS_TEXT_OFFSET_Y = 200;
		const int SHOOT_CONTROLS_TEXT_OFFSET_Y = 170;
		const int PAUSE_CONTROLS_TEXT_OFFSET_Y = 140;
		const int START_TEXT_OFFSET_Y = 80;
		const int COPYRIGHT_TEXT_OFFSET_Y = 15;
		const int SCORE_TEXT_OFFSET_X = SCREEN_BORDER_SIZE;
		const int SCORE_TEXT_OFFSET_Y = SCREEN_BORDER_SIZE;
		const int SCORE_OFFSET_X = 100;
		const int LIVES_TEXT_OFFSET_X = 250;
		const int LIVES_TEXT_OFFSET_Y = SCREEN_BORDER_SIZE;
		const int LIVES_OFFSET_X = 160;
		const int LIVES_SEPARATION_X = 40;
		const int LIVES_SEPARATION_Y = 22;

		const float START_SCREEN_UFO_SCALE = 0.75F;

		const string SPACE_TEXT = "SPACE";
		const string INVADERS_TEXT = "INVADERS";
		const string INVADER_SCORE_TEXT = "= {0} PTS";
		const string UFO_SCORE_TEXT = "= ??? PTS";
		const string MOVE_CONTROLS_TEXT = "[LEFT / RIGHT] TO MOVE";
		const string SHOOT_CONTROLS_TEXT = "[SPACE] TO SHOOT";
		const string PAUSE_CONTROLS_TEXT = "[ESC] TO PAUSE";
		const string START_TEXT = "PRESS [SPACE] TO START";
		const string COPYRIGHT_TEXT = "COPYRIGHT (C) 2015 JAMES CHAPMAN";
		const string SCORE_TEXT = "SCORE";
		const string LIVES_TEXT = "LIVES";
		const string PAUSED_TEXT = "PAUSED";
		const string GAME_OVER_TEXT = "GAME OVER";

		#endregion

		#region Fields

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		SpriteFont fontRegular8pt;
		SpriteFont fontRegular10pt;
		SpriteFont fontRegular12pt;
		SpriteFont fontBold18pt;

		Texture2D launcher;
		Texture2D invader1;
		Texture2D invader2;
		Texture2D invader3;
		Texture2D projectile;
		Texture2D ufoTexture;
		Texture2D defenceFull;
		Texture2D defenceInsideLeft;
		Texture2D defenceInsideRight;
		Texture2D defenceOutsideLeft;
		Texture2D defenceOutsideRight;
		Texture2D dummyTexture;

		SoundEffectInstance shootSound;
		SoundEffectInstance explosionSound;
		SoundEffectInstance invaderKilledSound;
		SoundEffectInstance invaderMoveSound1;
		SoundEffectInstance invaderMoveSound2;
		SoundEffectInstance invaderMoveSound3;
		SoundEffectInstance invaderMoveSound4;
		SoundEffectInstance ufoSound;

		GameState gameState;
		KeyboardState oldKeyboard;

		Player player;
		InvaderWave invaderWave;
		Defence[] defences;
		Ufo ufo;

		float scale;
		int ticks;
		int score;
		int lives;
		
		#endregion

		#region Properties

		public Rectangle ActiveWindowArea {
			get {
				int width = graphics.GraphicsDevice.Viewport.Width;
				int height = graphics.GraphicsDevice.Viewport.Height;
				int size = Math.Min(width, height);

				return new Rectangle((width - size) / 2, (height - size) / 2, size, size);
			}
		}

		#endregion

		public SpaceInvaders(int screenWidth = SCREEN_SIZE, int screenHeight = SCREEN_SIZE) {
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = screenWidth;
			graphics.PreferredBackBufferHeight = screenHeight;
			
			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);
			
			fontRegular8pt = Content.Load<SpriteFont>("font_regular8pt");
			fontRegular10pt = Content.Load<SpriteFont>("font_regular10pt");
			fontRegular12pt = Content.Load<SpriteFont>("font_regular12pt");
			fontBold18pt = Content.Load<SpriteFont>("font_bold18pt");

			launcher = Content.Load<Texture2D>("launcher");
			invader1 = Content.Load<Texture2D>("invader1");
			invader2 = Content.Load<Texture2D>("invader2");
			invader3 = Content.Load<Texture2D>("invader3");
			projectile = Content.Load<Texture2D>("projectile");
			ufoTexture = Content.Load<Texture2D>("ufo");
			defenceFull = Content.Load<Texture2D>("defenceFullBlock");
			defenceInsideLeft = Content.Load<Texture2D>("defenceInsideLeftBlock");
			defenceInsideRight = Content.Load<Texture2D>("defenceInsideRightBlock");
			defenceOutsideLeft = Content.Load<Texture2D>("defenceOutsideLeftBlock");
			defenceOutsideRight = Content.Load<Texture2D>("defenceOutsideRightBlock");

			// DummyTexture is a single white pixel, only required to draw rectangle borders outside ActiveWindowArea
			dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
			dummyTexture.SetData(new Color[] { Color.White });

			shootSound = Content.Load<SoundEffect>("shoot").CreateInstance();
			explosionSound = Content.Load<SoundEffect>("explosion").CreateInstance();
			invaderKilledSound = Content.Load<SoundEffect>("invaderKilled").CreateInstance();
			invaderMoveSound1 = Content.Load<SoundEffect>("invaderMove1").CreateInstance();
			invaderMoveSound2 = Content.Load<SoundEffect>("invaderMove2").CreateInstance();
			invaderMoveSound3 = Content.Load<SoundEffect>("invaderMove3").CreateInstance();
			invaderMoveSound4 = Content.Load<SoundEffect>("invaderMove4").CreateInstance();
			ufoSound = Content.Load<SoundEffect>("ufoMove").CreateInstance();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent() {
			Content.Unload();
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content. Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
			base.Initialize();

			oldKeyboard = Keyboard.GetState();
			scale = ActiveWindowArea.Width / (float) SCREEN_SIZE;

			player = new Player(launcher, projectile, scale, shootSound, explosionSound);
			invaderWave = new InvaderWave(invader1, invader2, invader3, projectile, scale, new SoundEffectInstance[] { invaderMoveSound1, invaderMoveSound2, invaderMoveSound3, invaderMoveSound4 });
			ufo = new Ufo(ufoTexture, scale, ufoSound);
			
			defences = new Defence[] {
				new Defence(defenceFull, defenceInsideLeft, defenceInsideRight, defenceOutsideLeft, defenceOutsideRight, scale, new Vector2(DEFENCE_1_POSITION_X, DEFENCE_POSITION_Y)),
				new Defence(defenceFull, defenceInsideLeft, defenceInsideRight, defenceOutsideLeft, defenceOutsideRight, scale, new Vector2(DEFENCE_2_POSITION_X, DEFENCE_POSITION_Y)),
				new Defence(defenceFull, defenceInsideLeft, defenceInsideRight, defenceOutsideLeft, defenceOutsideRight, scale, new Vector2(DEFENCE_3_POSITION_X, DEFENCE_POSITION_Y)),
				new Defence(defenceFull, defenceInsideLeft, defenceInsideRight, defenceOutsideLeft, defenceOutsideRight, scale, new Vector2(DEFENCE_4_POSITION_X, DEFENCE_POSITION_Y))
			};
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			// Clear the screen with a black background
            GraphicsDevice.Clear(Color.Black);

            // Initiates the spriteBatch, using default values except for the SamplerState which defaults to LinearClamp.
            // PointClamp prevents the textures from blurring when being scaled.
			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);

			Rectangle activeWindowArea = ActiveWindowArea;

			Vector2 drawPosition = Vector2.Zero;
			Rectangle frame;

			if (gameState == GameState.START) {
				// Draw SPACE INVADERS title text
				drawPosition.Y = activeWindowArea.Top + (SPACE_TEXT_OFFSET_Y * scale);
				Util.DrawStringCentered(spriteBatch, fontBold18pt, SPACE_TEXT, drawPosition.Y, scale, activeWindowArea);

				drawPosition.Y = activeWindowArea.Top + (INVADERS_TEXT_OFFSET_Y * scale);
				Util.DrawStringCentered(spriteBatch, fontBold18pt, INVADERS_TEXT, drawPosition.Y, scale, activeWindowArea);

				// Draw Invader/UFO icons and scores
				frame = new Rectangle(invader1.Width / Invader.FRAMES, 0, invader1.Width / Invader.FRAMES, invader1.Height);

				drawPosition.X = activeWindowArea.Center.X - (INVADER_OFFSET_X * scale);
				drawPosition.Y = activeWindowArea.Top + (INVADER_3_OFFSET_Y * scale);
				Util.Draw(spriteBatch, invader3, drawPosition, frame, scale);

				drawPosition.X = activeWindowArea.Center.X - (INVADER_SCORE_TEXT_OFFSET_X * scale);
				drawPosition.Y = activeWindowArea.Top + (INVADER_3_SCORE_TEXT_OFFSET_Y * scale);
				Util.DrawString(spriteBatch, fontRegular10pt, string.Format(INVADER_SCORE_TEXT, InvaderWave.INVADER_3_SCORE), drawPosition, scale);

				drawPosition.X = activeWindowArea.Center.X - (INVADER_OFFSET_X * scale);
				drawPosition.Y = activeWindowArea.Top + (INVADER_2_OFFSET_Y * scale);
				Util.Draw(spriteBatch, invader2, drawPosition, frame, scale);

				drawPosition.X = activeWindowArea.Center.X - (INVADER_SCORE_TEXT_OFFSET_X * scale);
				drawPosition.Y = activeWindowArea.Top + (INVADER_2_SCORE_TEXT_OFFSET_Y * scale);
				Util.DrawString(spriteBatch, fontRegular10pt, string.Format(INVADER_SCORE_TEXT, InvaderWave.INVADER_2_SCORE), drawPosition, scale);

				drawPosition.X = activeWindowArea.Center.X - (INVADER_OFFSET_X * scale);
				drawPosition.Y = activeWindowArea.Top + (INVADER_1_OFFSET_Y * scale);
				Util.Draw(spriteBatch, invader1, drawPosition, frame, scale);

				drawPosition.X = activeWindowArea.Center.X - (INVADER_SCORE_TEXT_OFFSET_X * scale);
				drawPosition.Y = activeWindowArea.Top + (INVADER_1_SCORE_TEXT_OFFSET_Y * scale);
				Util.DrawString(spriteBatch, fontRegular10pt, string.Format(INVADER_SCORE_TEXT, InvaderWave.INVADER_1_SCORE), drawPosition, scale);

				frame = new Rectangle(0, 0, ufoTexture.Width, ufoTexture.Height);
				drawPosition.X = activeWindowArea.Center.X - (UFO_OFFSET_X * scale);
				drawPosition.Y = activeWindowArea.Top + (UFO_OFFSET_Y * scale);
				Util.Draw(spriteBatch, ufoTexture, drawPosition, frame, scale * START_SCREEN_UFO_SCALE);

				drawPosition.X = activeWindowArea.Center.X - (UFO_SCORE_TEXT_OFFSET_X * scale);
				drawPosition.Y = activeWindowArea.Top + (UFO_SCORE_TEXT_OFFSET_Y * scale);
				Util.DrawString(spriteBatch, fontRegular10pt, UFO_SCORE_TEXT, drawPosition, scale);

				// Draw controls
				drawPosition.Y = activeWindowArea.Bottom - (MOVE_CONTROLS_TEXT_OFFSET_Y * scale);
				Util.DrawStringCentered(spriteBatch, fontRegular10pt, MOVE_CONTROLS_TEXT, drawPosition.Y, scale, activeWindowArea);

				drawPosition.Y = activeWindowArea.Bottom - (SHOOT_CONTROLS_TEXT_OFFSET_Y * scale);
				Util.DrawStringCentered(spriteBatch, fontRegular10pt, SHOOT_CONTROLS_TEXT, drawPosition.Y, scale, activeWindowArea);

				drawPosition.Y = activeWindowArea.Bottom - (PAUSE_CONTROLS_TEXT_OFFSET_Y * scale);
				Util.DrawStringCentered(spriteBatch, fontRegular10pt, PAUSE_CONTROLS_TEXT, drawPosition.Y, scale, activeWindowArea);

				// Draw flashing START text
				if (ticks < FLASHING_TEXT_TICKS / 2) {
					drawPosition.Y = activeWindowArea.Bottom - (START_TEXT_OFFSET_Y * scale);
					Util.DrawStringCentered(spriteBatch, fontRegular12pt, START_TEXT, drawPosition.Y, scale, activeWindowArea);
				}

				// Draw Copyright text
				drawPosition.Y = activeWindowArea.Bottom - (COPYRIGHT_TEXT_OFFSET_Y * scale);
				Util.DrawStringCentered(spriteBatch, fontRegular8pt, COPYRIGHT_TEXT, drawPosition.Y, scale, activeWindowArea);
			} else {
				// Draw SCORE text
				drawPosition.X = activeWindowArea.Left + (SCORE_TEXT_OFFSET_X * scale);
				drawPosition.Y = activeWindowArea.Top + (SCORE_TEXT_OFFSET_Y * scale);
				Util.DrawString(spriteBatch, fontRegular12pt, SCORE_TEXT, drawPosition, scale);

				// Draw score count
				drawPosition.X = activeWindowArea.Left + (SCORE_OFFSET_X * scale);
				Util.DrawString(spriteBatch, fontRegular12pt, score.ToString(), drawPosition, Color.Lime, scale);

				// Draw LIVES text
				drawPosition.X = activeWindowArea.Right - (LIVES_TEXT_OFFSET_X * scale);
				drawPosition.Y = activeWindowArea.Top + (LIVES_TEXT_OFFSET_Y * scale);
				Util.DrawString(spriteBatch, fontRegular12pt, LIVES_TEXT, drawPosition, scale);

				// Draw player launcher sprites as life count
				frame = new Rectangle(0, 0, launcher.Width / Player.FRAMES, launcher.Height);
				int x, y;

				for (int i = 0; i < lives; i++) {
					// Draw lives in 2 rows
                    x = i % (MAX_LIVES / 2);
					y = i / (MAX_LIVES / 2);

					drawPosition.X = activeWindowArea.Right - ((LIVES_OFFSET_X - (x * LIVES_SEPARATION_X)) * scale);
					drawPosition.Y = activeWindowArea.Top + ((LIVES_TEXT_OFFSET_Y + (y * LIVES_SEPARATION_Y)) * scale);

					Util.Draw(spriteBatch, launcher, drawPosition, frame, scale);
				}

				// Draw player launcher, ufo and defences
				player.Draw(spriteBatch, activeWindowArea);
				ufo.Draw(spriteBatch, activeWindowArea);

				foreach (Defence defence in defences)
					defence.Draw(spriteBatch, activeWindowArea);

				Vector2 textSize = Vector2.Zero;

				// Invaders aren't drawn if game is paused or over so they aren't in the way of the text
				switch (gameState) {
					case GameState.MAIN:
						// Draw invaders
						invaderWave.Draw(spriteBatch, activeWindowArea);

						// Draw flashing UFO score if UFO has been shot
						if (ufo.Destroyed && ((ticks > 0 && ticks < UFO_DESTRUCTION_TICKS * 0.15F) || (ticks >= UFO_DESTRUCTION_TICKS * 0.30F && ticks < UFO_DESTRUCTION_TICKS * 0.45F) || ticks > UFO_DESTRUCTION_TICKS * 0.60F)) {
							string ufoScore = ufo.LastScore.ToString();
							textSize = fontRegular12pt.MeasureString(ufoScore);

							drawPosition.X = (ufo.Position.X + ((ufo.Width / 2) - (textSize.X / 2))) * scale;
							drawPosition.Y = (ufo.Position.Y + ((ufo.Height / 2) - (textSize.Y / 2))) * scale;

							// Prevent score from being drawn outside the screen
							if (drawPosition.X < activeWindowArea.Left)
								drawPosition.X = activeWindowArea.Left;
							else if (drawPosition.X > activeWindowArea.Right - textSize.X)
								drawPosition.X = activeWindowArea.Right - textSize.X;

							Util.DrawString(spriteBatch, fontRegular12pt, ufoScore, drawPosition, scale);
						}

						break;
					case GameState.PAUSE:
						// Draw flashing PAUSED text
						if (ticks < FLASHING_TEXT_TICKS / 2)
							Util.DrawStringCentered(spriteBatch, fontBold18pt, PAUSED_TEXT, scale, activeWindowArea);

						break;
					case GameState.END:
						// Draw GAME OVER text, appearing one letter at a time
						string text = GAME_OVER_TEXT.Substring(0, (int) MathHelper.Lerp(1, GAME_OVER_TEXT.Length, ticks / (float) GAME_OVER_TEXT_TICKS));
						Util.DrawStringCentered(spriteBatch, fontBold18pt, text, scale, activeWindowArea);

						break;
				}
			}

			// If game window is not square, draw black borders outside the activeWindowArea
			// This hides the ufo, projectiles and other objects which are supposed to be outside the screen
			// ActiveWindowArea is always the largest square possible in the game window, so only left/right or top/bottom borders or none need to be drawn, never both
			if (activeWindowArea.X > 0) {
				// Left/Right borders
				spriteBatch.Draw(dummyTexture, new Rectangle(0, 0, activeWindowArea.Left, graphics.GraphicsDevice.Viewport.Height), Color.Black);
				spriteBatch.Draw(dummyTexture, new Rectangle(activeWindowArea.Right, 0, activeWindowArea.Left, activeWindowArea.Height), Color.Black);
			} else if (activeWindowArea.Y > 0) {
				// Top/Bottom borders
				spriteBatch.Draw(dummyTexture, new Rectangle(activeWindowArea.Left, 0, activeWindowArea.Width, activeWindowArea.Top), Color.Black);
				spriteBatch.Draw(dummyTexture, new Rectangle(activeWindowArea.Left, activeWindowArea.Bottom, activeWindowArea.Width, activeWindowArea.Top), Color.Black);
			}

			spriteBatch.End();
			base.Draw(gameTime);
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {
			KeyboardState keyboard = Keyboard.GetState();
			Rectangle activeWindowArea = ActiveWindowArea;

			switch (gameState) {
				case GameState.START:
					// Start game when Space key pressed
					if (keyboard.IsKeyDown(Keys.Space) && oldKeyboard.IsKeyUp(Keys.Space)) {
						gameState = GameState.MAIN;
						lives = STARTING_LIVES;
						score = 0;
						ticks = 0;
					}

					// Handle flashing text timing
					if (ticks > FLASHING_TEXT_TICKS)
						ticks = 0;
					else
						ticks++;

					break;
				case GameState.MAIN:
					if (keyboard.IsKeyDown(Keys.Escape) && oldKeyboard.IsKeyUp(Keys.Escape)) {
						// Pause game when Escape key is pressed
						gameState = GameState.PAUSE;
						ticks = 0;
						break;
					} else if (invaderWave.Count == 0) {
						// If all invaders are destroyed, start a new wave
						invaderWave.Reset();
						player.Reset();

						if (lives < MAX_LIVES)
							lives++;

						break;
					} else if (lives == 0 || invaderWave.Position.Y + invaderWave.Height >= player.Position.Y) {
						// Game over if wave reaches player or player runs out of lives
						gameState = GameState.END;
						ticks = 0;
						break;
					} else if (player.Destroyed) {
						// Handle player destruction pause timing
						if (ticks == 0) {
							ticks++;
							player.Projectile.Visible = false;

							foreach (Projectile projectile in invaderWave.Projectiles)
								projectile.Visible = false;
						} else if (ticks > PLAYER_DESTRUCTION_TICKS) {
							player.Reset();
							ticks = 0;
						} else
							ticks++;

						break;
					} else if (ufo.Destroyed) {
						// Handle ufo destruction timing
						if (ticks > UFO_DESTRUCTION_TICKS) {
							ufo.Reset();
							ticks = 0;
						} else
							ticks++;
					}

					// Update game objects
					player.Update(gameTime, oldKeyboard);
					invaderWave.Update(gameTime);
					ufo.Update(gameTime);

					// Do collision checks between defences and player/invader projectiles
					foreach (Defence defence in defences) {
						defence.CheckCollision(player.Projectile);

						foreach (Projectile projectile in invaderWave.Projectiles)
							defence.CheckCollision(projectile);
					}

					// Do collision checks between player/player projectile and invader projectiles
					foreach (Projectile projectile in invaderWave.Projectiles) {
						if (player.CheckCollision(projectile)) {
							ufo.Reset();
							lives--;
						}

						player.Projectile.CheckCollision(projectile);
					}

					// Do collision checks between invaders/ufo and player projectile
					if (invaderWave.CheckCollision(player.Projectile)) {
						score += invaderWave.LastScore;
						invaderKilledSound.Play();
					}
					
					score += ufo.CheckCollision(player.Projectile);

					break;
				case GameState.PAUSE:
					// Resume game when Escape key is pressed
					if (keyboard.IsKeyDown(Keys.Escape) && oldKeyboard.IsKeyUp(Keys.Escape)) {
						gameState = GameState.MAIN;
						ticks = 0;
					}

					// Handle flashing text timing
					if (ticks > FLASHING_TEXT_TICKS)
						ticks = 0;
					else
						ticks++;

					break;
				case GameState.END:
					// Return to start menu when Escape key is pressed
					if (keyboard.IsKeyDown(Keys.Escape) && oldKeyboard.IsKeyUp(Keys.Escape)) {
						gameState = GameState.START;
						ticks = 0;

						invaderWave.Reset();
						player.Reset();
						
						foreach (Defence defence in defences)
							defence.Reset();
					}

					// Handle game over text timing
					if (ticks < 100)
						ticks++;

					break;
			}

			oldKeyboard = keyboard;
			base.Update(gameTime);
		}

		void Window_ClientSizeChanged(object sender, EventArgs e) {
			scale = ActiveWindowArea.Width / (float) SCREEN_SIZE;

			player.Scale = scale;
			ufo.Scale = scale;
			invaderWave.Scale = scale;

			foreach (Defence defence in defences)
				defence.Scale = scale;
		}
	}
}

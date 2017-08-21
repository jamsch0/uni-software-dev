using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders {
	
	public class InvaderWave {

		#region Constants

		public const int INVADER_1_SCORE = 40;
		public const int INVADER_2_SCORE = 20;
		public const int INVADER_3_SCORE = 10;

		const int ROWS = 5;
		const int COLUMNS = 11;
		const int MOVE_AMOUNT_X = 4;
		const int MOVE_AMOUNT_Y = 14;
		const int START_POSITION_Y = 60;
		const int TICK_RATE_MIN = 30;
		const int TICK_RATE_MAX = 5;

		readonly int INVADER_WIDTH;

		#endregion

		#region Fields

		Texture2D invader1;
		Texture2D invader2;
		Texture2D invader3;
		Texture2D projectile;

		SoundEffectInstance[] moveSounds;

		Vector2 position;
		Invader[,] invaders;
		Projectile[] projectiles;

		int count;
		float scale;
		int ticks;
		int direction;
		int nextDirection;
		int leftMostInvader;
		int rightMostInvader;
		int bottomMostInvader;
		int lastScore;
		int soundPointer;

		#endregion

		#region Properties

		public int Count {
			get { return count; }
		}

		public float Scale {
			get { return scale; }
			set {
				scale = value;

				foreach (Invader invader in invaders)
						invader.Scale = scale;
			}
		}

		public int Width {
			get {
				return ((rightMostInvader - leftMostInvader) + 1) * INVADER_WIDTH;
			}
		}

		public int Height {
			get {
				return (bottomMostInvader + 1) * INVADER_WIDTH;
			}
		}

		public Vector2 Position {
			get { return position; }
		}

		public Projectile[] Projectiles {
			get { return projectiles; }
		}

		public int LastScore {
			get { return lastScore; }
		}

		#endregion

		public InvaderWave(Texture2D invader1, Texture2D invader2, Texture2D invader3, Texture2D projectile, float scale, SoundEffectInstance[] moveSounds) {
			this.invader1 = invader1;
			this.invader2 = invader2;
			this.invader3 = invader3;
			this.projectile = projectile;
			this.scale = scale;
			this.moveSounds = moveSounds;
			
			INVADER_WIDTH = invader1.Width / Invader.FRAMES;

			leftMostInvader = 0;
			rightMostInvader = COLUMNS - 1;
			bottomMostInvader = ROWS - 1;

			soundPointer = 0;
			count = COLUMNS * ROWS;
			direction = nextDirection = 1;
			position = new Vector2((SpaceInvaders.SCREEN_SIZE / 2) - (Width / 2), START_POSITION_Y);
			invaders = new Invader[COLUMNS, ROWS];
			projectiles = new Projectile[COLUMNS];

			for (int x = 0; x < COLUMNS; x++)
				projectiles[x] = new Projectile(projectile, scale);

			Random random = new Random();
			Vector2 offset = Vector2.Zero;

			for (int x = 0; x < COLUMNS; x++) {
				offset.X = x;
				invaders[x, 0] = new Invader(invader1, scale, position + (offset * INVADER_WIDTH), INVADER_1_SCORE, random.Next());
			}

			for (int y = 1; y < 3; y++) {
				offset.Y++;

				for (int x = 0; x < COLUMNS; x++) {
					offset.X = x;
					invaders[x, y] = new Invader(invader2, scale, position + (offset * INVADER_WIDTH), INVADER_2_SCORE, random.Next());
				}
			}

			for (int y = 3; y < ROWS; y++) {
				offset.Y++;

				for (int x = 0; x < COLUMNS; x++) {
					offset.X = x;
					invaders[x, y] = new Invader(invader3, scale, position + (offset * INVADER_WIDTH), INVADER_3_SCORE, random.Next());
				}
			}
		}

        /// <summary>
        /// Check if an invader has been shot.
        /// </summary>
        /// <param name="projectile"></param>
        /// <returns></returns>
		public bool CheckCollision(Projectile projectile) {
			if (projectile.Visible)
				foreach (Invader invader in invaders)
					if (invader.CheckCollision(projectile)) {
						lastScore = invader.Score;
						return true;
					}

			return false;
		}

        /// <summary>
        /// Draw each invader in the wave.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="activeWindowArea"></param>
		public void Draw(SpriteBatch spriteBatch, Rectangle activeWindowArea) {
			foreach (Invader invader in invaders)
				invader.Draw(spriteBatch, activeWindowArea);
		}

        /// <summary>
        /// Reset the wave to its initial state.
        /// </summary>
		public void Reset() {
			leftMostInvader = 0;
			rightMostInvader = COLUMNS - 1;
			bottomMostInvader = ROWS - 1;

			soundPointer = 0;
			count = COLUMNS * ROWS;
			direction = nextDirection = 1;
			position = new Vector2((SpaceInvaders.SCREEN_SIZE / 2) - (Width / 2), START_POSITION_Y);

			for (int x = 0; x < COLUMNS; x++)
				projectiles[x].Visible = false;

			Invader invader;
			Vector2 offset = Vector2.Zero;

			for (int y = 0; y < ROWS; y++) {
				offset.Y++;
				
				for (int x = 0; x < COLUMNS; x++) {
					offset.X = x;

					invader = invaders[x, y];
					invader.Position = position + (offset * invader.Width);
					invader.CurrentFrame = 0;
					invader.Visible = true;
					invader.Destroyed = false;
					invader.Projectile = null;
				}
			}
		}

        /// <summary>
        /// Updates the invader wave, handling invader movement etc.
        /// </summary>
        /// <param name="gameTime"></param>
		public void Update(GameTime gameTime) {
			if (count == 0)
				return;

            // Only update the invader wave position every few ticks, increasing this rate as more invaders are destroyed
			if (ticks > MathHelper.Lerp(TICK_RATE_MAX, TICK_RATE_MIN, ((float) count) / (ROWS * COLUMNS))) {
				ticks = 0;

				moveSounds[soundPointer++].Play();

				if (soundPointer >= moveSounds.Length)
					soundPointer = 0;

				int left = -1;
				int right = -1;
				int bottom = -1;

                // Find the leftmost, rightmost and bottommost invaders
                // Used for calculating how far left/right to move the invader wave
                // as well as calculating when a invader has reached the player, ending the game.
				for (int x = leftMostInvader; x <= rightMostInvader; x++)
					for (int y = bottomMostInvader; y >= 0; y--)
						if (invaders[x, y].Visible) {
							if (left < 0)
								left = x;
							
							if (y > bottom)
								bottom = y;
							
							right = x;
							break;
						}

				leftMostInvader = left;
				rightMostInvader = right;
				bottomMostInvader = bottom;

                // Direction: -1 = left, 0 = down, 1 = right

				if (direction != nextDirection)
					direction = nextDirection;

                // If at edge of screen, move downwards this tick and move in the opposite direction after.
				if (position.X + (leftMostInvader * INVADER_WIDTH) <= SpaceInvaders.SCREEN_BORDER_SIZE && direction == -1) {
					direction = 0;
					nextDirection = 1;
				} else if (position.X + ((rightMostInvader + 1) * INVADER_WIDTH) >= SpaceInvaders.SCREEN_SIZE - SpaceInvaders.SCREEN_BORDER_SIZE && direction == 1) {
					direction = 0;
					nextDirection = -1;
				}

				if (direction != 0)
					position.X += MOVE_AMOUNT_X * direction;
				else
					position.Y += MOVE_AMOUNT_Y;
			} else
				ticks++;

			Invader invader;
			Vector2 offset = Vector2.Zero;
			bool bottomInvader;

			for (int x = leftMostInvader; x <= rightMostInvader; x++) {
				bottomInvader = true;
				offset.X = x;

				for (int y = bottomMostInvader; y >= 0; y--) {
					invader = invaders[x, y];
					offset.Y = y;

					invader.Position = position + (offset * invader.Width);
					invader.Update(gameTime);

					if (ticks == 0 && invader.Visible && invader.Destroyed) {
                        // Remove destroyed invaders only on movement ticks,
                        // giving the invader time to show its destroyed animation
                        invader.Visible = false;
						count--;
					} else if (bottomInvader && invader.Visible && !invader.Destroyed) {
						// If the invader is at the bottom of a column and not destroyed, allow it to shoot
                        bottomInvader = false;
						
						if (invader.Projectile == null && !projectiles[x].Visible)
							invader.Projectile = projectiles[x];
					}
				}
			}
		}
	}
}

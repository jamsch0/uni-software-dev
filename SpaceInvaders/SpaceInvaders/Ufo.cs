using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders {
	
	public class Ufo : Sprite {

		#region Constants

		const float APPEAR_CHANCE = 0.001F;
		const float DIRECION_CHANCE = 0.5F;
		const float VELOCITY = 100.0F;
		
		const int FRAMES = 1;
		const int MIN_SCORE = 100;
		const int MAX_SCORE = 500;
		const int SCORE_NEAREST = 50;
		const int START_POSITION_Y = 40;
		
		#endregion

		#region Fields
		
		Random random;
		bool destroyed;
		int lastScore;
		SoundEffectInstance ufoSound;
		
		#endregion

		#region Properties

		public bool Destroyed {
			get { return destroyed; }
		}

		public int LastScore {
			get { return lastScore; }
		}
		
		#endregion

		public Ufo(Texture2D texture, float scale, SoundEffectInstance ufoSound)
			: base(texture, FRAMES, scale, new Vector2(0, START_POSITION_Y), Vector2.Zero, false) {
			this.ufoSound = ufoSound;
			this.ufoSound.IsLooped = true;
			random = new Random();
		}

        /// <summary>
        /// Check if UFO has been shot, resetting the UFO state and generating a score.
        /// </summary>
        /// <param name="projectile"></param>
        /// <returns></returns>
		public int CheckCollision(Projectile projectile) {
			if (visible && !destroyed && projectile.Visible && BoundingBox.Intersects(projectile.BoundingBox)) {
				destroyed = true;
				visible = false;
				projectile.Visible = false;
				ufoSound.Stop();

				lastScore = Util.RoundToNearest(random.Next(MIN_SCORE, MAX_SCORE), SCORE_NEAREST);
				return lastScore;
			}

			return 0;
		}

        /// <summary>
        /// Resets UFO to initial state.
        /// </summary>
		public void Reset() {
			destroyed = false;
			visible = false;
			velocity = Vector2.Zero;
			ufoSound.Stop();
		}

        /// <summary>
        /// Overrides base.Update to include appearance and movement direction dice rolls.
        /// </summary>
        /// <param name="gameTime"></param>
		public override void Update(GameTime gameTime) {
			if (destroyed)
				return;
			
			if (!visible && random.NextDouble() < APPEAR_CHANCE) {
				visible = true;
				ufoSound.Play();

                // Decide whether to move left to right or right to left.
				if (random.NextDouble() < DIRECION_CHANCE) {
					velocity.X = VELOCITY;
					position.X = -Width;
				} else {
					velocity.X = -VELOCITY;
					position.X = SpaceInvaders.SCREEN_SIZE;
				}
			}

			if (position.X + Width < 0 || position.X > SpaceInvaders.SCREEN_SIZE) {
				visible = false;
				ufoSound.Stop();
			}

			base.Update(gameTime);
		}
	}
}

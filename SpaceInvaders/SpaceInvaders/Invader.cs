using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders {
	
	public class Invader : Sprite {

		#region Constants

		public const int FRAMES = 3;

		const float SHOOT_CHANCE = 0.002F;
		const float PROJECTILE_VELOCITY = 200.0F;

		const int DESTROYED_FRAME = 2;

		#endregion

		#region Fields

		Random random;
		Projectile projectile;
		Vector2 oldPosition;
		bool destroyed;
		int score;

		#endregion

		#region Properties

		public Projectile Projectile {
			get { return projectile; }
			set { projectile = value; }
		}

		public bool Destroyed {
			get { return destroyed; }
			set { destroyed = value; }
		}

		public int Score {
			get { return score; }
		}

		#endregion

		public Invader(Texture2D texture, float scale, Vector2 position, int score, int seed)
			: base(texture, FRAMES, scale, position, Vector2.Zero) {
			this.score = score;
			oldPosition = position;
			random = new Random(seed);
		}

        /// <summary>
        /// Check if invader has been shot and play corresponding animation.
        /// </summary>
        /// <param name="projectile"></param>
        /// <returns></returns>
		public bool CheckCollision(Projectile projectile) {
			if (!destroyed && projectile.Visible && BoundingBox.Intersects(projectile.BoundingBox)) {
				destroyed = true;
				currentFrame = DESTROYED_FRAME;
				projectile.Visible = false;

				return true;
			}

			return false;
		}

        /// <summary>
        /// Overrides base.Draw to include drawing of invader projectile.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="activeWindowArea"></param>
		public override void Draw(SpriteBatch spriteBatch, Rectangle activeWindowArea) {
			base.Draw(spriteBatch, activeWindowArea);

			if (projectile != null)
				projectile.Draw(spriteBatch, activeWindowArea);
		}

        /// <summary>
        /// Overrides base.Update to include moving animation and shooting/updating invader projectile.
        /// </summary>
        /// <param name="gameTime"></param>
		public override void Update(GameTime gameTime) {
			if (visible && position != oldPosition) {
				currentFrame = (currentFrame == 0) ? 1 : 0;
				oldPosition = position;
			}

			if (projectile != null) {
				if (destroyed && !projectile.Visible) {
					projectile = null;
					return;
				} else if (!destroyed && !projectile.Visible && random.NextDouble() < SHOOT_CHANCE) {
					projectile.Position = new Vector2(position.X + (Width / 2) - (projectile.Width / 2), position.Y + Height);
					projectile.Velocity = new Vector2(0, PROJECTILE_VELOCITY);
					projectile.Visible = true;
				}

				projectile.Update(gameTime);
			}
		}
	}
}

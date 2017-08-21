using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders {

	public class Projectile : Sprite {

		#region Constants

		const int FRAMES = 1;

		#endregion

		public Projectile(Texture2D texture, float scale)
			: base(texture, FRAMES, scale, Vector2.Zero, Vector2.Zero, false) {
		}

        /// <summary>
        /// Check for collision between to projectiles, destroying them both.
        /// </summary>
        /// <param name="projectile"></param>
		public void CheckCollision(Projectile projectile) {
			if (visible && projectile.Visible && BoundingBox.Intersects(projectile.BoundingBox)) {
				visible = false;
				projectile.Visible = false;
			}
		}

        /// <summary>
        /// Overrides base.Update to include resetting the projectile if disabled or outside the screen.
        /// </summary>
        /// <param name="gameTime"></param>
		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (position.Y > SpaceInvaders.SCREEN_SIZE || position.Y + texture.Height < 0)
				visible = false;

			if (!visible) {
				position = Vector2.Zero;
				velocity = Vector2.Zero;
			}
		}
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders {
	
	public class Defence {

		#region Constants

		const int BLOCK_FRAMES = 4;

		readonly int BLOCK_WIDTH;

		#endregion

		#region Fields

		Texture2D fullBlock;
		Texture2D innerLeft;
		Texture2D innerRight;
		Texture2D outerLeft;
		Texture2D outerRight;

		Vector2 position;
		Sprite[] blocks;
		float scale;

		#endregion

		#region Properties

		public float Scale {
			get { return scale; }
			set {
				scale = value;

				foreach (Sprite block in blocks)
					block.Scale = scale;
			}
		}

		#endregion

		public Defence(Texture2D fullBlock, Texture2D innerLeft, Texture2D innerRight, Texture2D outerLeft, Texture2D outerRight, float scale, Vector2 position) {
			this.fullBlock = fullBlock;
			this.innerLeft = innerLeft;
			this.innerRight = innerRight;
			this.outerLeft = outerLeft;
			this.outerRight = outerRight;
			this.scale = scale;
			this.position = position;

			BLOCK_WIDTH = fullBlock.Width / BLOCK_FRAMES;

			blocks = new Sprite[] {
				new Sprite(outerLeft, BLOCK_FRAMES, scale, position, Vector2.Zero),
				new Sprite(fullBlock, BLOCK_FRAMES, scale, position + new Vector2(BLOCK_WIDTH, 0), Vector2.Zero),
				new Sprite(fullBlock, BLOCK_FRAMES, scale, position + new Vector2(2 * BLOCK_WIDTH, 0), Vector2.Zero),
				new Sprite(outerRight, BLOCK_FRAMES, scale, position + new Vector2(3 * BLOCK_WIDTH, 0), Vector2.Zero),
				new Sprite(fullBlock, BLOCK_FRAMES, scale, position + new Vector2(0, BLOCK_WIDTH), Vector2.Zero),
				new Sprite(innerLeft, BLOCK_FRAMES, scale, position + new Vector2(BLOCK_WIDTH, BLOCK_WIDTH), Vector2.Zero),
				new Sprite(innerRight, BLOCK_FRAMES, scale, position + new Vector2(2 * BLOCK_WIDTH, BLOCK_WIDTH), Vector2.Zero),
				new Sprite(fullBlock, BLOCK_FRAMES, scale, position + new Vector2(3 * BLOCK_WIDTH, BLOCK_WIDTH), Vector2.Zero),
				new Sprite(fullBlock, BLOCK_FRAMES, scale, position + new Vector2(0, 2 * BLOCK_WIDTH), Vector2.Zero),
				new Sprite(fullBlock, BLOCK_FRAMES, scale, position + new Vector2(3 * BLOCK_WIDTH, 2 * BLOCK_WIDTH), Vector2.Zero)
			};
		}

        /// <summary>
        /// Checks if a block in the defence has been shot, increasing its damage state.
        /// </summary>
        /// <param name="projectile"></param>
		public void CheckCollision(Projectile projectile) {
			if (!projectile.Visible)
				return;

			foreach (Sprite block in blocks) {
				if (block.Visible && block.BoundingBox.Intersects(projectile.BoundingBox)) {
					projectile.Visible = false;
					block.CurrentFrame++;

					if (block.CurrentFrame >= BLOCK_FRAMES)
						block.Visible = false;
				}
			}
		}

        /// <summary>
        /// Draws each block in the defence.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="activeWindowArea"></param>
		public void Draw(SpriteBatch spriteBatch, Rectangle activeWindowArea) {
			foreach (Sprite block in blocks)
				block.Draw(spriteBatch, activeWindowArea);
		}

        /// <summary>
        /// Resets the state of each block in the defence.
        /// </summary>
		public void Reset() {
			foreach (Sprite block in blocks) {
				block.CurrentFrame = 0;
				block.Visible = true;
			}
		}
	}
}

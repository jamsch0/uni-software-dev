using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders {
	
	public class Sprite {

		#region Fields

		protected readonly Texture2D texture;
		protected readonly int frames;
		
		protected int currentFrame;
		protected float scale;
		protected bool visible;

		protected Vector2 position;
		protected Vector2 velocity;
		
		#endregion

		#region Properties

		public int CurrentFrame {
			get { return currentFrame; }
			set { currentFrame = value; }
		}

		public virtual float Scale {
			get { return scale; }
			set { scale = value; }
		}

		public bool Visible {
			get { return visible; }
			set { visible = value; }
		}

		public Vector2 Position {
			get { return position; }
			set { position = value; }
		}

		public Vector2 Velocity {
			get { return velocity; }
			set { velocity = value; }
		}

		public int Width {
			get { return texture.Width / frames; }
		}

		public int Height {
			get { return texture.Height; }
		}

		public Rectangle BoundingBox {
			get {
				return new Rectangle((int) position.X, (int) position.Y, (int) Width, (int) Height);
			}
		}
		
		#endregion

		public Sprite(Texture2D texture, int frames, float scale, Vector2 position, Vector2 velocity, bool visible = true) {
			this.texture = texture;
			this.frames = frames;
			this.scale = scale;
			this.position = position;
			this.velocity = velocity;
			this.visible = visible;
		}

        /// <summary>
        /// Draws sprite to screen if visible, translating position vector as neccessary according to screen scale and activeWindowArea position.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="activeWindowArea"></param>
		public virtual void Draw(SpriteBatch spriteBatch, Rectangle activeWindowArea) {
			if (visible) {
				Vector2 drawPosition = new Vector2(activeWindowArea.X, activeWindowArea.Y) + (position * scale);
				spriteBatch.Draw(texture, drawPosition, new Rectangle(currentFrame * Width, 0, Width, Height), Color.White, 0.0F, Vector2.Zero, scale, SpriteEffects.None, 0.0F);
			}
		}

        /// <summary>
        /// Updates sprite position based upon its velocity.
        /// </summary>
        /// <param name="gameTime"></param>
		public virtual void Update(GameTime gameTime) {
			position += (velocity * scale) * (float) gameTime.ElapsedGameTime.TotalSeconds;
		}
	}
}

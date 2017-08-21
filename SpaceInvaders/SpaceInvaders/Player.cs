using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders {
	
	public class Player : Sprite {

		#region Constants

		public const int FRAMES = 2;

		const float VELOCITY = 200.0F;
		const float PROJECTILE_VELOCITY = 300.0F;

		const int DESTROYED_FRAME = 1;
		const int START_POSITION_Y = SpaceInvaders.SCREEN_SIZE - 60;

		#endregion

		#region Fields

		bool destroyed;
		Projectile projectile;
		SoundEffectInstance shootSound;
		SoundEffectInstance explosionSound;

		#endregion

		#region Properties

		public Projectile Projectile {
			get { return projectile; }
		}

		public bool Destroyed {
			get { return destroyed; }
		}

		public override float Scale {
			get { return scale; }
			set {
				scale = value;
				projectile.Scale = scale;
			}
		}
		
		#endregion

		public Player(Texture2D texture, Texture2D projectileTexture, float scale, SoundEffectInstance shootSound, SoundEffectInstance explosionSound)
			: base(texture, FRAMES, scale, new Vector2((SpaceInvaders.SCREEN_SIZE / 2) - ((texture.Width / FRAMES) / 2), START_POSITION_Y), Vector2.Zero) {
			this.shootSound = shootSound;
			this.explosionSound = explosionSound;
			projectile = new Projectile(projectileTexture, scale);
		}

        /// <summary>
        /// Check if player has been shot and play corresponding animation/sound.
        /// </summary>
        /// <param name="projectile"></param>
        /// <returns></returns>
		public bool CheckCollision(Projectile projectile) {
			if (!destroyed && projectile.Visible && BoundingBox.Intersects(projectile.BoundingBox)) {
				destroyed = true;
				currentFrame = DESTROYED_FRAME;
				projectile.Visible = false;
				explosionSound.Play();

				return true;
			}

			return false;
		}

        /// <summary>
        /// Overrides base.Draw to include drawing of player projectile.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="activeWindowArea"></param>
		public override void Draw(SpriteBatch spriteBatch, Rectangle activeWindowArea) {
			base.Draw(spriteBatch, activeWindowArea);
			projectile.Draw(spriteBatch, activeWindowArea);
		}

        /// <summary>
        /// Resets player to initial position and state.
        /// </summary>
		public void Reset() {
			position.X = (SpaceInvaders.SCREEN_SIZE / 2) - (Width / 2);
			destroyed = false;
			currentFrame = 0;
		}

        /// <summary>
        /// Overrides base.Update to include polling of user input and updating of player projectile.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="oldKeyboard"></param>
		public void Update(GameTime gameTime, KeyboardState oldKeyboard) {
			KeyboardState keyboard = Keyboard.GetState();

            // Move left or right on left/right arrow key press
			if (keyboard.IsKeyDown(Keys.Left) && position.X > SpaceInvaders.SCREEN_BORDER_SIZE)
				velocity.X = -VELOCITY;
			else if (keyboard.IsKeyDown(Keys.Right) && position.X + Width < SpaceInvaders.SCREEN_SIZE - SpaceInvaders.SCREEN_BORDER_SIZE)
				velocity.X = VELOCITY;
			else
				velocity.X = 0.0F;

            // Fire projectile when space key is pressed, as long as no previously fired projectiles are still in motion
			if (!projectile.Visible && keyboard.IsKeyDown(Keys.Space) && oldKeyboard.IsKeyUp(Keys.Space)) {
				projectile.Position = new Vector2(position.X + (Width / 2) - (projectile.Width / 2), position.Y - projectile.Height);
				projectile.Velocity = new Vector2(0, -PROJECTILE_VELOCITY);
				projectile.Visible = true;
				shootSound.Play();
			}

			projectile.Update(gameTime);
			base.Update(gameTime);
		}
	}
}

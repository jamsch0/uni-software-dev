using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders {
	
	public static class Util {

        /// <summary>
        /// Wrapper around spriteBatch.Draw overload with scale parameter, passing default values to all unused arguments.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="sourceRect"></param>
        /// <param name="color"></param>
        /// <param name="scale"></param>
        public static void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle sourceRect, Color color, float scale) {
            spriteBatch.Draw(texture, position, sourceRect, color, 0.0F, Vector2.Zero, scale, SpriteEffects.None, 0.0F);
        }

        /// <summary>
        /// Wrapper around spriteBatch.Draw overload with scale parameter, passing default values to all unused arguments.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="sourceRect"></param>
        /// <param name="scale"></param>
		public static void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle sourceRect, float scale) {
			Draw(spriteBatch, texture, position, sourceRect, Color.White, scale);
		}

        /// <summary>
        /// Wrapper around spriteBatch.DrawString overload with scale parameter, passing default values to all unused arguments.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="scale"></param>
        public static void DrawString(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale) {
            spriteBatch.DrawString(font, text, position, color, 0.0F, Vector2.Zero, scale, SpriteEffects.None, 0.0F);
        }

        /// <summary>
        /// Wrapper around spriteBatch.DrawString overload with scale parameter, passing default values to all unused arguments.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
		public static void DrawString(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, float scale) {
			DrawString(spriteBatch, font, text, position, Color.White, scale);
		}

        /// <summary>
        /// Wrapper around spriteBatch.DrawString overload with scale parameter, passing default values to all unused arguments.
        /// Centers the text on the Y axis.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="scale"></param>
        /// <param name="activeWindowArea"></param>
		public static void DrawStringCentered(SpriteBatch spriteBatch, SpriteFont font, string text, float position, Color color, float scale, Rectangle activeWindowArea) {
			Vector2 textSize = font.MeasureString(text);
			Vector2 drawPosition = new Vector2(activeWindowArea.Center.X - ((textSize.X / 2) * scale), position);
			DrawString(spriteBatch, font, text, drawPosition, color, scale);
		}

        /// <summary>
        /// Wrapper around spriteBatch.DrawString overload with scale parameter, passing default values to all unused arguments.
        /// Centers the text on the Y axis.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        /// <param name="activeWindowArea"></param>
        public static void DrawStringCentered(SpriteBatch spriteBatch, SpriteFont font, string text, float position, float scale, Rectangle activeWindowArea) {
            DrawStringCentered(spriteBatch, font, text, position, Color.White, scale, activeWindowArea);
        }

        /// <summary>
        /// Wrapper around spriteBatch.DrawString overload with scale parameter, passing default values to all unused arguments.
        /// Centers the text on the X and Y axis.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <param name="scale"></param>
        /// <param name="activeWindowArea"></param>
		public static void DrawStringCentered(SpriteBatch spriteBatch, SpriteFont font, string text, Color color, float scale, Rectangle activeWindowArea) {
			Vector2 textSize = font.MeasureString(text);
			Vector2 drawPosition = new Vector2(activeWindowArea.Center.X - ((textSize.X / 2) * scale), activeWindowArea.Center.Y - ((textSize.Y / 2) * scale));
			DrawString(spriteBatch, font, text, drawPosition, color, scale);
		}

        /// <summary>
        /// Wrapper around spriteBatch.DrawString overload with scale parameter, passing default values to all unused arguments.
        /// Centers the text on the X and Y axis.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <param name="scale"></param>
        /// <param name="activeWindowArea"></param>
        public static void DrawStringCentered(SpriteBatch spriteBatch, SpriteFont font, string text, float scale, Rectangle activeWindowArea) {
            DrawStringCentered(spriteBatch, font, text, Color.White, scale, activeWindowArea);
        }

        /// <summary>
        /// Round integer to nearest n, e.g: nearest 10.
        /// </summary>
        /// <param name="valueToRound"></param>
        /// <param name="nearest"></param>
        /// <returns></returns>
		public static int RoundToNearest(int valueToRound, int nearest) {
			return nearest * (int) Math.Round(valueToRound / (float) nearest);
		}
	}
}

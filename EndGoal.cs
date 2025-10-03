using CollisionExample.Collisions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject1
{
    public class EndGoal
    {
        private int spriteWidth = 118;
        private int spriteHeight = 128;
        private float spriteScale = 2.0f;

        private Texture2D texture;

        private BoundingRectangle bounds;

        private double animationTimer;

        private short animationFrame = 1;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;
        
        public Vector2 Position;

        /// <summary>
        /// Creates a new coin sprite
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public EndGoal(Vector2 position)
        {
            this.Position = position;
            // The issue lies in the calculation of the bounds' position. The position of the bounding rectangle is being offset incorrectly.
            // Instead of subtracting half the scaled width and height, you should adjust the bounds' position to align with the sprite's center.

            this.bounds = new BoundingRectangle(
                new Vector2(
                    this.Position.X,
                    this.Position.Y
                ),
                spriteWidth * spriteScale,
                spriteHeight * spriteScale
            );
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("OakWoodsAssets//oak_woods_v1.0//decorations//shop_anim");

        }

        /// <summary>
        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle source = FindAnimationFrame(gameTime, texture.Width / spriteWidth);
            spriteBatch.Draw(texture, Position, source, Color.White, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0.0f);
            //spriteBatch.Draw(texture, new Vector2(this.position.X, this.position.Y), source, Color.DarkRed, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0.0f);
        }

        private Rectangle FindAnimationFrame(GameTime gameTime, int totalFrames)
        {
            //Update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Update Animation frame
            if (animationTimer > 0.15)
            {
                animationFrame++;
                if (animationFrame > totalFrames - 1) animationFrame = 1;

                animationTimer -= 0.15;
            }
            return new Rectangle(animationFrame * spriteWidth, 0, spriteWidth, spriteHeight);
        }
    } 
}

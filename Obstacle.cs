using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;


namespace GameProject1
{
    public class Obstacle
    {
        public Vector2 Position;

        private Texture2D texture;

        private BoundingRectangle bounds;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// Creates a new coin sprite
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public Obstacle(Vector2 position)
        {
            this.Position = position;
            this.bounds = new BoundingRectangle(position, 27 * 2.0f, 12 * 2.0f);
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("OakWoodsAssets//oak_woods_v1.0//decorations//rock_2");

        }

        /// <summary>
        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0.0f);
        }
    }
}

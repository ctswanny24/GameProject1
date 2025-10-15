using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using InputExample;
using Microsoft.Xna.Framework.Input;
using GameProject1.Enums;

namespace GameProject1
{

    public class PlayerSprite
    {


        /// <summary>
        /// A class representing a player sprite
        /// </summary>
        /// 
        private int frameWidth = 56;
        private int spriteWidth = 56;
        private int spriteHeight = 56;
        private int spriteXOffset = 0;
        private float spriteScale = 1.75f;

        private Texture2D collisionBox;

        private Texture2D[] texture;

        private double directionTimer;

        private double animationTimer;

        private short animationFrame = 1;

        public int ScreenWidth;

        private int health = 2;

        public bool Dead { get; private set; } = false;

        public bool Colliding { get; set; } = false;

        public SpriteEffects Direction;

        public Vector2 Position;

        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;

            public void Initialize()
            {
                bounds = new BoundingRectangle(new Vector2(Position.X - ((spriteWidth * spriteScale) / 2), Position.Y -((spriteHeight * spriteScale) / 2)), spriteWidth * spriteScale, spriteHeight * spriteScale);
            }

        /// <summary>
        /// Loads the PlayerSprite texture
        /// </summary>
        /// <param name="content">The content manager to load</param>
        public void LoadContent(ContentManager content)
        {
            texture = new Texture2D[]
            {
                content.Load<Texture2D>("OakWoodsAssets\\oak_woods_v1.0\\character\\char_blue"),
            };
        }

        /// <summary>
        /// Updates the PlayerSprite to know when facing a certain direction.
        /// </summary>
        /// <param name="gameTime"> The game time </param>
        public void Update(GameTime gameTime)
        {
            if (!Colliding)
            {
                //Position += InputManager.Movement;
                bounds.X = Position.X - 2;
                bounds.Y = Position.Y - 2;
            }
            else
            {
                Dead = true;
            }
        }

            /// <summary>
            /// Draws the animated sprite
            /// </summary>
            /// <param name="gameTime"> The game time </param>
            /// <param name="spriteBatch"> The SpriteBatch to draw with </param>
            public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
            {
            Rectangle source;

            spriteBatch.Draw(texture[0], Position, new Rectangle(0, 0, spriteWidth, spriteHeight), Color.White, 0.0f, Vector2.Zero, 1.0f, Direction, 0.0f);

            }

            /// <summary>
            /// Finds the animation frame that needs to be displayed. 
            /// </summary>
            /// <param name="gameTime">Game time</param>
            /// <param name="totalFrames">The total number of frames in that particular png</param>
            /// <returns></returns>
            private Rectangle FindAnimationFrame(GameTime gameTime, int totalFrames)
        {
            //Update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Update Animation frame
            if (animationTimer > 0.1)
            {
                animationFrame++;
                if (animationFrame > totalFrames - 1) animationFrame = 0;
                    
                animationTimer -= 0.1;
            }
            return new Rectangle(animationFrame * frameWidth + spriteXOffset, 42, spriteWidth, spriteHeight);
        }
            
        public void TakeDamage()
        {
            if(health > 1)
            {
                health--;
            }
            else
            {
                Dead = true;
            }
        }

    }
}

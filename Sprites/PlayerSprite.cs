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
using System.Diagnostics.Eventing.Reader;
using SharpDX.DirectWrite;
using System.Reflection.Metadata.Ecma335;

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
        private float intangibilityTimer = 0f;

        private Texture2D heart;

        private Texture2D collisionBox;

        private Texture2D[] texture;

        private double directionTimer;

        private double animationTimer;

        private int animationFrame = 1;

        public int ScreenWidth;

        public int Health = 3;

        public Enums.CharacterStates State { get; set; }
        public bool Damaged { get; set; } = false;
        public bool Dead { get; private set; } = false;

        public bool Colliding { get; set; } = false;

        public SpriteEffects Direction;

        public Vector2 Position;

        public bool Intangible { get; set; } = false;

        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;

        public void Initialize()
        {
            bounds = new BoundingRectangle(new Vector2(Position.X - ((spriteWidth * spriteScale) / 2), Position.Y - ((spriteHeight * spriteScale) / 2)), spriteWidth * spriteScale, spriteHeight * spriteScale);
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
            heart = content.Load<Texture2D>("CaveAssets\\Items\\000_0062_heart4");
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
                if (Damaged && (intangibilityTimer < 3.0))
                {
                    intangibilityTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Damaged = true;
                }
                else
                {
                    Colliding = false;
                    Damaged = false;
                    Intangible = false;
                    intangibilityTimer = 0.0f;
                }
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
            switch (State)
            {
                case CharacterStates.Running:
                    source = FindAnimationFrame(gameTime, 8, 2);
                    break;
                case CharacterStates.Idle:
                    source = FindAnimationFrame(gameTime, 6, 0);
                    break;
                case CharacterStates.Dead:
                    source = AnimateDeath(gameTime, 8);
                    break;
                default:
                    source = FindAnimationFrame(gameTime, 1, 0);
                    break;
            }

            for(int i = Health; i > 0; i--)
            {
                spriteBatch.Draw(heart, new Vector2(ScreenWidth - (31 * i), 0), Color.White);
            }

            if (Damaged && (intangibilityTimer < 1.5))
            {
                if (intangibilityTimer < .5f || intangibilityTimer > 1.5f || intangibilityTimer < 2.0f || intangibilityTimer > 2.5f)
                {
                    spriteBatch.Draw(texture[0], Position, source, Color.Crimson, 0.0f, Vector2.Zero, 1.0f, Direction, 0.0f);
                }
                else if (intangibilityTimer >= .5f && intangibilityTimer < 1.5f || (intangibilityTimer > 2.0f && intangibilityTimer < 2.5f))
                {
                    spriteBatch.Draw(texture[0], Position, source, Color.White, 0.0f, Vector2.Zero, 1.0f, Direction, 0.0f);
                }
            }
            else
            {
                spriteBatch.Draw(texture[0], Position, source, Color.White, 0.0f, Vector2.Zero, 1.0f, Direction, 0.0f);
            }

            
        }

        /// <summary>
        /// Finds the animation frame that needs to be displayed. 
        /// </summary>
        /// <param name="gameTime">Game time</param>
        /// <param name="totalFrames">The total number of frames in that particular png</param>
        /// <returns></returns>
        private Rectangle FindAnimationFrame(GameTime gameTime, int totalFrames, int spriteMapRow)
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
            return new Rectangle(animationFrame * frameWidth + spriteXOffset, spriteMapRow * spriteHeight, spriteWidth, spriteHeight);
        }

        private Rectangle AnimateDeath(GameTime gameTime, int totalFrames)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if(animationTimer > 0.1)
            {
                animationFrame++;
                if (animationFrame > totalFrames - 1) animationFrame = (totalFrames - 1);

                animationTimer -= 0.1;
            }
            return new Rectangle(animationFrame * frameWidth + spriteXOffset, 5 * spriteHeight, spriteWidth, spriteHeight);
        }

        public void TakeDamage()
        {
            if (Health > 1)
            {
                if (Damaged)
                {
                }
                else
                {
                    Health--;
                    Intangible = true;
                    Damaged = true;
                }
            }
            else if (Intangible)
            {

            }
            else
            {
                Health--;
                Dead = true;
                State = CharacterStates.Dead;
            }
        }

    }
}

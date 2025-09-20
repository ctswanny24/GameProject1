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
using CollisionExample.Collisions;

namespace GameProject1
{

    public class KnightSprite
    {


        /// <summary>
        /// A class representing a Knight sprite
        /// </summary>
        /// 
        private int frameWidth = 120;
        private int spriteWidth = 30;
        private int spriteHeight = 39;
        private int spriteXOffset = 40;
        private float spriteScale = 1.75f;

        private Texture2D collisionBox;

            private Texture2D[] texture;

            private double directionTimer;

            private double animationTimer;

            private short animationFrame = 1;

            public InputManager InputManager { get; set; }

            public int ScreenWidth;

            public bool Colliding { get; set; } = false;

            public bool AllowRight { get; set; } = true;
            public bool AllowLeft { get; set; } = true;

            /// <summary>
            /// Direction of the Knight
            /// </summary>
            public SpriteEffects Direction;

            /// <summary>
            /// The position of the Knight
            /// </summary>
            public Vector2 Position;

            private BoundingRectangle bounds;
            public BoundingRectangle Bounds => bounds;

            public void Initialize()
            {
                InputManager = new InputManager();
                bounds = new BoundingRectangle(new Vector2(Position.X - ((spriteWidth * spriteScale) / 2), Position.Y -((spriteHeight * spriteScale) / 2)), spriteWidth * spriteScale, spriteHeight * spriteScale);
            }

        /// <summary>
        /// Loads the KnightSprite texture
        /// </summary>
        /// <param name="content">The content manager to load</param>
        public void LoadContent(ContentManager content)
        {
            texture = new Texture2D[]
            {
                content.Load<Texture2D>("FreeKnight_v1//Colour1//NoOutline//120x80_PNGSheets//_Idle"),
                content.Load<Texture2D>("FreeKnight_v1//Colour1//NoOutline//120x80_PNGSheets//_Run"),
                content.Load<Texture2D>("FreeKnight_v1//Colour1//NoOutline//120x80_PNGSheets//_TurnAround"),
                content.Load<Texture2D>("FreeKnight_v1//Colour1//NoOutline//120x80_PNGSheets//_CrouchFull"),
                content.Load<Texture2D>("FreeKnight_v1//Colour1//NoOutline//120x80_PNGSheets//_Attack2NoMovement")
            };
        }
            /// <summary>
            /// Updates the KnightSprite to know when facing a certain direction.
            /// </summary>
            /// <param name="gameTime"> The game time </param>
            public void Update(GameTime gameTime)
            {
            InputManager.Update(gameTime);
            if(Position.X < -40)
            {
                Position = new Vector2(ScreenWidth, Position.Y);
            }
            else if (Position.X > ScreenWidth + 20)
            {
                Position = new Vector2(-40, Position.Y);
            }

            if(InputManager.Direction == Enums.Direction.Left)
            {
                Direction = SpriteEffects.FlipHorizontally;
            }
            else
            {
                Direction = SpriteEffects.None;
            }
            if (!Colliding)
            {
                Position += InputManager.Movement;
                bounds.X = Position.X - 2;
                bounds.Y = Position.Y - 2;
            }
            else if (Colliding && AllowLeft && InputManager.Direction == Enums.Direction.Left)
            {
                Position += InputManager.Movement;
                bounds.X = Position.X - 2;
                bounds.Y = Position.Y - 2;
            }
            else if(Colliding && AllowRight && InputManager.Direction == Enums.Direction.Right)
            {
                Position += InputManager.Movement;
                bounds.X = Position.X - 2;
                bounds.Y = Position.Y - 2;
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
            switch ((int)InputManager.State)
            {
                case 0:
                    source = FindAnimationFrame(gameTime, texture[0].Width / 120);
                    spriteBatch.Draw(texture[0], Position, source, Color.White, 0, Vector2.Zero, spriteScale, Direction, 0.0f);
                    break;
                case 1:
                    source = FindAnimationFrame(gameTime, texture[1].Width / 120);
                    spriteBatch.Draw(texture[1], Position, source, Color.White, 0, Vector2.Zero, spriteScale, Direction, 0.0f);
                    break;
                case 2:
                    source = FindAnimationFrame(gameTime, texture[2].Width / 120);
                    spriteBatch.Draw(texture[2], Position, source, Color.White, 0, Vector2.Zero, spriteScale, Direction, 0.0f);
                    break;
                case 3:
                    source = FindAnimationFrame(gameTime, texture[3].Width / 240);
                    spriteBatch.Draw(texture[3], Position, source, Color.White, 0, Vector2.Zero, spriteScale, Direction, 0.0f);
                    break;
                case 4:
                    source = FindAnimationFrame(gameTime, texture[4].Width / 120);
                    spriteBatch.Draw(texture[4], Position, source, Color.White, 0, Vector2.Zero, spriteScale, Direction, 0.0f);
                    break;
            }

            //spriteBatch.Draw(texture[0], Position, source, Color.White, 0, Vector2.Zero, 1.5f, Direction, 0.0f);
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
            
        }
    }

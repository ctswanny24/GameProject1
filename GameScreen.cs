using GameProject1.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System.Collections.Generic;
namespace GameProject1
{
    public class GameScreen : Game
    {
        //Tasks:
        //Keyboard, gamepad, or mouse input COMPLETE
        //Collision Detection with collision primitives
        //Multi-frame animated sprites COMPLETE


        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        KnightSprite knight;
        Texture2D[] backgrounds;
        Texture2D floorTexture;
        Obstacle obstacle;
        EndGoal endGoal;
        private SpriteFont arial;
        private SpriteFont algerian;

        private bool closeGame = false;
        private double endingTime = 0.0;

        public GameScreen()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            backgrounds = new Texture2D[3];
            knight = new KnightSprite() { Position = new Vector2(0, graphics.GraphicsDevice.Viewport.Height - 105), ScreenWidth = graphics.GraphicsDevice.Viewport.Width };
            obstacle = new Obstacle(new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - 27, graphics.GraphicsDevice.Viewport.Height - 63));
            endGoal = new EndGoal(new Vector2((graphics.GraphicsDevice.Viewport.Width * .75f) - 116, graphics.GraphicsDevice.Viewport.Height - 290));
            knight.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            backgrounds[0] = Content.Load<Texture2D>("OakWoodsAssets//oak_woods_v1.0//background//background_layer_1");
            backgrounds[1] = Content.Load<Texture2D>("OakWoodsAssets//oak_woods_v1.0//background//background_layer_2");
            backgrounds[2] = Content.Load<Texture2D>("OakWoodsAssets//oak_woods_v1.0//background//background_layer_3");

            floorTexture = Content.Load<Texture2D>("OakWoodsAssets//oak_woods_v1.0//oak_woods_tileset");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            knight.LoadContent(Content);
            obstacle.LoadContent(Content);
            endGoal.LoadContent(Content);
            arial = Content.Load<SpriteFont>("arial");
            algerian = Content.Load<SpriteFont>("Algerian");
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (closeGame)
            {
                endingTime += gameTime.ElapsedGameTime.TotalSeconds;
                if(endingTime > 3.0)
                {
                    Exit();
                }
            }

            // TODO: Add your update logic here
            knight.Update(gameTime);

            if(obstacle.Bounds.CollidesWith(knight.Bounds) == Enums.CollisionSide.None)
            {
                knight.Colliding = false;
                knight.AllowLeft = true;
                knight.AllowRight = true;
            }
            else if (obstacle.Bounds.CollidesWith(knight.Bounds) == Enums.CollisionSide.Left)
            {
                knight.InputManager.State = Enums.CharacterStates.Idle;
                knight.Colliding = true;
                knight.AllowLeft = false;
                knight.AllowRight = true;
            }
            else if(obstacle.Bounds.CollidesWith(knight.Bounds) == Enums.CollisionSide.Right)
            {
                knight.InputManager.State = Enums.CharacterStates.Idle;
                knight.Colliding = true;
                knight.AllowRight = false;
                knight.AllowLeft = true;
            }

            if(endGoal.Bounds.CollidesWith(knight.Bounds) == Enums.CollisionSide.Right || endGoal.Bounds.CollidesWith(knight.Bounds) == Enums.CollisionSide.Left)
            {
                closeGame = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);
            spriteBatch.Begin();
            
            // TODO: Add your drawing code here
            foreach (Texture2D t in backgrounds)
            {
                spriteBatch.Draw(t, new Vector2(0, 0), null, Color.White, 0.0f, Vector2.Zero, 2.7f, SpriteEffects.None, 0);
            }


            obstacle.Draw(gameTime, spriteBatch);
            endGoal.Draw(gameTime, spriteBatch);
            knight.Draw(gameTime, spriteBatch);
            for (int x = -10; x < graphics.GraphicsDevice.Viewport.Width; x += 72)
            {
                spriteBatch.Draw(floorTexture, new Vector2(x, graphics.GraphicsDevice.Viewport.Height - 40), new Rectangle(119, 215, 72, 20), Color.White, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0.0f);
            }

            if(gameTime.TotalGameTime.TotalSeconds < 30)
            {
                spriteBatch.DrawString(algerian, $"Get to tha shoppa", new Vector2((graphics.GraphicsDevice.Viewport.Width - arial.MeasureString("Get to tha shoppa").X) / 2, 10), Color.RoyalBlue);
                spriteBatch.DrawString(algerian, $"To win, you must get to the shop getting past that VERY devious looking rock", new Vector2((graphics.GraphicsDevice.Viewport.Width - arial.MeasureString("To win, you must get to the shop getting past that VERY devious looking rock").X) / 2, graphics.GraphicsDevice.Viewport.Height - 25), Color.RoyalBlue);
            }
            else if(gameTime.TotalGameTime.TotalSeconds >= 30)
            {
                spriteBatch.DrawString(algerian, $"Was this too ambiguous?", new Vector2((graphics.GraphicsDevice.Viewport.Width - arial.MeasureString("Was this too ambiguous?").X) / 2, 10), Color.RoyalBlue);
                spriteBatch.DrawString(algerian, $"You go off the screen on the left to get to the right", new Vector2((graphics.GraphicsDevice.Viewport.Width - arial.MeasureString("You go off the screen on the left to get to the right").X) / 2, graphics.GraphicsDevice.Viewport.Height - 25), Color.RoyalBlue);

            }

            if (gameTime.TotalGameTime.TotalSeconds > 5)
            {
                spriteBatch.DrawString(algerian, $"Perhaps you could think outside the box", new Vector2((graphics.GraphicsDevice.Viewport.Width - arial.MeasureString("Perhaps you could think outside the box").X) / 2, 40), Color.SkyBlue);
            }
            if(gameTime.TotalGameTime.TotalSeconds > 16)
            {
                spriteBatch.DrawString(algerian, $"Turn left to go right", new Vector2((graphics.GraphicsDevice.Viewport.Width - arial.MeasureString("Turn left to go right").X) / 2, 55), Color.SkyBlue);
            }

            if (closeGame)
            {
                spriteBatch.DrawString(algerian, $"You got to the shop!!! Great work!!!", new Vector2((graphics.GraphicsDevice.Viewport.Width - arial.MeasureString("You got to the shop!!! Great work!!!").X) / 2, (graphics.GraphicsDevice.Viewport.Height / 2)), Color.MonoGameOrange);
                spriteBatch.DrawString(algerian, $"Exiting Game...", new Vector2((graphics.GraphicsDevice.Viewport.Width - arial.MeasureString("Exiting Game...").X) / 2, (graphics.GraphicsDevice.Viewport.Height / 2 + 30)), Color.MonoGameOrange);

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

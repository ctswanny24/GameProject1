using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject1.StateManagement;
using System.Threading;
using SharpDX.Direct3D9;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using SharpDX.Direct2D1;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace GameProject1.Screens
{
    public class MinigameScreen : GameScreen
    {
        KnightSprite knight;
        Texture2D[] backgrounds;
        Texture2D floorTexture;
        Obstacle obstacle;
        EndGoal endGoal;
        private SpriteFont arial;
        private SpriteFont algerian;
        private Song music;
        private Song victoryTone;
        private SoundEffect collision;
        private float soundTimer = 1;


        private bool closeGame = false;
        private double endingTime = 0.0;
        private float inBetweenSounds = 0;

        private ContentManager content;
        private readonly InputAction _pauseAction;
        private float pauseAlpha;
        private GraphicsDevice graphics;

        private Vector2 playerPosition;
        private Vector2 obstaclePosition;
        private Vector2 goalPosition;

        private bool playedBefore = false;

        public MinigameScreen(GraphicsDevice graphics)
        {
            this.graphics = graphics;
            backgrounds = new Texture2D[3];

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
        }

        public void Initialize()
        {
            playerPosition = new Vector2(0, graphics.Viewport.Height - 105);
            obstaclePosition = new Vector2((graphics.Viewport.Width / 2) - 27, graphics.Viewport.Height - 63);
            goalPosition = new Vector2((graphics.Viewport.Width * .75f) - 116, graphics.Viewport.Height - 290);

            knight = new KnightSprite() { Position = playerPosition, ScreenWidth = graphics.Viewport.Width };
            obstacle = new Obstacle(obstaclePosition);
            endGoal = new EndGoal(goalPosition);

            knight.Initialize();
        }

        public override void Activate()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");

                backgrounds[0] = content.Load<Texture2D>("OakWoodsAssets//oak_woods_v1.0//background//background_layer_1");
                backgrounds[1] = content.Load<Texture2D>("OakWoodsAssets//oak_woods_v1.0//background//background_layer_2");
                backgrounds[2] = content.Load<Texture2D>("OakWoodsAssets//oak_woods_v1.0//background//background_layer_3");

                floorTexture = content.Load<Texture2D>("OakWoodsAssets//oak_woods_v1.0//oak_woods_tileset");

                music = content.Load<Song>("Jon Shuemaker - Autumn's Awakening");
                collision = content.Load<SoundEffect>("collision");
                victoryTone = content.Load<Song>("VictoryChime");
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 0.10f;
                MediaPlayer.Play(music);

                knight.LoadContent(content);
                obstacle.LoadContent(content);
                endGoal.LoadContent(content);
                arial = content.Load<SpriteFont>("arial");
                algerian = content.Load<SpriteFont>("Algerian");

                Thread.Sleep(1000);

                ScreenManager.Game.ResetElapsedTime();
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (coveredByOtherScreen)
            {
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            }
            else
            {
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
            }

            if (IsActive)
            {
                if (closeGame)
                {
                    if (endingTime == 0)
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.IsRepeating = false;
                        MediaPlayer.Play(victoryTone);
                    }
                    endingTime += gameTime.ElapsedGameTime.TotalSeconds;
                    if (endingTime > 4.2)
                    {
                        LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
                    }
                }

                // TODO: Add your update logic here
                knight.Update(gameTime);
                if (obstacle.Bounds.CollidesWith(knight.Bounds) == Enums.CollisionSide.None)
                {
                    knight.Colliding = false;
                    knight.AllowLeft = true;
                    knight.AllowRight = true;
                }
                else if (obstacle.Bounds.CollidesWith(knight.Bounds) == Enums.CollisionSide.Left)
                {
                    if (!playedBefore && inBetweenSounds >= soundTimer)
                    {
                        collision.Play();
                        playedBefore = true;
                        inBetweenSounds = 0;
                    }
                    knight.InputManager.State = Enums.CharacterStates.Idle;
                    knight.Colliding = true;
                    knight.AllowLeft = false;
                    knight.AllowRight = true;
                }
                else if (obstacle.Bounds.CollidesWith(knight.Bounds) == Enums.CollisionSide.Right)
                {
                    if (!playedBefore && inBetweenSounds >= soundTimer)
                    {
                        collision.Play();
                        playedBefore = true;
                        inBetweenSounds = 0;
                    }
                    knight.InputManager.State = Enums.CharacterStates.Idle;
                    knight.Colliding = true;
                    knight.AllowRight = false;
                    knight.AllowLeft = true; 
                }
                inBetweenSounds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                playedBefore = false;

                if (endGoal.Bounds.CollidesWith(knight.Bounds) == Enums.CollisionSide.Right || endGoal.Bounds.CollidesWith(knight.Bounds) == Enums.CollisionSide.Left)
                {
                    closeGame = true;
                }

            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex player;
            if(_pauseAction.Occurred(input, ControllingPlayer, out player))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            // TODO: Add your drawing code here
            foreach (Texture2D t in backgrounds)
            {
                spriteBatch.Draw(t, new Vector2(0, 0), null, Color.White, 0.0f, Vector2.Zero, 2.7f, SpriteEffects.None, 0);
            }


            obstacle.Draw(gameTime, spriteBatch);
            endGoal.Draw(gameTime, spriteBatch);
            knight.Draw(gameTime, spriteBatch);
            for (int x = -10; x < graphics.Viewport.Width; x += 72)
            {
                spriteBatch.Draw(floorTexture, new Vector2(x, graphics.Viewport.Height - 40), new Rectangle(119, 215, 72, 20), Color.White, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0.0f);
            }

            if (gameTime.TotalGameTime.TotalSeconds < 30)
            {
                spriteBatch.DrawString(algerian, $"Get to tha shoppa", new Vector2((graphics.Viewport.Width - arial.MeasureString("Get to tha shoppa").X) / 2, 10), Color.RoyalBlue);
                spriteBatch.DrawString(algerian, $"To win, you must get to the shop getting past that VERY devious looking rock", new Vector2((graphics.Viewport.Width - arial.MeasureString("To win, you must get to the shop getting past that VERY devious looking rock").X) / 2, graphics.Viewport.Height - 25), Color.RoyalBlue);
            }
            else if (gameTime.TotalGameTime.TotalSeconds >= 30)
            {
                spriteBatch.DrawString(algerian, $"Was this too ambiguous?", new Vector2((graphics.Viewport.Width - arial.MeasureString("Was this too ambiguous?").X) / 2, 10), Color.RoyalBlue);
                spriteBatch.DrawString(algerian, $"You go off the screen on the left to get to the right", new Vector2((graphics.Viewport.Width - arial.MeasureString("You go off the screen on the left to get to the right").X) / 2, graphics.Viewport.Height - 25), Color.RoyalBlue);
            }

            if (gameTime.TotalGameTime.TotalSeconds > 5)
            {
                spriteBatch.DrawString(algerian, $"Perhaps you could think outside the box", new Vector2((graphics.Viewport.Width - arial.MeasureString("Perhaps you could think outside the box").X) / 2, 40), Color.SkyBlue);
            }
            if (gameTime.TotalGameTime.TotalSeconds > 16)
            {
                spriteBatch.DrawString(algerian, $"Turn left to go right", new Vector2((graphics.Viewport.Width - arial.MeasureString("Turn left to go right").X) / 2, 55), Color.SkyBlue);
            }
            if (closeGame)
            {
                spriteBatch.DrawString(algerian, $"You got to the shop!!! Great work!!!", new Vector2((graphics.Viewport.Width - arial.MeasureString("You got to the shop!!! Great work!!!").X) / 2, (graphics.Viewport.Height / 2)), Color.MonoGameOrange);
                spriteBatch.DrawString(algerian, $"Returning to menu", new Vector2((graphics.Viewport.Width - arial.MeasureString("Returning to menu").X) / 2, (graphics.Viewport.Height / 2 + 30)), Color.MonoGameOrange);
            }
            spriteBatch.End();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameProject1.ParticleSystems;
using GameProject1.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.MediaFoundation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameProject1.Screens
{
    public class RockslideMinigame : GameScreen
    {
        private double gameTimer = 0;
        private float winTime = 15;
        private bool beginGame = false;
        private bool closeGame = false;
        private SoundEffect rumble;

        private float shakeIntensity = 0f;
        private float shakeDuration = 0f;
        private Random rand = new Random();
        private float shakeInterval = 0f; 

        private List<Texture2D> backgrounds;
        private ContentManager content;
        private readonly InputAction _pauseAction;
        private readonly InputAction _beginGame;
        private float pauseAlpha;
        private GraphicsDevice graphics;
        private RockslideParticleSystem rockslide;
        private Game _game;
        private bool LockedMovement = false;
        private Texture2D torch;
        private SpriteFont algerian;
        private Texture2D caveGround;
        private Song song;


        public BoundingRectangle PlayerHitbox { get; set; }
        public PlayerSprite Player;


        public RockslideMinigame(GraphicsDevice graphics, Game game)
        {
            _game = game;

            this.graphics = graphics;
            Player = new PlayerSprite() { Position = new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height - 86), ScreenWidth = graphics.Viewport.Width};
            backgrounds = new List<Texture2D>();

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);

            _beginGame = new InputAction(
                new[] { Buttons.A },
                new[] { Keys.Space, Keys.Enter }, true);
        }

        public void Initialize()
        {
            rockslide = new RockslideParticleSystem(_game, new Rectangle(0, 0, graphics.Viewport.Width, 0));
            _game.Components.Add(rockslide);
        }

        public override void Activate()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
                Player.LoadContent(content);
                backgrounds.Add(content.Load<Texture2D>("CaveAssets\\Background\\Pale\\Background"));
                backgrounds.Add(content.Load<Texture2D>("CaveAssets\\Background\\Pale\\bg"));
                torch = content.Load<Texture2D>("CaveAssets\\Details\\torch1_2");
                algerian = content.Load<SpriteFont>("Algerian");
                caveGround = content.Load<Texture2D>("CaveGround");
                rumble = content.Load<SoundEffect>("8bit_bomb_explosion");
                song = content.Load<Song>("Oneosune - Silent Realm.mp3");
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = .5f;
                MediaPlayer.Play(song);

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
            Player.Update(gameTime);

            if(shakeInterval > 2.0f)
            {
                rumble.Play();
                TriggerTremor(3.5f, 0.75f);
                shakeInterval = 0;
            }

            if (Player.Damaged)
            {
                TriggerTremor(1.0f, .05f);
            }

            Particle[] particles = rockslide.GetParticles();
            for(int i = 0; i < particles.Length; i++)
            {
                if (Player.Bounds.CollidesWith(particles[i].bounds))
                {
                    Player.TakeDamage();
                    Player.Colliding = true;
                }
            }

            if (Player.Dead)
            {
                EndGame();
            }

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

                if (beginGame)
                {
                    if(shakeDuration > 0)
                    {
                        shakeDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if(shakeDuration <= 0)
                        {
                            shakeIntensity = 0f;
                        }
                    }

                    gameTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    shakeInterval += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (gameTimer > 20.0f)
                    {
                        EndGame();
                        if(gameTimer > 25.0f)
                        {
                            MediaPlayer.Stop();
                            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen(_game));
                        }
                    }
                }
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            #region PauseAction and BeginGame
            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player))
            {
                beginGame = false;
                rockslide.Occurring = false;
                ScreenManager.AddScreen(new PauseMenuScreen(_game), ControllingPlayer);
            }

            if(_beginGame.Occurred(input, ControllingPlayer, out player))
            {
                beginGame = true;
                rockslide.Occurring = true;
            }
            #endregion

            #region PlayerMovement
            var movement = Vector2.Zero;
            if (!LockedMovement)
            {
                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                {
                    movement.X--;
                    Player.Direction = SpriteEffects.FlipHorizontally;
                    Player.State = Enums.CharacterStates.Running;
                }
                else if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                {
                    movement.X++;
                    Player.Direction = SpriteEffects.None;
                    Player.State = Enums.CharacterStates.Running;
                }
                else if(Player.State == Enums.CharacterStates.Dead)
                {
                    LockedMovement = true;
                }
                else
                {
                    Player.State = Enums.CharacterStates.Idle;
                }
            }

            //if (keyboardState.IsKeyDown(Keys.Up))
            //    movement.Y--;

            //if (keyboardState.IsKeyDown(Keys.Down))
            //    movement.Y++;

            var thumbstick = gamePadState.ThumbSticks.Left;

            movement.X += thumbstick.X;
            movement.Y -= thumbstick.Y;

            if (movement.Length() > 1)
                movement.Normalize();

            Player.Position += movement * 5f;
            #endregion
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            var spriteBatch = ScreenManager.SpriteBatch;

            Vector2 offset = Vector2.Zero;

            if(shakeIntensity > 0)
            {
                offset = new Vector2(
                    (float)(rand.NextDouble() * 2 - 1) * shakeIntensity,
                    (float)(rand.NextDouble() * 2 - 1) * shakeIntensity
                    );
            }
            Matrix shakeTransform = Matrix.CreateTranslation(offset.X, offset.Y, 0);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, shakeTransform);
            spriteBatch.Draw(backgrounds[1], new Rectangle(-20, -20, graphics.Viewport.Width + 40, graphics.Viewport.Height + 40), Color.White);
            spriteBatch.Draw(backgrounds[0], new Rectangle(-20, -20, graphics.Viewport.Width + 40, graphics.Viewport.Height + 40), Color.White);
            for (int i = 0; i < graphics.Viewport.Width; i = i + 63 * 3)
            {
                spriteBatch.Draw(torch, new Vector2(i, graphics.Viewport.Height - (63 - 15) * 3), null, Color.White, 0.0f, Vector2.Zero, 3.0f, SpriteEffects.None, 0.0f);
            }
            spriteBatch.End();

            spriteBatch.Begin(transformMatrix: shakeTransform);
            if (!beginGame)
            {
                spriteBatch.DrawString(algerian, $"Press Space Bar, Enter, or the A button to start", new Vector2((graphics.Viewport.Width - algerian.MeasureString("Press Space Bar, Enter, or the A button to start").X) / 2, 10), Color.OrangeRed);
            }

            spriteBatch.DrawString(algerian, $"Time survived: {gameTimer:F2}", Vector2.Zero, Color.OrangeRed);
            Player.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(caveGround, new Vector2(0, graphics.Viewport.Height - 32), new Rectangle(graphics.Viewport.Width, 0, graphics.Viewport.Width, 32), Color.White);
            spriteBatch.End();

            if (gameTimer > 20.0)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(algerian, $"You win!!! Exiting to menu", new Vector2((graphics.Viewport.Width - algerian.MeasureString("You win!!! Exiting to menu").X) / 2, graphics.Viewport.Height / 2), Color.Crimson);
                spriteBatch.End();
            }
        }

        public void EndGame()
        {
            _game.Components.Remove(rockslide);
            closeGame = true;
        }

        public void TriggerTremor(float i, float d)
        {
            shakeIntensity = i;
            shakeDuration = d;
        }
    }
}

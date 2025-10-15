using System;
using System.Collections.Generic;
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

namespace GameProject1.Screens
{
    public class RockslideMinigame : GameScreen
    {
        private double gameTimer = 0;
        private float winTime = 15;
        private bool beginGame = false;
        private bool closeGame = false;

        private ContentManager content;
        private readonly InputAction _pauseAction;
        private readonly InputAction _beginGame;
        private float pauseAlpha;
        private GraphicsDevice graphics;
        private RockslideParticleSystem rockslide;
        private Game _game;

        public BoundingRectangle PlayerHitbox { get; set; }
        public PlayerSprite Player;

        public RockslideMinigame(GraphicsDevice graphics, Game game)
        {
            _game = game;

            this.graphics = graphics;
            Player = new PlayerSprite() { Position = new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height - 60)};


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

            Particle[] particles = rockslide.GetParticles();
            for(int i = 0; i < particles.Length; i++)
            {
                if (Player.Bounds.CollidesWith(particles[i].bounds))
                {
                    Player.TakeDamage();
                }
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
                    gameTimer += gameTime.ElapsedGameTime.TotalSeconds;

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

            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                movement.X--;
                Player.Direction = SpriteEffects.FlipHorizontally;
            }

            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                movement.X++;
                Player.Direction = SpriteEffects.None;
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
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.DarkBlue, 0, 0);

            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            Player.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}

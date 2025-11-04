using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameProject1.Player;
using GameProject1.Saving;
using GameProject1.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace GameProject1.Screens
{
    public class TopDownScreen : GameScreen
    {
        private GraphicsDevice _graphics;
        private SpriteFont _font;
        private Game _game;
        private Tilemap _tilemap;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;
        private BowlingBallMan player;
        private Villian villian;
        private SaveData _saveData;
        private readonly InputAction _pauseAction;

        public TopDownScreen(GraphicsDevice graphics, Game game, SaveData save)
        {
            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
            _graphics = graphics;
            _game = game;
            _saveData = save;
            if(_saveData == null)
            {
                player = new BowlingBallMan();
            }
            else
            {
                player = new BowlingBallMan(new Vector2(_saveData.PlayerX, _saveData.PlayerY), _saveData.PlayerHealth);
            }
            villian = new Villian();
        }

        public void Initialize()
        {
            _tilemap = new Tilemap("tileset.txt");
        }

        public override void Activate()
        {
            _spriteBatch = new SpriteBatch(_graphics);
            _tilemap.LoadContent(_game.Content);
            player.LoadContent(_game.Content);
            villian.LoadContent(_game.Content);
            if (_content == null)
            {
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
                Thread.Sleep(1000);

                ScreenManager.Game.ResetElapsedTime();
            }

            _font = _content.Load<SpriteFont>("Fonts//Arial");

            base.Activate();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            player.Update(gameTime);
            foreach(Projectile p in player.Projectiles)
            {
                if(CollisionHelper.Collides(p.Bounds, villian.Bounds))
                {
                    villian.Health--;
                }

            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if(villian.Health <= 0)
            {
                _game.Exit();
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);
            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(_game, this.player), ControllingPlayer);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _tilemap.Draw(gameTime, _spriteBatch);
            _spriteBatch.DrawString(_font, "Press 'F' to Save your game, or use 'ESC' to navigate to the menu and save there", new Vector2(0, _graphics.Viewport.Height - 30), Color.White);
            player.Draw(gameTime, _spriteBatch);
            villian.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

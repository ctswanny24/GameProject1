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
        private Random random = new Random();
        private GraphicsDevice _graphics;
        private SpriteFont _font;
        private Game _game;
        private Tilemap _tilemap;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;
        private BowlingBallMan player;
        private List<Villian> _villians;
        private Villian villian;
        private SaveData _saveData;
        private readonly InputAction _pauseAction;
        private float endCounter = 0;

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
                player = new BowlingBallMan(_graphics);
            }
            else
            {
                player = new BowlingBallMan(new Vector2(_saveData.PlayerX, _saveData.PlayerY), _saveData.PlayerHealth, _graphics);
            }
            _villians = new List<Villian>();
            if(save == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    _villians.Add(new Villian(new Vector2(random.NextInt64(0, _graphics.Viewport.Width), random.NextInt64(0, _graphics.Viewport.Height - 48)), 1));
                }
            }
            else
            {
                foreach (Tuple<float, float, bool> i in save.Villians)
                {
                    _villians.Add(new Villian(new Vector2(i.Item1, i.Item2), 1) { Dead = i.Item3 });
                }
            }
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
            foreach(Villian v in _villians)
            {
                v.LoadContent(_game.Content);
            }
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
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                List<Tuple<float, float, bool>> villianInfo = new List<Tuple<float, float, bool>>();
                foreach (Villian v in _villians)
                    villianInfo.Add(new Tuple<float, float, bool>(v.Position.X, v.Position.Y, v.Dead));
                SaveStateManager.SaveGame(new SaveData(player.Position.X, player.Position.Y, player.Health, villianInfo));
            }


            player.Update(gameTime);
            foreach(Villian v in _villians)
            {
                foreach(Projectile p in player.Projectiles)
                {
                    if(CollisionHelper.Collides(p.Bounds, v.Bounds))
                    {
                        v.Dead = true;
                    } 
                }
            }
            List<Villian> toRemove = new List<Villian>();
            foreach (Villian v in _villians)
                if (v.Dead) toRemove.Add(v);
            foreach (Villian v in toRemove)
                _villians.Remove(v);

            if(_villians.Count == 0)
            {
                endCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(endCounter > 1.5)
                {
                    ExitScreen();
                    ScreenManager.AddScreen(new BackgroundScreen(), null);
                    ScreenManager.AddScreen(new MainMenuScreen(_game), null);

                }
                //for (int i = 0; i < 3; i++)
                //{
                //    _villians.Add(new Villian(new Vector2(random.NextInt64(0, _graphics.Viewport.Width), random.NextInt64(0, _graphics.Viewport.Height - 48)), 1));
                //}
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);
            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(_game, this.player, _villians), ControllingPlayer);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _tilemap.Draw(gameTime, _spriteBatch);
            _spriteBatch.DrawString(_font, "Press 'F' to Save your game, or use 'ESC' to navigate to the menu and save there", new Vector2(0, _graphics.Viewport.Height - 30), Color.White);
            player.Draw(gameTime, _spriteBatch);
            if(_villians.Count != 0)
            {
                foreach(Villian v in _villians)
                {
                    v.Draw(gameTime, _spriteBatch);
                }
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

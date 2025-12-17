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
    public enum ScoringType
    {
        Single = 1,
        HalfStrike = 5,
        Strike = 10,
        Double = 20,
        Turkey = 30,
        Hambone = 40,
    }

    public class TopDownScreen : GameScreen
    {
        private float shakeIntensity = 0f;
        private float shakeDuration = 0f;
        private float shakeInterval = 0f;

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
        private Cube powerup;
        private Texture2D wrappingTexture;
        private BasicEffect effect;
        public int EnemyWaveCount;
        public int EnemiesDefeated;
        public Song backgroundMusic;
        private SoundEffect bowlingPinSE;
        private Texture2D controlsTexture;

        public int Score = 0;
        public int HighestCombo = 0;

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
                player = new BowlingBallMan(_graphics) { Position = new Vector2(120, graphics.Viewport.Height / 2)};
                powerup = new Cube(game);
            }
            else
            {
                player = new BowlingBallMan(new Vector2(_saveData.PlayerX, _saveData.PlayerY), _saveData.PlayerHealth, _graphics);
                powerup = new Cube(game);
            }
            _villians = new List<Villian>();
            if(save == null)
            {
                EnemyWaveCount = 10;
                for (int i = 0; i < EnemyWaveCount; i++)
                {
                    var v = new Villian(_graphics.Viewport.Width - ((float)random.NextInt64(0, 10)), (int)random.NextInt64(0, 5), random);
                    v = CreateNoOverlapVillians(v);
                    _villians.Add(v);
                }
            }
            else
            {
                EnemyWaveCount = save.EnemyCount;
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
            powerup.LoadContent(_content, _graphics);
            backgroundMusic = _content.Load<Song>("Sounds//groovy-two-shoes-235244");
            bowlingPinSE = _content.Load<SoundEffect>("Sounds//bowling-strike-40456");
            controlsTexture = _content.Load<Texture2D>("Textures//Controls");
            MediaPlayer.Volume = 0.15f;
            MediaPlayer.Play(backgroundMusic);
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
                SaveStateManager.SaveGame(new SaveData(player.Position.X, player.Position.Y, player.Health, villianInfo, EnemyWaveCount));
            }


            player.Update(gameTime);

            if (shakeDuration > 0)
            {
                shakeDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (shakeDuration <= 0)
                {
                    shakeIntensity = 0f;
                }
            }

            foreach (Villian v in _villians)
            {
                v.Update(gameTime);
                foreach(ShotputProjectile p in player.ShotputProjectiles)
                {
                    if(p.IsFinished && !p.ComboScored)
                    {
                        CalcScore(p.EnemiesDefeated);
                        p.ComboScored = true;
                    }


                    if(p.HasLanded && p.HasLandedBefore)
                    {
                        TriggerTremor(2.0f, 0.1f);
                    }

                    if (CollisionHelper.Collides(p.Bounds, v.Bounds) && v.Dead != true)
                    {
                        p.EnemiesDefeated++;
                        EnemiesDefeated++;
                        v.Dead = true;
                        if (p.EnemiesDefeated >= 5 && !p.SoundPlaying)
                        {
                            bowlingPinSE.Play();
                            p.SoundPlaying = true;
                        }

                    }
                }
                foreach(Projectile p in player.Projectiles)
                {
                    if (p.Offscreen && !p.ComboScored)
                    {
                        CalcScore(p.EnemiesDestroyed);
                        p.ComboScored = true;
                    }
                    if (CollisionHelper.Collides(p.Bounds, v.Bounds) && v.Dead != true)
                    {
                        EnemiesDefeated++;
                        p.EnemiesDestroyed++;
                        v.Dead = true;
                        if(p.EnemiesDestroyed >= 5 && !p.SoundPlaying)
                        {
                            bowlingPinSE.Play();
                            p.SoundPlaying = true;
                        }
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
                //endCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
                //if(endCounter > 1.5)
                //{
                //    ExitScreen();
                //    ScreenManager.AddScreen(new BackgroundScreen(), null);
                //    ScreenManager.AddScreen(new MainMenuScreen(_game), null);

                //}
                EnemyWaveCount += 10;

                for (int i = 0; i < EnemyWaveCount; i++)
                {
                    var v = new Villian(_graphics.Viewport.Width + ((float)random.NextInt64(0, 50)), (int)random.NextInt64(0, 5), random);
                    v = CreateNoOverlapVillians(v);
                    _villians.Add(v);
                    v.LoadContent(_content);

                }
            }
            powerup.Update(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);
            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(_game, this.player, _villians, this), ControllingPlayer);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            Vector2 offset = Vector2.Zero;


            if(shakeIntensity > 0)
            {
                offset = new Vector2(
                      (float)(random.NextDouble() * 2 - 1) * shakeIntensity,
                      (float)(random.NextDouble() * 2 - 1) * shakeIntensity
                      );
                }
                Matrix shakeTransform = Matrix.CreateTranslation(offset.X, offset.Y, 0);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, shakeTransform);

            _tilemap.Draw(gameTime, _spriteBatch);
            _spriteBatch.DrawString(_font, $"Score: {Score}", new Vector2(10, _graphics.Viewport.Height - 30), Color.White);
            _spriteBatch.DrawString(_font, $"Highest Combo {HighestCombo}", new Vector2(10, _graphics.Viewport.Height - 50), Color.White);
            player.Draw(gameTime, _spriteBatch);
            if(_villians.Count != 0)
            {
                foreach(Villian v in _villians)
                {
                    v.Draw(gameTime, _spriteBatch);
                }
            }

            //if(EnemiesDefeated >= 3 && EnemiesDefeated < 15)
            //{
            //    _spriteBatch.DrawString(_font, $"Behold, the cube of (eventual) POWER (true functionality and powerup to be implemented later)", new Vector2(0, 0), Color.Gold);
            //}
            
            _spriteBatch.End();



            _spriteBatch.Begin();
            
            _spriteBatch.Draw(controlsTexture, Vector2.Zero, Color.White);

            _spriteBatch.End();
            
            //if(EnemiesDefeated >= 3 && EnemiesDefeated < 150)
            //{
            //    powerup.Draw();
            //}

            base.Draw(gameTime);            

        }

        public Villian CreateNoOverlapVillians(Villian v)
        {
            foreach (Villian vil in _villians)
            {
                while (CollisionHelper.Collides(vil.Bounds, v.Bounds))
                {
                    v = new Villian(_graphics.Viewport.Width + ((float)random.NextInt64(0, 100)), (int)random.NextInt64(0, 5), random);
                }
            }
            return v;
        }

        public void CalcScore(int hits)
        {
            if(hits > HighestCombo)
            {
                HighestCombo = hits;
            }

            if (hits <= 0)
                return;
            Score += hits;
            if (hits >= (int)ScoringType.Hambone)
                Score += 160;
            else if (hits >= (int)ScoringType.Turkey)
                Score += 80;
            else if (hits >= (int)ScoringType.Double)
                Score += 40;
            else if (hits >= (int)ScoringType.Strike)
                Score += 20;
            else if (hits >= (int)ScoringType.HalfStrike)
                Score += 10;
        }

        public void TriggerTremor(float i, float d)
        {
            shakeIntensity = i;
            shakeDuration = d;
        }

    }
}

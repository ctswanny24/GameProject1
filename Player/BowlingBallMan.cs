using GameProject1.Saving;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject1.Player
{
    public class BowlingBallMan
    {
        private KeyboardState _prevKeyboardState;
        private KeyboardState _keyboardState;
        private MouseState _mouseState;
        private MouseState _prevMouseState;
        private int _spriteWidth;
        private int _spriteHeight;
        private int _frameHeight;

        public Vector2 Position;
        public BoundingRectangle HitBox;
        public List<Texture2D> Sprites;
        public List<Projectile> Projectiles;
        public int Health;

        public ContentManager Content;
        public float chargeTime;

        public BowlingBallMan()
        {
            Sprites = new List<Texture2D>();
            Projectiles = new List<Projectile>();
            Health = 3;
        }

        public BowlingBallMan(Vector2 position, int health)
        {
            Sprites = new List<Texture2D>();
            Projectiles = new List<Projectile>();
            Position = position;
            Health = health;
        }

        public void LoadContent(ContentManager content)
        {
            Content = content;
            Sprites.Add(content.Load<Texture2D>("Textures//BarryBowlingBall"));
            Sprites.Add(content.Load<Texture2D>("Textures//CaveAssets//Items//000_0060_heart6"));
        }

        public void Update(GameTime gameTime)
        {
            HandleInput();
            foreach(Projectile p in Projectiles)
            {
                p.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprites[0], Position, Color.White);
            foreach(Projectile p in Projectiles)
            {
                p.Draw(gameTime, spriteBatch);
            }
            //for(int i = Health; i > 0; i--)
            //{
            //    spriteBatch.Draw(Sprites[1], new Vector2(Health * 25, 0), Color.White);
            //}
        }

        private void HandleInput()
        {
            _prevMouseState = _mouseState;
            _mouseState = Mouse.GetState();
            _prevKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();

            SimpleInput(_keyboardState);
        }

        private void SimpleInput(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.A))
            {
                Position += new Vector2(-5, 0);
            }
            if (state.IsKeyDown(Keys.D))
            {
                Position += new Vector2(5, 0);
            }
            if (state.IsKeyDown(Keys.W))
            {
                Position += new Vector2(0, -5);
            }
            if (state.IsKeyDown(Keys.S))
            {
                Position += new Vector2(0, 5);
            }
            if (state.IsKeyDown(Keys.F))
            {
                SaveStateManager.SaveGame(new SaveData(Position.X, Position.Y, Health));
            }
            if ((_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton != ButtonState.Pressed) || (state.IsKeyDown(Keys.Space) && !_prevKeyboardState.IsKeyDown(Keys.Space)))
            {
                Projectiles.Add(new Projectile(Position, Content));
            }
        }
    }
}

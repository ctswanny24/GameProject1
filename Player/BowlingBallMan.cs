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
        private int _spriteWidth = 48;
        private int _spriteHeight = 48;
        private GraphicsDevice _graphics;

        public Vector2 Position;
        public BoundingRectangle Bounds;
        public List<Texture2D> Sprites;
        public List<Projectile> Projectiles;
        public int Health;

        public ContentManager Content;
        public float chargeTime;

        public BowlingBallMan(GraphicsDevice graphics)
        {
            _graphics = graphics;
            Sprites = new List<Texture2D>();
            Projectiles = new List<Projectile>();
            Health = 3;
            Bounds = new BoundingRectangle(new Vector2(Position.X, Position.Y), _spriteWidth, _spriteHeight);
        }

        public BowlingBallMan(Vector2 position, int health, GraphicsDevice graphics)
        {
            _graphics = graphics;
            Sprites = new List<Texture2D>();
            Projectiles = new List<Projectile>();
            Position = position;
            Health = health;
            Bounds = new BoundingRectangle(new Vector2(Position.X, Position.Y), _spriteWidth, _spriteHeight);

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
            List<Projectile> toRemove = new List<Projectile>();
            foreach(Projectile p in Projectiles)
            {
                p.Update(gameTime);
                if(p.Position.X > _graphics.Viewport.Width + 48)
                {
                    toRemove.Add(p);
                    break;
                }
            }
            foreach(Projectile p in toRemove)
            {
                Projectiles.Remove(p);
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
            if (state.IsKeyDown(Keys.A) && Position.X > 0)
            {
                Position += new Vector2(-5, 0);
            }
            if (state.IsKeyDown(Keys.D) && Position.X < _graphics.Viewport.Width - 48)
            {
                Position += new Vector2(5, 0);
            }
            if (state.IsKeyDown(Keys.W) && Position.Y > 0)
            {
                Position += new Vector2(0, -5);
            }
            if (state.IsKeyDown(Keys.S) && Position.Y < _graphics.Viewport.Height - 48)
            {
                Position += new Vector2(0, 5);
            }

            if ((_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton != ButtonState.Pressed) || (state.IsKeyDown(Keys.Space) && !_prevKeyboardState.IsKeyDown(Keys.Space)))
            {
                Projectiles.Add(new Projectile(Position, Content));
            }
        }
    }
}

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

namespace GameProject1
{
    public class Villian
    {
        private KeyboardState _prevKeyboardState;
        private KeyboardState _keyboardState;
        private MouseState _mouseState;
        private MouseState _prevMouseState;
        private int _spriteWidth = 32;
        private int _spriteHeight = 48;

        public Vector2 Position;
        public BoundingRectangle Bounds;
        public Texture2D Texture;
        public int Health;
        public float Speed;

        public ContentManager Content;
        public float chargeTime;
        public bool Dead;

        public Villian()
        {
            Position = new Vector2(300, 300);
            Health = 3;
            Bounds = new BoundingRectangle(new Vector2(Position.X, Position.Y), _spriteWidth, _spriteHeight);
            Dead = false;
        }

        public Villian(Vector2 position, int health)
        {
            Position = position;
            Health = health;
            Bounds = new BoundingRectangle(new Vector2(Position.X, Position.Y), _spriteWidth, _spriteHeight);
            Dead = false;

        }

        public Villian(float posX, int lane, Random r)
        {
            float laneY = 0;
            switch (lane)
            {
                case 0:
                    laneY = (float)r.NextInt64((128 * lane) + 10, (128 * (lane + 1)) - 10);
                    break;
                case 1:
                    laneY = (float)r.NextInt64((128 * lane) + 10, (128 * (lane + 1)) - 10);
                    break;
                case 2:
                    laneY = (float)r.NextInt64((128 * lane) + 10, (128 * (lane + 1)) - 10);
                    break;
                case 3:
                    laneY = (float)r.NextInt64((128 * lane) + 10, (128 * (lane + 1)) - 90);
                    break;
            }
            Position = new Vector2(posX, laneY);
            Health = 1;
            Bounds = new BoundingRectangle(new Vector2(Position.X, Position.Y), _spriteWidth, _spriteHeight);
            Dead = false;
            Speed = ((float)r.NextInt64(3, 7) / 10);
        }

        public void LoadContent(ContentManager content)
        {
            Content = content;
            Texture = content.Load<Texture2D>("Textures//EvilBowlingPin");
        }

        public void Update(GameTime gameTime)
        {
            if(Position.X >= 225)
            {
                Vector2 movement = new Vector2(Speed, 0);
                Position -= movement;
            }
            Bounds.X = Position.X;
            Bounds.Y = Position.Y;

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(Texture != null)
                spriteBatch.Draw(Texture, Position, Color.White);
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
        }
    }
}

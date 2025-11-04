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
        private int _spriteWidth = 48;
        private int _spriteHeight = 48;

        public Vector2 Position;
        public BoundingRectangle Bounds;
        public List<Texture2D> Sprites;
        public int Health;

        public ContentManager Content;
        public float chargeTime;

        public Villian()
        {
            Sprites = new List<Texture2D>();
            Position = new Vector2(300, 300);
            Health = 3;
            Bounds = new BoundingRectangle(new Vector2(Position.X, Position.Y), _spriteWidth, _spriteHeight);
        }

        public Villian(Vector2 position, int health)
        {
            Sprites = new List<Texture2D>();
            Position = position;
            Health = health;
            Bounds = new BoundingRectangle(new Vector2(Position.X, Position.Y), _spriteWidth, _spriteHeight);

        }

        public void LoadContent(ContentManager content)
        {
            Content = content;
            Sprites.Add(content.Load<Texture2D>("Textures//EvilBowlingPin"));
        }

        public void Update(GameTime gameTime)
        {
            HandleInput();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprites[0], Position, Color.White);
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
        }
    }
}

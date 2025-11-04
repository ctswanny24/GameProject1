using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject1.Sprites
{
    public class Shotput
    {
        private Texture2D texture;
        private KeyboardState _prevKeyboardState;
        private KeyboardState _keyboardState;

        public Vector2 Velocity;
        public Vector2 Position;
        public BoundingCircle HitBox;
        public float ChargeTime;

        public Shotput(Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
            HitBox = new BoundingCircle();
            ChargeTime = 0;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Textures//shotput");
        }

        public void Update(GameTime gameTime)
        {
            _prevKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 acceleration = new Vector2(10, 50);
            if (_keyboardState.IsKeyDown(Keys.Space) && _prevKeyboardState.IsKeyDown(Keys.Space))
            {
                if(ChargeTime < 2.0f)
                    ChargeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if(_keyboardState.IsKeyUp(Keys.Space) && _prevKeyboardState.IsKeyDown(Keys.Space))
            {
                Velocity += acceleration * ChargeTime;
                Position += Velocity * ChargeTime;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}

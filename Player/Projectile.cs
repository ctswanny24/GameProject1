using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject1.Player
{
    public class Projectile
    {
        public Texture2D Sprite { get; set; }
        public Vector2 Position { get; set; }
        public BoundingRectangle Bounds { get; set; }

        private int _spriteWidth = 48;
        private int _spriteHeight = 28;

        public Projectile(Vector2 startingPos, ContentManager content)
        {
            Position = startingPos + new Vector2(0,17);
            Sprite = content.Load<Texture2D>("Textures//ProjectileMan");
            Bounds = new BoundingRectangle(new Vector2(Position.X - (_spriteWidth / 2), Position.Y - (_spriteHeight / 2)), _spriteWidth, _spriteHeight);

        }

        public void LoadContent(ContentManager content)
        {
        }

        public void Update(GameTime gameTime)
        {
            Position += new Vector2(7, 0);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, Color.White);
        }
    }
}

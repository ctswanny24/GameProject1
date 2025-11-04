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
        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;

        private int _spriteWidth = 48;
        private int _spriteHeight = 28;

        public Projectile(Vector2 startingPos, ContentManager content)
        {
            Position = startingPos + new Vector2(0,17);
            Sprite = content.Load<Texture2D>("Textures//ProjectileMan");
            bounds = new BoundingRectangle(new Vector2(Position.X, Position.Y), _spriteWidth, _spriteHeight);

        }

        public void LoadContent(ContentManager content)
        {
        }

        public void Update(GameTime gameTime)
        {
            Position += new Vector2(7, 0);
            bounds.X = Position.X;
            bounds.Y = Position.Y;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, Color.White);
        }
    }
}

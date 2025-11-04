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
        public BoundingRectangle Hitbox { get; set; }

        public Projectile(Vector2 startingPos, ContentManager content)
        {
            Position = startingPos + new Vector2(0,17);
            Sprite = content.Load<Texture2D>("Textures//ProjectileMan");
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

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameProject1.Player
{
    public class Turret
    {
        float lifetime = 10.0f;
        public float Alive = 0.0f;
        float shotInterval = 0.0f;
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

        public Turret(GraphicsDevice graphics)
        {
        }

        public void LoadContent(ContentManager content)
        {
            Content = content;
            Sprites.Add(content.Load<Texture2D>("Textures//Turret"));
        }

        public void Update(GameTime gameTime)
        {
            Alive += (float)gameTime.ElapsedGameTime.TotalSeconds;
            shotInterval += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Alive < lifetime && shotInterval == 1.0f)
            {
                Projectiles.Add(new Projectile(Position, Content));
                shotInterval = shotInterval - 1.0f;
            }
            List<Projectile> toRemove = new List<Projectile>();
            foreach (Projectile p in Projectiles)
            {
                p.Update(gameTime);
                if (p.Position.X > _graphics.Viewport.Width + 48)
                {
                    toRemove.Add(p);
                    break;
                }
            }
            foreach (Projectile p in toRemove)
            {
                Projectiles.Remove(p);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprites[0], Position, Color.White);
            foreach (Projectile p in Projectiles)
            {
                p.Draw(gameTime, spriteBatch);
            }
            //for(int i = Health; i > 0; i--)
            //{
            //    spriteBatch.Draw(Sprites[1], new Vector2(Health * 25, 0), Color.White);
            //}
        }
    }
}

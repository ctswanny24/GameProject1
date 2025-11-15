using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
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
        public Song Sound { get; set; }
        private BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;

        private int _spriteWidth = 48;
        private int _spriteHeight = 28;

        public int EnemiesDestroyed = 0;

        public Projectile(Vector2 startingPos, ContentManager content)
        {
            Position = startingPos + new Vector2(0,17);
            Sprite = content.Load<Texture2D>("Textures//ProjectileMan");
            //Sound = content.Load<Song>("Sounds//man-scream-08-352438");
            bounds = new BoundingRectangle(new Vector2(Position.X, Position.Y), _spriteWidth, _spriteHeight);
            //MediaPlayer.Volume = 0.25f;
            //MediaPlayer.Play(Sound);
        }

        public void LoadContent(ContentManager content)
        {
        }

        public void Update(GameTime gameTime)
        {
            //MediaPlayer.Volume = MediaPlayer.Volume - 0.003f;
            Position += new Vector2(6, 0);
            bounds.X = Position.X;
            bounds.Y = Position.Y;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, Color.White);
        }
    }
}

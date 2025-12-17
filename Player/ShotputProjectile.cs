using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SharpDX.Multimedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject1.Player
{
    public class ShotputProjectile
    {
        public Vector2 Position;
        private Vector2 velocity;
        private float gravity;

        private float timeAlive;
        private float totalFlightTime;
        private float startY;
        private float endX;

        private Texture2D projectileTexture;
        private Texture2D shockwaveTexture;

        private float radius = 100.0f;
        public BoundingCircle Bounds;

        public int EnemiesDefeated = 0;

        public bool IsFinished => timeAlive >= totalFlightTime + 0.25;
        public bool HasLandedBefore = false;
        public bool HasLanded = false;

        private float shockwaveTimer = 0.0f;
        private float shockwaveTime = 0.5f;

        public SoundEffect ScreamingSound;
        public SoundEffect ThudSound;

        public bool SoundPlaying = false;
        private SoundEffectInstance i;
        private SoundEffectInstance i2;

        public bool ComboScored = false;

        public ShotputProjectile(Vector2 startingPos, float horizDistance, float peakHeight, float flightTime, ContentManager content)
        {
            Position = startingPos;
            startY = Position.Y;

            totalFlightTime = flightTime;
            gravity = (2f * peakHeight) / (flightTime * flightTime) * 4f;

            float vx = horizDistance / flightTime;

            float vy = -gravity * flightTime / 2f;

            velocity = new Vector2(vx, vy);

            projectileTexture = content.Load<Texture2D>("Textures//SecondaryFire");
            shockwaveTexture = content.Load<Texture2D>("Textures//Shockwave");
            ScreamingSound = content.Load<SoundEffect>("Sounds//falling-man-scream-450793");
            ThudSound = content.Load<SoundEffect>("Sounds//8bit_bomb_explosion");

            i2 = ThudSound.CreateInstance();
            i2.Volume = 0.4f;

            i = ScreamingSound.CreateInstance();
            i.Volume = 0.3f;
            i.Play();

            Bounds = new BoundingCircle(new Vector2(Position.X + (projectileTexture.Width / 2), Position.Y + (projectileTexture.Height / 2)), projectileTexture.Width/2);

        }

        public void Update(GameTime gameTime)
        {
            if (i.Volume >= 0.005f)
            {
                i.Volume -= 0.005f;
            }

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeAlive += dt;

            if(timeAlive <= totalFlightTime)
            {
                velocity.Y += gravity * dt;
                Position += velocity * dt;
            }

            if(timeAlive >= totalFlightTime)
            {
                HasLanded = true;
                shockwaveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if(HasLanded == true && HasLandedBefore == false)
            {
                Bounds = new BoundingCircle(new Vector2(Position.X + projectileTexture.Width / 2f, Position.Y + projectileTexture.Height / 2f), radius);
                i.Stop();
                i2.Play();
                HasLandedBefore = true;
            }
            else if(timeAlive >= totalFlightTime * .66f || timeAlive <= totalFlightTime * .33f)
            {
                Bounds = new BoundingCircle(new Vector2(Position.X + (projectileTexture.Width / 2), Position.Y + (projectileTexture.Height / 2)), projectileTexture.Width / 2); ;
            }

            // Clamp final Y to starting Y
            if (timeAlive >= totalFlightTime)
            {
                Position.Y = startY;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(projectileTexture, Position, Color.White);
            if(HasLanded == true && shockwaveTimer <= shockwaveTime)
            {
                spriteBatch.Draw(
                        shockwaveTexture,
                        Bounds.Center,
                        null,
                        Color.White,
                        0f,
                        new Vector2(
                            shockwaveTexture.Width / 2f,
                            shockwaveTexture.Height / 2f
                        ),
                        (radius * 2f) / shockwaveTexture.Width,
                        SpriteEffects.None,
                        0f
                    );
            }
        }
    }
}

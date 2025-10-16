using Microsoft.Xna.Framework;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject1.ParticleSystems
{
    public class RockslideParticleSystem : ParticleSystem
    {
        Rectangle _source;

        public bool Occurring { get; set; } = false;

        public RockslideParticleSystem(Game game, Rectangle source) : base(game, 20)
        {
            _source = source;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "boulder";
            minNumParticles = 5;
            maxNumParticles = 15;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            Vector2 position = where;
            Vector2 velocity = Vector2.UnitY * 200;
            Vector2 acceleration = Vector2.Zero;
            Color color = Color.White;
            float scale = 1.5f;
            float rotation = 0f;
            float angularVelocity = 2f;
            float angularAccel = 0f;

            p.Initialize(position, velocity, acceleration, color, lifetime: 4.0f, scale, rotation, angularVelocity: angularVelocity, angularAcceleration: angularAccel);
            SetBoundingCircles(16 * scale, 16 * scale);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateBoundingPosition(16);
            if (Occurring)
            {
                AddParticles(_source);
                ResumeParticles(Vector2.UnitY * 150, 6);
            }
            if (!Occurring) StopParticles();
        }

        public Particle[] GetParticles()
        {
            return GetParticlesArray();
        }

    }
}

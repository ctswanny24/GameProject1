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

        public RockslideParticleSystem(Game game, Rectangle source) : base(game, 25)
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
            p.Initialize(where, Vector2.UnitY * 150, Vector2.Zero, Color.White, scale: 1.0f, rotation: p.Rotation, lifetime: 4f);
            SetBoundingCircles(16, 16);
            p.Rotation += (float)(Math.PI/6);
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

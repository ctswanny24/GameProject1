using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace GameProject1
{
    public class Cube
    {
        VertexBuffer vertices;

        IndexBuffer indices;

        BasicEffect effect;

        Texture2D wrappingTexture;

        Game game;

        public Cube(Game game)
        {
            this.game = game;
            InitializeVertices();
            InitializeIndices();
            InitializeEffect();
        }

        public void InitializeVertices()
        {
            var vertexData = new VertexPositionNormalTexture[] {
            // FRONT FACE (-Z)
            new VertexPositionNormalTexture(new Vector3(-1,  1, -1), Vector3.Forward, new Vector2(0,0)),
            new VertexPositionNormalTexture(new Vector3( 1,  1, -1), Vector3.Forward, new Vector2(1,0)),
            new VertexPositionNormalTexture(new Vector3(-1, -1, -1), Vector3.Forward, new Vector2(0,1)),
            new VertexPositionNormalTexture(new Vector3( 1, -1, -1), Vector3.Forward, new Vector2(1,1)),

            // BACK FACE (+Z)
            new VertexPositionNormalTexture(new Vector3( 1,  1,  1), Vector3.Backward, new Vector2(0,0)),
            new VertexPositionNormalTexture(new Vector3(-1,  1,  1), Vector3.Backward, new Vector2(1,0)),
            new VertexPositionNormalTexture(new Vector3( 1, -1,  1), Vector3.Backward, new Vector2(0,1)),
            new VertexPositionNormalTexture(new Vector3(-1, -1,  1), Vector3.Backward, new Vector2(1,1)),

            // LEFT FACE (-X)
            new VertexPositionNormalTexture(new Vector3(-1,  1,  1), Vector3.Left, new Vector2(0,0)),
            new VertexPositionNormalTexture(new Vector3(-1,  1, -1), Vector3.Left, new Vector2(1,0)),
            new VertexPositionNormalTexture(new Vector3(-1, -1,  1), Vector3.Left, new Vector2(0,1)),
            new VertexPositionNormalTexture(new Vector3(-1, -1, -1), Vector3.Left, new Vector2(1,1)),

            // RIGHT FACE (+X)
            new VertexPositionNormalTexture(new Vector3( 1,  1, -1), Vector3.Right, new Vector2(0,0)),
            new VertexPositionNormalTexture(new Vector3( 1,  1,  1), Vector3.Right, new Vector2(1,0)),
            new VertexPositionNormalTexture(new Vector3( 1, -1, -1), Vector3.Right, new Vector2(0,1)),
            new VertexPositionNormalTexture(new Vector3( 1, -1,  1), Vector3.Right, new Vector2(1,1)),

            // TOP FACE (+Y)
            new VertexPositionNormalTexture(new Vector3(-1,  1,  1), Vector3.Up, new Vector2(0,0)),
            new VertexPositionNormalTexture(new Vector3( 1,  1,  1), Vector3.Up, new Vector2(1,0)),
            new VertexPositionNormalTexture(new Vector3(-1,  1, -1), Vector3.Up, new Vector2(0,1)),
            new VertexPositionNormalTexture(new Vector3( 1,  1, -1), Vector3.Up, new Vector2(1,1)),

            // BOTTOM FACE (-Y)
            new VertexPositionNormalTexture(new Vector3(-1, -1, -1), Vector3.Down, new Vector2(0,0)),
            new VertexPositionNormalTexture(new Vector3( 1, -1, -1), Vector3.Down, new Vector2(1,0)),
            new VertexPositionNormalTexture(new Vector3(-1, -1,  1), Vector3.Down, new Vector2(0,1)),
            new VertexPositionNormalTexture(new Vector3( 1, -1,  1), Vector3.Down, new Vector2(1,1)),
        };
            vertices = new VertexBuffer(
                game.GraphicsDevice,             
                typeof(VertexPositionNormalTexture),     
                24,                              
                BufferUsage.None                
            );
            vertices.SetData<VertexPositionNormalTexture>(vertexData);
        }

        public void InitializeIndices()
        {
            var indexData = new short[]
            {
                // FRONT FACE (-Z)
        0, 2, 1,
        2, 3, 1,

        // BACK FACE (+Z)
        4, 6, 5,
        6, 7, 5,

        // LEFT FACE (-X)
        8, 10, 9,
        10, 11, 9,

        // RIGHT FACE (+X)
        12, 14, 13,
        14, 15, 13,

        // TOP FACE (+Y)
        16, 18, 17,
        18, 19, 17,

        // BOTTOM FACE (-Y)
        20, 22, 21,
        22, 23, 21
            //0, 1, 2,
            //2, 1, 3,
            //4, 0, 6, 
            //6, 0, 2,
            //7, 5, 6, 
            //6, 5, 4,
            //3, 1, 7, 
            //7, 1, 5,
            //4, 5, 0, 
            //0, 5, 1,
            //3, 7, 2,  
            //2, 7, 6
            };
            indices = new IndexBuffer(
                game.GraphicsDevice,            
                IndexElementSize.SixteenBits,    
                36,                             
                BufferUsage.None               
            );
            indices.SetData<short>(indexData);
        }

        public void LoadContent(ContentManager content, GraphicsDevice graphics)
        {

            wrappingTexture = content.Load<Texture2D>("Textures//Present_Texture");
            effect.Texture = wrappingTexture;

        }

        void InitializeEffect()
        {
            effect = new BasicEffect(game.GraphicsDevice);
            float scaleFactor = 0.25f;
            effect.World = Matrix.CreateScale(scaleFactor);
            //effect.World = Matrix.Identity;
            effect.View = Matrix.CreateLookAt(
                new Vector3(0, 0, 4), 
                new Vector3(0, 0, 0), 
                Vector3.Up            
            );
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,                          
                game.GraphicsDevice.Viewport.AspectRatio,   
                0.1f,  
                100.0f 
            );

            effect.TextureEnabled = true;
            effect.LightingEnabled = true;
            effect.EnableDefaultLighting();
        }

        public void Update(GameTime gameTime)
        {
            float angle = (float)gameTime.TotalGameTime.TotalSeconds;

            Vector3 position = new Vector3(-1, 0, -2);
            effect.World = Matrix.CreateScale(0.25f);

            effect.View = Matrix.CreateRotationY(angle) * Matrix.CreateLookAt(
                new Vector3(0, 5, -10),
                Vector3.Zero,
                Vector3.Up
            );
        }

        public void Draw()
        {
            game.GraphicsDevice.SetVertexBuffer(vertices);

            game.GraphicsDevice.Indices = indices;

            foreach( var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }
        
            game.GraphicsDevice.DrawIndexedPrimitives(
                PrimitiveType.TriangleList,
                0,                          
                0,                          
                12                          
            );
        }
    }
}

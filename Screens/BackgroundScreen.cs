using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameProject1.StateManagement;
using System.Reflection.Metadata;
using SharpDX.Direct2D1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using Microsoft.Xna.Framework.Media;

namespace GameProject1.Screens
{
    // The background screen sits behind all the other menu screens.
    // It draws a background image that remains fixed in place regardless
    // of whatever transitions the screens on top of it may be doing.
    public class BackgroundScreen : GameScreen
    {
        private ContentManager _content;
        private Texture2D[] backgrounds = new Texture2D[3];
        private float offsetX = 0;
        private float backgroundPos = 0;
        private Song backgroundMusic; 

        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, whereas if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            backgrounds[0] = _content.Load<Texture2D>("OakWoodsAssets//oak_woods_v1.0//background//background_layer_1");
            backgrounds[1] = _content.Load<Texture2D>("OakWoodsAssets//oak_woods_v1.0//background//background_layer_2");
            backgrounds[2] = _content.Load<Texture2D>("OakWoodsAssets//oak_woods_v1.0//background//background_layer_3");

            backgroundMusic = _content.Load<Song>("Hawkin - Woods");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
            //_backgroundTexture = _content.Load<Texture2D>("background");
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // Unlike most screens, this should not transition off even if
        // it has been covered by another screen: it is supposed to be
        // covered, after all! This overload forces the coveredByOtherScreen
        // parameter to false in order to stop the base Update method wanting to transition off.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            backgroundPos++;
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            var viewport = ScreenManager.GraphicsDevice.Viewport;
            float backgroundLen = (296.5f * 2.7f);
            var fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            //spriteBatch.Begin();
            
            backgroundPos = MathHelper.Clamp(backgroundPos, -1920 * 3, 1000);
            offsetX = 0 - backgroundPos;

            Matrix transform = Matrix.CreateTranslation(offsetX, 0, 0);

            // TODO: Add your drawing code here

            //Background
            transform = Matrix.CreateTranslation(offsetX * 0.333f, 0, 0);
            spriteBatch.Begin(transformMatrix: transform);
            for(int i = 0; i < 6; i++)
            {
                fullscreen = new Rectangle((int)(i * backgroundLen), 0, viewport.Width, viewport.Height);
                spriteBatch.Draw(backgrounds[0], fullscreen,
                    new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            }
            spriteBatch.End();

            transform = Matrix.CreateTranslation(offsetX * 0.666f, 0, 0);
            spriteBatch.Begin(transformMatrix: transform);
            for (int i = 0; i < 6; i++)
            {
                fullscreen = new Rectangle((int)(i * backgroundLen), 0, viewport.Width, viewport.Height);
                spriteBatch.Draw(backgrounds[1], fullscreen,
                    new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            }
            spriteBatch.End();

            transform = Matrix.CreateTranslation(offsetX, 0, 0);
            spriteBatch.Begin(transformMatrix: transform);
            for (int i = 0; i < 6; i++)
            {
                fullscreen = new Rectangle((int)(i * backgroundLen), 0, viewport.Width, viewport.Height);
                spriteBatch.Draw(backgrounds[2], fullscreen,
                    new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            }
            spriteBatch.End();


            //spriteBatch.End();
        }
    }
}

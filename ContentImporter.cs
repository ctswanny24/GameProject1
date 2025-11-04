using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject1
{
    public static class ContentImporter
    {
        public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        public static Dictionary<string, SoundEffect> SoundEffects = new Dictionary<string, SoundEffect>();
        public static Dictionary<string, Song> Songs = new Dictionary<string, Song>();
        public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();

        public static void LoadAll(ContentManager content, string contentRoot)
        {
            foreach(string file in Directory.GetFiles(contentRoot, "*.*", SearchOption.AllDirectories))
            {
                if (Path.GetExtension(file) != ".xnb") continue;

                string assetName = file.Substring(contentRoot.Length + 1);
                assetName = assetName.Replace("\\", "/");
                assetName = Path.ChangeExtension(assetName, null);

                try
                {
                    if (assetName.Contains("Textures"))
                    {
                        Textures[assetName] = content.Load<Texture2D>(assetName);
                    }
                    else if (assetName.Contains("Sounds"))
                    {
                        SoundEffects[assetName] = content.Load<SoundEffect>(assetName);
                    }
                    else if (assetName.Contains("Music"))
                    {
                        Songs[assetName] = content.Load<Song>(assetName);
                    }
                    else if (assetName.Contains("Fonts"))
                    {
                        Fonts[assetName] = content.Load<SpriteFont>(assetName);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Failed to load {assetName}: {ex.Message}");
                }
            }
        }
    }
}

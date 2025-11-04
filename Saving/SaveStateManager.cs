using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace GameProject1.Saving
{
    public static class SaveStateManager
    {
        public static readonly string SaveFile = "savefile.json";

        public static void SaveGame(SaveData data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(SaveFile, json);
        }

        public static SaveData LoadGame()
        {
            if (!File.Exists(SaveFile))
                return null;

            string json = File.ReadAllText(SaveFile);
            return JsonSerializer.Deserialize<SaveData>(json);
        }
    }
}

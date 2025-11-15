using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameProject1.Saving
{
    public class SaveData
    {
        public float PlayerX { get; set; }
        public float PlayerY { get; set; }

        public List<Tuple<float, float, bool>> Villians { get; set; }

        public int PlayerHealth { get; set; }

        public int EnemyCount { get; set; }

        public SaveData(float PlayerX, float PlayerY, int PlayerHealth, List<Tuple<float, float, bool>> villians, int enemyCount)
        {
            this.PlayerX = PlayerX;
            this.PlayerY = PlayerY;
            this.PlayerHealth = PlayerHealth;
            Villians = villians;
            EnemyCount = enemyCount;
        }
    }
}

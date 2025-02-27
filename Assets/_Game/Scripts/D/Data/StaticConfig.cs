using System;

namespace D
{
    [Serializable]
    public class StaticConfig
    {
        // Gold Config
        public int GOLD_PER_WAVE = 5;
        public int GOLD_PER_KILL = 1;
        public int GOLD_PER_FLOOR = 10;

        // Player Config
        public StatDictionary playerStats = new StatDictionary()
        {
            {StatType.Health, 100},
            {StatType.Damage, 10},
            {StatType.AttackSpeed, 1},
            {StatType.Speed, 5},
            {StatType.CritChance, 0},
            {StatType.CritDamage, 0},
            {StatType.Armor, 0},
        };

        public float SHOP_APPERANCE_RATE = 0.3f;

        public int STAT_POINTS_PER_LEVEL = 1;

        public int STAT_POINTS_PER_FLOOR = 10;
    }
}

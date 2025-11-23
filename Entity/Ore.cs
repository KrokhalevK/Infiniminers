using System;
using System.Drawing;

namespace Infiniminers
{
    /// <summary>
    /// Класс руды с координатами, прочностью и типом.
    /// </summary>
    public class Ore
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Size { get; set; }
        public OreType Type { get; private set; }
        public OreData Data { get; private set; }
        public int Durability { get; set; }
        public int MaxDurability { get; set; }

        private static Random rnd = new Random();

        public Ore(int x, int y, OreType type)
        {
            X = x;
            Y = y;
            Type = type;
            Data = OreDatabase.GetOreData(type);

            Size = rnd.Next(60, 140);
            Durability = Data.Durability;
            MaxDurability = Data.Durability;
        }

        /// <summary>
        /// Наносит урон руде с учётом брони.
        /// </summary>
        /// <returns>Финальный урон (для визуализации)</returns>
        public int TakeDamage(int pickaxeDamage)
        {
            int finalDamage = CalculateDamage(pickaxeDamage);
            Durability = Math.Max(0, Durability - finalDamage);
            return finalDamage;
        }

        private int CalculateDamage(int pickaxeDamage)
        {
            return Math.Max(0, pickaxeDamage - Data.Armor);
        }

        public bool IsDestroyed => Durability <= 0;

        public float GetHealthPercent()
        {
            return (float)Durability / MaxDurability;
        }
    }
}

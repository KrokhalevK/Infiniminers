namespace Infiniminers
{
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

        public int TakeDamage(int pickaxeDamage)
        {
            int finalDamage = CalculateDamage(pickaxeDamage);
            Durability = Math.Max(0, Durability - finalDamage);
            return finalDamage;
        }

        private int CalculateDamage(int pickaxeDamage)
        {
            int damage = pickaxeDamage - Data.Armor;
            return Math.Max(0, damage);
        }

        public bool IsDestroyed => Durability <= 0;

        public float GetHealthPercent()
        {
            return (float)Durability / MaxDurability;
        }

        public Color GetColor()
        {
            return Data.Color;
        }

        public string GetTextureName()
        {
            return Data.TextureName;
        }

        public int GetValue()
        {
            return Data.Value;
        }
    }
}

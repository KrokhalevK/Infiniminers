using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infiniminers
{
    public class OreData
    {
        public OreType Type {  get; set; }
        public int Durability { get; set; }
        public int Armor { get; set; }
        public int Value {  get; set; }
        public Color Color { get; set; }
        public string TextureName {  get; set; }

        public OreData(OreType type, int durability, int armor, int value, Color color, string textureName)
        {
            Type = type;
            Durability = durability;
            Armor = armor;
            Value = value;
            Color = color;
            TextureName = textureName;
        }
    }

    public enum OreType
    {
        Stone,
        Iron,
        Gold,
        Diamond
    }

    public static class OreDatabase
    {
        public static OreData GetOreData(OreType type)
        {
            return type switch
            {
                OreType.Stone => new OreData(
                    OreType.Stone,
                    durability: 15,
                    armor: 0,
                    value: 10,
                    Color.Gray,
                    "ores/ore_stone"
                ),
                OreType.Iron => new OreData(
                    OreType.Iron,
                    durability: 25,
                    armor: 5,
                    value: 30,
                    Color.Silver,
                    "ores/ore_iron"
                ),
                OreType.Gold => new OreData(
                    OreType.Gold,
                    durability: 30,
                    armor: 10,
                    value: 100,
                    Color.Gold,
                    "ores/ore_gold"
                ),
                OreType.Diamond => new OreData(
                    OreType.Diamond,
                    durability: 50,
                    armor: 25,
                    value: 500,
                    Color.Cyan,
                    "ores/ore_diamond"
                ),
                _ => throw new System.ArgumentException("Неизвестный тип руды")
            };
        }
    }
}

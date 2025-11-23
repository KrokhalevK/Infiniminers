using System;
using System.Drawing;

namespace Infiniminers
{
    /// <summary>
    /// Данные конкретного типа руды: прочность, броня, стоимость, цвет и текстура.
    /// </summary>
    public class OreData
    {
        public OreType Type { get; }
        public int Durability { get; }
        public int Armor { get; }
        public int Value { get; }
        public Color Color { get; }
        public string TextureName { get; }

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

    /// <summary>
    /// Типы руд в игре.
    /// </summary>
    public enum OreType
    {
        Stone,
        Iron,
        Gold,
        Diamond
    }

    /// <summary>
    /// База данных всех типов руд с их характеристиками.
    /// </summary>
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
                _ => throw new ArgumentException($"Неизвестный тип руды: {type}")
            };
        }
    }
}

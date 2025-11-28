using System;

namespace Infiniminers
{
    /// <summary>
    /// Кирка с характеристиками: тип, урон, цена, название.
    /// </summary>
    public class Pickaxe
    {
        public PickaxeType Type { get; }
        public int Damage { get; }
        public int Price { get; }
        public string Name { get; }
        public string TextureName { get; set; }

        public Pickaxe(PickaxeType type, int damage, int price, string name, string textureName)
        {
            Type = type;
            Damage = damage;
            Price = price;
            Name = name;
            TextureName = textureName;
        }
    }

    /// <summary>
    /// Типы кирок в игре.
    /// </summary>
    public enum PickaxeType
    {
        Wood,
        Stone,
        Iron,
        Diamond,
        Creative
    }
}

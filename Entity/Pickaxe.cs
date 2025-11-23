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

        public Pickaxe(PickaxeType type, int damage, int price, string name)
        {
            Type = type;
            Damage = damage;
            Price = price;
            Name = name;
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

    /// <summary>
    /// База данных всех типов кирок с их характеристиками.
    /// </summary>
    public static class PickaxeDatabase
    {
        public static Pickaxe GetPickaxe(PickaxeType type)
        {
            return type switch
            {
                PickaxeType.Wood => new Pickaxe(
                    PickaxeType.Wood,
                    damage: 5,
                    price: 0,
                    name: "Деревянная кирка"
                ),
                PickaxeType.Stone => new Pickaxe(
                    PickaxeType.Stone,
                    damage: 10,
                    price: 10,
                    name: "Каменная кирка"
                ),
                PickaxeType.Iron => new Pickaxe(
                    PickaxeType.Iron,
                    damage: 15,
                    price: 500,
                    name: "Железная кирка"
                ),
                PickaxeType.Diamond => new Pickaxe(
                    PickaxeType.Diamond,
                    damage: 40,
                    price: 2000,
                    name: "Алмазная кирка"
                ),
                PickaxeType.Creative => new Pickaxe(
                    PickaxeType.Creative,
                    damage: 1000000000,
                    price: 0,
                    name: "Кирка разработчика"
                ),
                _ => throw new ArgumentException($"Неизвестный тип кирки: {type}")
            };
        }
    }
}

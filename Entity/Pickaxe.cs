using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infiniminers
{
    public class Pickaxe
    {
        public PickaxeType Type { get; set; }
        public int Damage { get; set; }
        public int Price { get; set; }
        public string Name { get; set; }
        public Pickaxe(PickaxeType type, int damage, int price, string name) 
        {
            Type = type;
            Damage = damage;
            Price = price;
            Name = name;
        }
    }
    public enum PickaxeType
    {
        Wood,
        Stone,
        Iron,
        Diamond,
        Creative
    }
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
                PickaxeType.Creative => new Pickaxe(PickaxeType.Creative, damage: 1000000000, price: 0, name: "Кирка разработчика"),
                _ => throw new System.ArgumentException("Неизвестный тип кирки")
            };
        }
    }
}

using System;

namespace Infiniminers
{
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
                    name: "Деревянная кирка",
                    textureName: "pickaxes/pickaxe_wood"  // ← ВОТ ЭТА СТРОКА ВАЖНА
                ),
                PickaxeType.Stone => new Pickaxe(
                    PickaxeType.Stone,
                    damage: 10,
                    price: 100,
                    name: "Каменная кирка",
                    textureName: "pickaxes/pickaxe_stone"
                ),
                PickaxeType.Iron => new Pickaxe(
                    PickaxeType.Iron,
                    damage: 20,
                    price: 500,
                    name: "Железная кирка",
                    textureName: "pickaxes/pickaxe_iron"
                ),
                PickaxeType.Diamond => new Pickaxe(
                    PickaxeType.Diamond,
                    damage: 50,
                    price: 2000,
                    name: "Алмазная кирка",
                    textureName: "pickaxes/pickaxe_diamond"
                ),
                PickaxeType.Creative => new Pickaxe(
                    PickaxeType.Creative,
                    damage: 1000000,
                    price: 0,
                    name: "Творческая кирка",
                    textureName: "pickaxes/pickaxe_creative"
                ),
                _ => new Pickaxe(
                    PickaxeType.Wood,
                    damage: 5,
                    price: 0,
                    name: "Деревянная кирка",
                    textureName: "pickaxes/pickaxe_wood"
                )
            };
        }
    }
}

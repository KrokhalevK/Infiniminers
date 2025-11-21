using System;

namespace Infiniminers_v0._0
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Money { get; set; }
        public int Mana { get; set; }
        public int Speed { get; set; }
        public int Size { get; set; }
        public Pickaxe CurrentPickaxe { get; set; }

        public Player(int startX, int startY)
        {
            X = startX;
            Y = startY;
            Money = 0;
            Mana = 100;
            Speed = 20;
            Size = 100;
            CurrentPickaxe = PickaxeDatabase.GetPickaxe(PickaxeType.Wood);
        }

        public void Move(int dx, int dy)
        {
            X += dx * Speed;
            Y += dy * Speed;
        }

        public int GetAttackPower()
        {
            return CurrentPickaxe.Damage;
        }

        public bool BuyPickaxe(Pickaxe pickaxe)
        {
            if (Money >= pickaxe.Price)
            {
                Money -= pickaxe.Price;
                CurrentPickaxe = pickaxe;
                return true;
            }
            return false;
        }

        public void RestoreMana(int amount)
        {
            Mana = Math.Min(Mana + amount, 100);
        }

        public bool UseMana(int amount)
        {
            if (Mana >= amount)
            {
                Mana -= amount;
                return true;
            }
            return false;
        }
    }
}

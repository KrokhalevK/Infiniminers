using System;

namespace Infiniminers
{
    /// <summary>
    /// Игрок с характеристиками, которые можно улучшать через апгрейды.
    /// </summary>
    public class Player
    {
        // === Позиция ===
        public int X { get; set; }
        public int Y { get; set; }
        public int FacingDirection { get; set; } = 1;

        // === Ресурсы ===
        public int Money { get; set; }
        public int Mana { get; set; }
        public int MaxMana { get; set; }

        // === Характеристики (можно апгрейдить) ===
        public int Speed { get; set; }
        public int Size { get; set; }
        public int BonusDamage { get; set; }
        public int MiningRange { get; set; }

        // === Экипировка ===
        public Pickaxe CurrentPickaxe { get; set; }

        // === Начальные значения (для сброса/новой игры) ===
        private const int INITIAL_MONEY = 0;
        private const int INITIAL_MAX_MANA = 100;
        private const int INITIAL_SPEED = 20;
        private const int INITIAL_SIZE = 100;

        public Player(int startX, int startY)
        {
            X = startX;
            Y = startY;
            CurrentPickaxe = PickaxeDatabase.GetPickaxe(PickaxeType.Wood);

            // Инициализация ресурсов
            Money = INITIAL_MONEY;
            MaxMana = INITIAL_MAX_MANA;
            Mana = MaxMana;

            // Инициализация характеристик
            Speed = INITIAL_SPEED;
            Size = INITIAL_SIZE;
            BonusDamage = 0;
            MiningRange = 50;

            // Начальная кирка
            CurrentPickaxe = PickaxeDatabase.GetPickaxe(PickaxeType.Wood);
        }

        // === Движение ===
        public void Move(int dx, int dy)
        {
            if (dx > 0)
                FacingDirection = 1;
            else if (dx < 0)
                FacingDirection = -1;

            X += dx * Speed;
            Y += dy * Speed;
        }

        // === Урон (кирка + бонусы) ===
        public int GetTotalDamage()
        {
            return CurrentPickaxe.Damage + BonusDamage;
        }

        // === Покупка кирки ===
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

        // === Управление маной ===
        public void RestoreMana(int amount)
        {
            Mana = Math.Min(Mana + amount, MaxMana);
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

        // === Апгрейды (для будущего) ===
        public void UpgradeSpeed(int amount)
        {
            Speed += amount;
        }

        public void UpgradeMaxMana(int amount)
        {
            MaxMana += amount;
            Mana = MaxMana; // Восстанавливаем до нового максимума
        }

        public void UpgradeDamage(int amount)
        {
            BonusDamage += amount;
        }

        public void UpgradeSize(int amount)
        {
            Size += amount;
        }

        public void UpgradeMiningRange(int amount)
        {
            MiningRange += amount;
        }
    }
}

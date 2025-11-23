using System;
using System.Linq;

namespace Infiniminers
{
    /// <summary>
    /// Контроллер магазина кирок: навигация и покупка.
    /// </summary>
    public class ShopController
    {
        private int selectedPickaxeIndex = 0;
        public int SelectedPickaxeIndex => selectedPickaxeIndex;

        // Список кирок, доступных в магазине (БЕЗ Creative)
        private static readonly PickaxeType[] ShopPickaxes = Enum.GetValues(typeof(PickaxeType))
            .Cast<PickaxeType>()
            .Where(p => p != PickaxeType.Creative)
            .ToArray();

        public void MoveSelection(int direction)
        {
            selectedPickaxeIndex += direction;
            int maxIndex = ShopPickaxes.Length - 1;

            if (selectedPickaxeIndex < 0)
                selectedPickaxeIndex = maxIndex;
            if (selectedPickaxeIndex > maxIndex)
                selectedPickaxeIndex = 0;
        }

        public bool TryBuyPickaxe(Player player)
        {
            // Получаем выбранную кирку из магазина
            PickaxeType selectedType = ShopPickaxes[selectedPickaxeIndex];
            Pickaxe pickaxe = PickaxeDatabase.GetPickaxe(selectedType);

            // Проверка: уже экипирована?
            if (player.CurrentPickaxe.Type == pickaxe.Type)
                return false;

            // Попытка покупки
            return player.BuyPickaxe(pickaxe);
        }

        public void Reset()
        {
            selectedPickaxeIndex = 0;
        }

        /// <summary>
        /// Получить список всех кирок в магазине (для отрисовки).
        /// </summary>
        public PickaxeType[] GetAvailablePickaxes()
        {
            return ShopPickaxes;
        }
    }
}

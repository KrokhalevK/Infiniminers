namespace Infiniminers_v0._0
{
    public class ShopController
    {
        private int selectedPickaxeIndex = 0;
        public int SelectedPickaxeIndex => selectedPickaxeIndex;

        public void MoveSelection(int direction)
        {
            selectedPickaxeIndex += direction;
            if (selectedPickaxeIndex < 0) selectedPickaxeIndex = 3;
            if (selectedPickaxeIndex > 3) selectedPickaxeIndex = 0;
        }

        public bool TryBuyPickaxe(Player player)
        {
            Pickaxe pickaxe = PickaxeDatabase.GetPickaxe((PickaxeType)selectedPickaxeIndex);
            return player.BuyPickaxe(pickaxe);
        }

        public void Reset()
        {
            selectedPickaxeIndex = 0;
        }
    }
}

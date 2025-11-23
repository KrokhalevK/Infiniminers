namespace Infiniminers
{
    public class ShopRenderer
    {
        private Font titleFont;
        private Font itemFont;
        private Font priceFont;

        public ShopRenderer()
        {
            titleFont = new Font("Arial", 16, FontStyle.Bold);
            itemFont = new Font("Arial", 12);
            priceFont = new Font("Arial", 10);
        }

        public void DrawShop(Graphics g, Player player, int selectedIndex, Size screenSize)
        {
            int shopX = screenSize.Width / 2;  // ← Справа от середины экрана
            int shopY = 0;
            int shopWidth = screenSize.Width / 2;
            int shopHeight = screenSize.Height;

            // Фон магазина
            g.FillRectangle(new SolidBrush(Color.FromArgb(200, 50, 50, 50)), shopX, shopY, shopWidth, shopHeight);
            g.DrawRectangle(Pens.Yellow, shopX, shopY, shopWidth, shopHeight);

            // Заголовок
            g.DrawString("МАГАЗИН", titleFont, Brushes.Yellow, shopX + 20, shopY + 20);
            g.DrawString($"Баланс: {player.Money}", itemFont, Brushes.LimeGreen, shopX + 20, shopY + 50);

            // Предметы (кирки)
            int itemY = shopY + 100;
            int itemHeight = 80;
            int padding = 15;

            for (int i = 0; i < 4; i++)
            {
                PickaxeType type = (PickaxeType)i;
                Pickaxe pickaxe = PickaxeDatabase.GetPickaxe(type);

                int y = itemY + i * (itemHeight + padding);

                // Подсветка выбранного
                if (i == selectedIndex)
                    g.FillRectangle(Brushes.Orange, shopX + padding, y, shopWidth - padding * 2, itemHeight);
                else
                    g.FillRectangle(new SolidBrush(Color.FromArgb(100, 100, 100, 100)), shopX + padding, y, shopWidth - padding * 2, itemHeight);

                // Граница
                g.DrawRectangle(Pens.White, shopX + padding, y, shopWidth - padding * 2, itemHeight);

                // Информация о кирке
                g.DrawString(pickaxe.Name, itemFont, Brushes.White, shopX + 25, y + 10);
                g.DrawString($"Урон: {pickaxe.Damage}", priceFont, Brushes.Cyan, shopX + 25, y + 35);
                g.DrawString($"Цена: {pickaxe.Price}", priceFont,
                    pickaxe.Price <= player.Money ? Brushes.LimeGreen : Brushes.Red,
                    shopX + 25, y + 55);

                // Если это текущая кирка
                if (player.CurrentPickaxe.Type == type)
                    g.DrawString("✓ АКТИВНА", priceFont, Brushes.Lime, shopX + shopWidth - 100, y + 30);
            }

            // Подсказка внизу
            g.DrawString("W/S - Выбор | ENTER - Купить | I - Закрыть", priceFont, Brushes.White,
                shopX + 20, shopY + shopHeight - 40);
        }
    }
}

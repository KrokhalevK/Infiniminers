using System;
using System.Drawing;

namespace Infiniminers
{
    /// <summary>
    /// Рендерер магазина кирок.
    /// </summary>
    public class ShopRenderer : IDisposable
    {
        private Font titleFont;
        private Font itemFont;
        private Font hintFont;

        public ShopRenderer()
        {
            titleFont = new Font("Arial", 20, FontStyle.Bold);
            itemFont = new Font("Arial", 14);
            hintFont = new Font("Arial", 12);
        }

        public void DrawShop(Graphics g, Player player, int selectedPickaxeIndex, Size screenSize)
        {
            int centerX = screenSize.Width / 2;
            int startY = 150;
            int lineHeight = 50;

            // Фон (полупрозрачный)
            g.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.Black)),
                            0, 0, screenSize.Width, screenSize.Height);

            // Заголовок магазина
            string title = "МАГАЗИН КИРОК";
            SizeF titleSize = g.MeasureString(title, titleFont);
            g.DrawString(title, titleFont, Brushes.Gold,
                        centerX - titleSize.Width / 2, 80);

            // Деньги игрока
            g.DrawString($"Деньги: {player.Money}$", itemFont, Brushes.White,
                        centerX - 60, 120);

            // Получаем список кирок БЕЗ Creative
            var pickaxeTypes = new[] { PickaxeType.Wood, PickaxeType.Stone,
                                       PickaxeType.Iron, PickaxeType.Diamond };

            // Отрисовка кирок
            for (int i = 0; i < pickaxeTypes.Length; i++)
            {
                Pickaxe pickaxe = PickaxeDatabase.GetPickaxe(pickaxeTypes[i]);
                int y = startY + i * lineHeight;

                // Статус кирки
                bool isSelected = (i == selectedPickaxeIndex);
                bool isEquipped = (player.CurrentPickaxe.Type == pickaxe.Type);
                bool canAfford = (player.Money >= pickaxe.Price);

                // Выбор цвета
                Brush brush;
                if (isSelected)
                    brush = Brushes.Yellow;           // Выбрана
                else if (isEquipped)
                    brush = Brushes.LimeGreen;        // Экипирована
                else if (canAfford)
                    brush = Brushes.White;            // Можно купить
                else
                    brush = Brushes.Gray;             // Недостаточно денег

                // Текст
                string status = isEquipped ? " [ЭКИПИРОВАНА]" :
                               !canAfford ? " [НЕДОСТАТОЧНО ДЕНЕГ]" : "";
                string text = $"{pickaxe.Name} - {pickaxe.Price}$ (Урон: {pickaxe.Damage}){status}";

                // Отрисовка
                SizeF textSize = g.MeasureString(text, itemFont);
                g.DrawString(text, itemFont, brush, centerX - textSize.Width / 2, y);
            }

            // Подсказки
            g.DrawString("W/S - Выбор | ENTER - Купить | ESC/I - Выход",
                        hintFont, Brushes.LightGray,
                        centerX - 180, screenSize.Height - 60);
        }

        public void Dispose()
        {
            titleFont?.Dispose();
            itemFont?.Dispose();
            hintFont?.Dispose();
        }
    }
}

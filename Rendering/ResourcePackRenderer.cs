using System;
using System.Collections.Generic;
using System.Drawing;

namespace Infiniminers
{
    public class ResourcePackRenderer
    {
        private Font titleFont;
        private Font optionFont;
        private Font hintFont;

        public ResourcePackRenderer()
        {
            titleFont = new Font("Arial", 20, FontStyle.Bold);
            optionFont = new Font("Arial", 16);
            hintFont = new Font("Arial", 12);
        }

        public void DrawResourcePackMenu(Graphics g, Size screenSize, List<string> packs, int selectedIndex)
        {
            int centerX = screenSize.Width / 2;
            int centerY = screenSize.Height / 2;

            // Фон
            g.Clear(Color.DarkGray);

            // Заголовок
            g.DrawString("РЕСУРСПАКИ", titleFont, Brushes.Yellow, centerX - 100, centerY - 200);

            // Паки
            int y = centerY - 100;
            for (int i = 0; i < packs.Count; i++)
            {
                Brush brush = (selectedIndex == i) ? Brushes.Yellow : Brushes.White;
                g.DrawString(packs[i], optionFont, brush, centerX - 80, y);
                y += 70;
            }

            // Подсказка
            g.DrawString("W/S - Выбор | ESC - Назад", hintFont, Brushes.LightGray,
                centerX - 120, screenSize.Height - 50);
        }
    }
}

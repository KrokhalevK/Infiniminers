using System;
using System.Drawing;

namespace Infiniminers
{
    public class SettingsRenderer
    {
        private Font titleFont;
        private Font optionFont;
        private Font hintFont;

        public SettingsRenderer()
        {
            titleFont = new Font("Arial", 20, FontStyle.Bold);
            optionFont = new Font("Arial", 16);
            hintFont = new Font("Arial", 12);
        }

        public void DrawSettingsMenu(Graphics g, Size screenSize, int selectedIndex)
        {
            int centerX = screenSize.Width / 2;
            int centerY = screenSize.Height / 2;

            // Фон
            g.Clear(Color.DarkGray);

            // Заголовок
            g.DrawString("НАСТРОЙКИ", titleFont, Brushes.Yellow, centerX - 80, centerY - 150);

            // Опции
            string[] options = { "РЕСУРСПАКИ", "ЗВУК", "ГРАФИКА" };
            for (int i = 0; i < options.Length; i++)
            {
                int y = centerY + i * 60;
                Brush brush = (selectedIndex == i) ? Brushes.Yellow : Brushes.White;
                g.DrawString(options[i], optionFont, brush, centerX - 100, y);
            }

            // Подсказка
            g.DrawString("W/S - Выбор | ENTER - Открыть | ESC - Назад", hintFont, Brushes.LightGray,
                centerX - 150, screenSize.Height - 50);
        }
    }
}

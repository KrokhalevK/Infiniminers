using System;
using System.Drawing;

namespace Infiniminers
{
    public class PauseMenuRenderer
    {
        private Font titleFont;
        private Font optionFont;
        private Font hintFont;

        public PauseMenuRenderer()
        {
            titleFont = new Font("Arial", 20, FontStyle.Bold);
            optionFont = new Font("Arial", 16);
            hintFont = new Font("Arial", 12);
        }

        public void DrawPauseMenu(Graphics g, Size screenSize, int selectedIndex)
        {
            int centerX = screenSize.Width / 2;
            int centerY = screenSize.Height / 2;

            // Полупрозрачный фон
            g.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), 0, 0, screenSize.Width, screenSize.Height);

            // Заголовок
            g.DrawString("ПАУЗА", titleFont, Brushes.Yellow, centerX - 50, centerY - 150);

            // Опции
            string[] options = { "ПРОДОЛЖИТЬ", "МАГАЗИН", "ГЛАВНОЕ МЕНЮ" };
            for (int i = 0; i < options.Length; i++)
            {
                int y = centerY + i * 60;
                Brush brush = (selectedIndex == i) ? Brushes.Yellow : Brushes.White;
                g.DrawString(options[i], optionFont, brush, centerX - 100, y);
            }

            // Подсказка
            g.DrawString("W/S - Выбор | ENTER - Выбрать", hintFont, Brushes.LightGray,
                centerX - 120, screenSize.Height - 50);
        }
    }
}

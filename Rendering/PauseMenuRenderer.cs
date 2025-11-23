using System;
using System.Drawing;

namespace Infiniminers
{
    /// <summary>
    /// Рендерер меню паузы.
    /// </summary>
    public class PauseMenuRenderer : IDisposable
    {
        private Font titleFont;
        private Font optionFont;
        private Font hintFont;

        // Константы для позиций
        private const int TITLE_OFFSET_Y = 150;
        private const int OPTION_LINE_HEIGHT = 60;
        private const int HINT_OFFSET_Y = 50;
        private const int BACKGROUND_ALPHA = 150;

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
            g.FillRectangle(new SolidBrush(Color.FromArgb(BACKGROUND_ALPHA, 0, 0, 0)),
                            0, 0, screenSize.Width, screenSize.Height);

            // Заголовок (центрирован)
            string title = "ПАУЗА";
            SizeF titleSize = g.MeasureString(title, titleFont);
            g.DrawString(title, titleFont, Brushes.Yellow,
                        centerX - titleSize.Width / 2, centerY - TITLE_OFFSET_Y);

            // Опции
            string[] options = { "ПРОДОЛЖИТЬ", "МАГАЗИН","НАСТРОЙКИ", "ГЛАВНОЕ МЕНЮ" };
            for (int i = 0; i < options.Length; i++)
            {
                int y = centerY + i * OPTION_LINE_HEIGHT;
                Brush brush = (selectedIndex == i) ? Brushes.Yellow : Brushes.White;

                // Центрирование опции
                SizeF optionSize = g.MeasureString(options[i], optionFont);
                g.DrawString(options[i], optionFont, brush,
                            centerX - optionSize.Width / 2, y);
            }

            // Подсказка (центрирована)
            string hint = "W/S - Выбор | ENTER - Выбрать";
            SizeF hintSize = g.MeasureString(hint, hintFont);
            g.DrawString(hint, hintFont, Brushes.LightGray,
                        centerX - hintSize.Width / 2, screenSize.Height - HINT_OFFSET_Y);
        }

        public void Dispose()
        {
            titleFont?.Dispose();
            optionFont?.Dispose();
            hintFont?.Dispose();
        }
    }
}

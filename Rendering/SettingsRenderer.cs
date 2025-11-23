using System;
using System.Drawing;

namespace Infiniminers
{
    /// <summary>
    /// Рендерер меню настроек.
    /// </summary>
    public class SettingsRenderer : IDisposable
    {
        private Font titleFont;
        private Font optionFont;
        private Font hintFont;

        // Константы для позиций
        private const int TITLE_OFFSET_Y = 150;
        private const int OPTION_LINE_HEIGHT = 60;
        private const int HINT_OFFSET_Y = 50;
        private const int BACKGROUND_ALPHA = 150;

        // Опции настроек (соответствуют SettingsOption enum в MenuController)
        private static readonly string[] SETTINGS_OPTIONS = { "РЕСУРСПАКИ", "ЗВУК", "ГРАФИКА" };

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

            // Полупрозрачный фон (видна игра позади)
            g.FillRectangle(new SolidBrush(Color.FromArgb(BACKGROUND_ALPHA, 0, 0, 0)),
                            0, 0, screenSize.Width, screenSize.Height);

            // Заголовок (центрирован)
            string title = "НАСТРОЙКИ";
            SizeF titleSize = g.MeasureString(title, titleFont);
            g.DrawString(title, titleFont, Brushes.Yellow,
                        centerX - titleSize.Width / 2, centerY - TITLE_OFFSET_Y);

            // Опции
            for (int i = 0; i < SETTINGS_OPTIONS.Length; i++)
            {
                int y = centerY + i * OPTION_LINE_HEIGHT;
                Brush brush = (selectedIndex == i) ? Brushes.Yellow : Brushes.White;

                // Центрирование опции
                SizeF optionSize = g.MeasureString(SETTINGS_OPTIONS[i], optionFont);
                g.DrawString(SETTINGS_OPTIONS[i], optionFont, brush,
                            centerX - optionSize.Width / 2, y);
            }

            // Подсказка (центрирована)
            string hint = "W/S - Выбор | ENTER - Открыть | ESC - Назад";
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

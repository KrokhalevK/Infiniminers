using System;
using System.Collections.Generic;
using System.Drawing;

namespace Infiniminers
{
    /// <summary>
    /// Рендерер меню выбора ресурс-паков.
    /// </summary>
    public class ResourcePackRenderer : IDisposable
    {
        private Font titleFont;
        private Font optionFont;
        private Font hintFont;

        // Константы для позиций
        private const int TITLE_OFFSET_Y = 200;
        private const int PACK_OFFSET_X = 80;
        private const int PACK_LINE_HEIGHT = 70;
        private const int HINT_OFFSET_X = 120;
        private const int HINT_OFFSET_Y = 50;
        private const int BACKGROUND_ALPHA = 150;

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

            // Полупрозрачный фон (видна игра позади)
            g.FillRectangle(new SolidBrush(Color.FromArgb(BACKGROUND_ALPHA, 0, 0, 0)),
                            0, 0, screenSize.Width, screenSize.Height);

            // Заголовок (центрирован)
            string title = "РЕСУРСПАКИ";
            SizeF titleSize = g.MeasureString(title, titleFont);
            g.DrawString(title, titleFont, Brushes.Yellow,
                        centerX - titleSize.Width / 2, centerY - TITLE_OFFSET_Y);

            // Паки
            int y = centerY - 100;
            for (int i = 0; i < packs.Count; i++)
            {
                Brush brush = (selectedIndex == i) ? Brushes.Yellow : Brushes.White;

                // Центрирование пака
                SizeF packSize = g.MeasureString(packs[i], optionFont);
                g.DrawString(packs[i], optionFont, brush,
                            centerX - packSize.Width / 2, y);

                y += PACK_LINE_HEIGHT;
            }

            // Подсказка (центрирована)
            string hint = "W/S - Выбор | ENTER - Выбрать | ESC - Назад";
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

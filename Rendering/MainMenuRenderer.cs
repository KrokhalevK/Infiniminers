using System;
using System.Drawing;

namespace Infiniminers
{
    /// <summary>
    /// Рендерер главного меню игры.
    /// </summary>
    public class MainMenuRenderer : IDisposable
    {
        private Font titleFont;
        private Font menuFont;
        private Font hintFont;
        private Font versionFont;

        // Константы для позиций
        private const int TITLE_OFFSET_Y = 150;
        private const int MENU_LINE_HEIGHT = 60;
        private const int HINT_OFFSET_Y = 50;
        private const int BACKGROUND_ALPHA = 150;
        private const string GAME_VERSION = "v0.1.0";

        // Опции меню (соответствуют MenuOption enum в MenuController)
        private static readonly string[] MENU_OPTIONS = { "ИГРАТЬ", "НАСТРОЙКИ", "ВЫХОД" };

        public MainMenuRenderer()
        {
            titleFont = new Font("Arial", 32, FontStyle.Bold);
            menuFont = new Font("Arial", 18);
            hintFont = new Font("Arial", 12);
            versionFont = new Font("Arial", 10);
        }

        public void DrawMainMenu(Graphics g, Size screenSize, int selectedIndex)
        {
            int centerX = screenSize.Width / 2;
            int centerY = screenSize.Height / 2;

            // Полупрозрачный фон
            g.FillRectangle(new SolidBrush(Color.FromArgb(BACKGROUND_ALPHA, 0, 0, 0)),
                            0, 0, screenSize.Width, screenSize.Height);

            // Заголовок (центрирован)
            string title = "INFINIMINERS";
            SizeF titleSize = g.MeasureString(title, titleFont);
            g.DrawString(title, titleFont, Brushes.Gold,
                        centerX - titleSize.Width / 2, centerY - TITLE_OFFSET_Y);

            // Версия (под названием)
            SizeF versionSize = g.MeasureString(GAME_VERSION, versionFont);
            g.DrawString(GAME_VERSION, versionFont, Brushes.Gray,
                        centerX - versionSize.Width / 2, centerY - TITLE_OFFSET_Y + titleSize.Height + 10);

            // Опции меню
            for (int i = 0; i < MENU_OPTIONS.Length; i++)
            {
                int y = centerY + i * MENU_LINE_HEIGHT;
                Brush brush = (selectedIndex == i) ? Brushes.Yellow : Brushes.White;

                // Центрирование опции
                SizeF optionSize = g.MeasureString(MENU_OPTIONS[i], menuFont);
                g.DrawString(MENU_OPTIONS[i], menuFont, brush,
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
            menuFont?.Dispose();
            hintFont?.Dispose();
            versionFont?.Dispose();
        }
    }
}

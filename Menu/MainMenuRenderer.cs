using System;
using System.Collections.Generic;
using System.Drawing;

namespace Infiniminers_v0._0
{
    public class MainMenuRenderer
    {
        private Font titleFont;
        private Font menuFont;
        private Font selectedFont;

        public MainMenuRenderer()
        {
            titleFont = new Font("Arial", 36, FontStyle.Bold);
            menuFont = new Font("Arial", 20);
            selectedFont = new Font("Arial", 20, FontStyle.Bold);
        }

        public void DrawMainMenu(Graphics g, Size screenSize, int selectedIndex)
        {
            // Рисуем фон
            g.Clear(Color.DarkGray);

            // Рисуем заголовок
            string title = "INFINIMINERS";
            SizeF titleSize = g.MeasureString(title, titleFont);
            float titleX = (screenSize.Width - titleSize.Width) / 2;
            float titleY = 50;
            g.DrawString(title, titleFont, Brushes.Gold, titleX, titleY);

            // Опции меню
            string[] menuOptions = { "Начать игру", "Настройки", "Выход" };
            float menuStartY = 200;
            float lineSpacing = 60;

            for (int i = 0; i < menuOptions.Length; i++)
            {
                Font font = (i == selectedIndex) ? selectedFont : menuFont;
                Brush brush = (i == selectedIndex) ? Brushes.Yellow : Brushes.White;

                SizeF optionSize = g.MeasureString(menuOptions[i], font);
                float optionX = (screenSize.Width - optionSize.Width) / 2;
                float optionY = menuStartY + (i * lineSpacing);

                g.DrawString(menuOptions[i], font, brush, optionX, optionY);
            }

            // Подсказка управления
            g.DrawString("W/S - Выбор | Enter - Выбрать", new Font("Arial", 12), Brushes.LightGray, 10, screenSize.Height - 30);
        }

        public void DrawSettingsMenu(Graphics g, Size screenSize)
        {
            g.Clear(Color.DarkGray);

            string title = "НАСТРОЙКИ";
            SizeF titleSize = g.MeasureString(title, titleFont);
            float titleX = (screenSize.Width - titleSize.Width) / 2;
            g.DrawString(title, titleFont, Brushes.Gold, titleX, 50);

            g.DrawString("Здесь будут настройки", menuFont, Brushes.White, 100, 150);
            g.DrawString("ESC - Вернуться в меню", new Font("Arial", 12), Brushes.LightGray, 10, screenSize.Height - 30);
        }
    }
}

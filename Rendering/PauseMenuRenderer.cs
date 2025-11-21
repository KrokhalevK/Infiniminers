namespace Infiniminers_v0._0
{
    public class PauseMenuRenderer
    {
        private Font titleFont;
        private Font menuFont;
        private Font selectedFont;

        public PauseMenuRenderer()
        {
            titleFont = new Font("Arial", 36, FontStyle.Bold);
            menuFont = new Font("Arial", 20);
            selectedFont = new Font("Arial", 20, FontStyle.Bold);
        }

        public void DrawPauseMenu(Graphics g, Size screenSize, int selectedIndex)
        {
            // Рисуем полупрозрачный оверлей
            using (Brush darkBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
            {
                g.FillRectangle(darkBrush, 0, 0, screenSize.Width, screenSize.Height);
            }

            // Рисуем заголовок
            string title = "ПАУЗА";
            SizeF titleSize = g.MeasureString(title, titleFont);
            float titleX = (screenSize.Width - titleSize.Width) / 2;
            float titleY = 100;
            g.DrawString(title, titleFont, Brushes.White, titleX, titleY);

            // Опции меню паузы
            string[] menuOptions = { "Продолжить", "В главное меню", "Выход" };
            float menuStartY = 250;
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
    }
}

using System.Drawing;

namespace Infiniminers_v0._0
{
    public class GameRenderer
    {
        private Font hudFont;

        public GameRenderer()
        {
            hudFont = new Font("Arial", 12);
        }

        public void Draw(Graphics g, GameController game)
        {
            // Рисуем игрока
            g.FillRectangle(Brushes.SaddleBrown, game.Player.X, game.Player.Y, game.Player.Size, game.Player.Size);

            // Рисуем руду
            foreach (var ore in game.Ores)
            {
                using (Brush oreBrush = new SolidBrush(ore.OreColor))
                {
                    g.FillRectangle(oreBrush, ore.X, ore.Y, ore.Size, ore.Size);
                    g.DrawString($"{ore.Value}", hudFont, Brushes.Black, ore.X, ore.Y - 15);
                }
            }
        }
    }
}

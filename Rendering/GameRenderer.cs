using System.Drawing;

namespace Infiniminers_v0._0
{
    public class GameRenderer
    {
        private Font hudFont;
        private ResourceManager resourceManager;

        public GameRenderer(ResourceManager resourceManager = null)
        {
            hudFont = new Font("Arial", 12);
            this.resourceManager = resourceManager;
        }

        public void Draw(Graphics g, GameController game)
        {
            DrawBackground(g);
            DrawPlayer(g, game);
            DrawOres(g, game);
        }

        private void DrawBackground(Graphics g)
        {
            if (resourceManager == null)
                return;

            var bgTexture = resourceManager.GetTexture("background/bg_grass");
            if (bgTexture != null)
                g.DrawImage(bgTexture, 0, 0, g.VisibleClipBounds.Width, g.VisibleClipBounds.Height);
        }

        private void DrawPlayer(Graphics g, GameController game)
        {
            if (resourceManager != null)
            {
                var playerTexture = resourceManager.GetTexture("player/player");
                if (playerTexture != null)
                {
                    g.DrawImage(playerTexture, game.Player.X, game.Player.Y,
                               game.Player.Size, game.Player.Size);
                    return;
                }
            }

            g.FillRectangle(Brushes.SaddleBrown, game.Player.X, game.Player.Y,
                           game.Player.Size, game.Player.Size);
        }

        private void DrawOres(Graphics g, GameController game)
        {
            foreach (var ore in game.Ores)
            {
                DrawOreTexture(g, ore);
                DrawOreHealthBar(g, ore);
                DrawOreValue(g, ore);
            }
        }

        private void DrawOreTexture(Graphics g, Ore ore)
        {
            if (resourceManager != null)
            {
                var oreTexture = resourceManager.GetTexture(ore.GetTextureName());
                if (oreTexture != null)
                {
                    g.DrawImage(oreTexture, ore.X, ore.Y, ore.Size, ore.Size);
                    return;
                }
            }

            using (Brush oreBrush = new SolidBrush(ore.GetColor()))
                g.FillRectangle(oreBrush, ore.X, ore.Y, ore.Size, ore.Size);
        }

        private void DrawOreHealthBar(Graphics g, Ore ore)
        {
            const int barHeight = 5;
            int barWidth = ore.Size;
            float healthPercent = ore.GetHealthPercent();

            g.FillRectangle(Brushes.DarkRed, ore.X, ore.Y - 10, barWidth, barHeight);
            g.FillRectangle(Brushes.LimeGreen, ore.X, ore.Y - 10,
                           (int)(barWidth * healthPercent), barHeight);
            g.DrawRectangle(Pens.Black, ore.X, ore.Y - 10, barWidth, barHeight);
        }

        private void DrawOreValue(Graphics g, Ore ore)
        {
            g.DrawString(ore.GetValue().ToString(), hudFont, Brushes.Black, ore.X, ore.Y - 15);
        }
    }
}

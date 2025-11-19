using System.Drawing;
using System.Resources;

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
            if (resourceManager != null)
            {
                var bgTexture = resourceManager.GetTexture("background/bg_grass");
                if (bgTexture != null)
                {
                    g.DrawImage(bgTexture, 0, 0, g.VisibleClipBounds.Width, g.VisibleClipBounds.Height);
                }
            }

            if (resourceManager != null)
            {
                var playerTexture = resourceManager.GetTexture("player/player");
                if (playerTexture != null)
                {
                    g.DrawImage(playerTexture, game.Player.X, game.Player.Y, game.Player.Size, game.Player.Size);
                }
                else
                {
                    g.FillRectangle(Brushes.SaddleBrown, game.Player.X, game.Player.Y, game.Player.Size, game.Player.Size);
                }
            }
            else
            {
                g.FillRectangle(Brushes.SaddleBrown, game.Player.X, game.Player.Y, game.Player.Size, game.Player.Size);
            }

            foreach (var ore in game.Ores)
            {
                if (resourceManager != null)
                {
                    var oreTexture = resourceManager.GetTexture("ores/ore_gold");
                    if (oreTexture != null)
                    {
                        g.DrawImage(oreTexture, ore.X, ore.Y, ore.Size, ore.Size);
                    }
                    else
                    {
                        using (Brush oreBrush = new SolidBrush(ore.OreColor))
                        {
                            g.FillRectangle(oreBrush, ore.X, ore.Y, ore.Size, ore.Size);
                        }
                    }
                }
                else
                {
                    using (Brush oreBrush = new SolidBrush(ore.OreColor))
                    {
                        g.FillRectangle(oreBrush, ore.X, ore.Y, ore.Size, ore.Size);
                    }
                }
                g.DrawString($"{ore.Value}", hudFont, Brushes.Black, ore.X, ore.Y - 15);
            }
        }
    }
}

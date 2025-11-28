using System;
using System.Drawing;

namespace Infiniminers
{
    /// <summary>
    /// Рендерер игры: отрисовывает фон, игрока, руды и UI.
    /// </summary>
    public class GameRenderer : IDisposable
    {
        private Font hudFont;
        private ResourceManager resourceManager;

        // Константы для отрисовки
        private const int HEALTH_BAR_HEIGHT = 5;
        private const int HEALTH_BAR_OFFSET = 10;
        private const int VALUE_TEXT_OFFSET = 20;

        public GameRenderer(ResourceManager resourceManager = null)
        {
            hudFont = new Font("Arial", 12);
            this.resourceManager = resourceManager;
        }

        public void Draw(Graphics g, GameController game)
        {
            DrawBackground(g, game.MapManager);  // ← Передаём MapManager
            DrawOres(g, game);
            DrawPlayer(g, game);
        }

        private void DrawBackground(Graphics g, MapManager mapManager)
        {
            if (resourceManager == null)
                return;

            // Получаем имя текстуры в зависимости от глубины
            string bgTextureName = mapManager.GetBackgroundTextureName();
            var bgTexture = resourceManager.GetTexture(bgTextureName);

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
                    int playerX = game.Player.X;
                    int playerY = game.Player.Y;
                    int playerSize = game.Player.Size;

                    if (game.Player.FacingDirection == -1)
                        DrawFlippedImage(g, playerTexture, playerX, playerY, playerSize, playerSize);
                    else
                        g.DrawImage(playerTexture, playerX, playerY, playerSize, playerSize);

                    DrawPickaxeInHand(g, game.Player, playerX, playerY, playerSize);
                    return;
                }
            }

            g.FillRectangle(Brushes.SaddleBrown, game.Player.X, game.Player.Y,
                           game.Player.Size, game.Player.Size);
        }

        private void DrawPickaxeInHand(Graphics g, Player player, int playerX, int playerY, int playerSize)
        {
            if (resourceManager == null)
                return;

            var pickaxeTexture = resourceManager.GetTexture(player.CurrentPickaxe.TextureName);
            if (pickaxeTexture == null)
                return;

            int pickaxeSize = playerSize / 2;
            int pickaxeX, pickaxeY;

            if (player.FacingDirection == 1)
            {
                pickaxeX = playerX + playerSize - pickaxeSize / 2;
                pickaxeY = playerY + playerSize - pickaxeSize - 5;
            }
            else
            {
                pickaxeX = playerX - pickaxeSize / 2;
                pickaxeY = playerY + playerSize - pickaxeSize - 5;
            }

            if (player.FacingDirection == -1)
                DrawFlippedImage(g, pickaxeTexture, pickaxeX, pickaxeY, pickaxeSize, pickaxeSize);
            else
                g.DrawImage(pickaxeTexture, pickaxeX, pickaxeY, pickaxeSize, pickaxeSize);
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
            var oreTexture = TryGetTexture(ore.Data.TextureName);
            if (oreTexture != null)
            {
                g.DrawImage(oreTexture, ore.X, ore.Y, ore.Size, ore.Size);
                return;
            }

            // Fallback: цветной квадрат
            using (Brush oreBrush = new SolidBrush(ore.Data.Color))
                g.FillRectangle(oreBrush, ore.X, ore.Y, ore.Size, ore.Size);
        }

        private void DrawOreHealthBar(Graphics g, Ore ore)
        {
            int barWidth = ore.Size;
            float healthPercent = ore.GetHealthPercent();
            int barY = ore.Y - HEALTH_BAR_OFFSET;

            // Фон (красный)
            g.FillRectangle(Brushes.DarkRed, ore.X, barY, barWidth, HEALTH_BAR_HEIGHT);

            // Текущее здоровье (зелёный)
            g.FillRectangle(Brushes.LimeGreen, ore.X, barY,
                           (int)(barWidth * healthPercent), HEALTH_BAR_HEIGHT);

            // Обводка
            g.DrawRectangle(Pens.Black, ore.X, barY, barWidth, HEALTH_BAR_HEIGHT);
        }

        private void DrawOreValue(Graphics g, Ore ore)
        {
            string valueText = ore.Data.Value.ToString();
            float x = ore.X;
            float y = ore.Y - VALUE_TEXT_OFFSET;

            // Обводка для читаемости на любом фоне
            g.DrawString(valueText, hudFont, Brushes.Black, x - 1, y);
            g.DrawString(valueText, hudFont, Brushes.Black, x + 1, y);
            g.DrawString(valueText, hudFont, Brushes.Black, x, y - 1);
            g.DrawString(valueText, hudFont, Brushes.Black, x, y + 1);

            // Основной текст (белый)
            g.DrawString(valueText, hudFont, Brushes.White, x, y);
        }

        private void DrawFlippedImage(Graphics g, Image image, int x, int y, int width, int height)
        {
            // Сохраняем текущее состояние Graphics
            var state = g.Save();

            // Переводим в точку назначения
            g.TranslateTransform(x + width, y);

            // Отражаем по X (масштаб -1 по X, 1 по Y)
            g.ScaleTransform(-1, 1);

            // Рисуем изображение в позицию (0, 0) после трансформации
            g.DrawImage(image, 0, 0, width, height);

            // Восстанавливаем состояние Graphics
            g.Restore(state);
        }

        private Image? TryGetTexture(string textureName)
        {
            return resourceManager?.GetTexture(textureName);
        }

        public void Dispose()
        {
            hudFont?.Dispose();
        }
    }
}

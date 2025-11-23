using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Infiniminers
{
    public partial class Form1 : Form
    {
        private GameController game = null!;
        private GameRenderer gameRenderer = null!;
        private MainMenuRenderer menuRenderer = null!;
        private PauseMenuRenderer pauseMenuRenderer = null!;
        private ResourceManager resourceManager = null!;
        private MenuController menuController = null!;
        private ShopController shopController = null!;
        private ShopRenderer shopRenderer = null!;
        private SettingsRenderer settingsRenderer = null!;
        private ResourcePackRenderer resourcePackRenderer = null!;
        private HashSet<Keys> pressedKeys = new HashSet<Keys>();

        public Form1()
        {
            InitializeComponent();

            try
            {
                InitializeGame();
                InitializeForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}\n{ex.StackTrace}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeGame()
        {
            resourceManager = new ResourceManager();
            game = new GameController(this.ClientSize);
            gameRenderer = new GameRenderer(resourceManager);
            menuRenderer = new MainMenuRenderer();
            pauseMenuRenderer = new PauseMenuRenderer();
            settingsRenderer = new SettingsRenderer();
            resourcePackRenderer = new ResourcePackRenderer();
            menuController = new MenuController();
            shopController = new ShopController();
            shopRenderer = new ShopRenderer();
            menuController.InitializeResourcePackManager(resourceManager);

#if DEBUG
            resourceManager.SetDebugMode(true);
#endif
        }

        private void InitializeForm()
        {
            this.KeyPreview = true;
            this.KeyDown += MainForm_KeyDown;
            this.KeyUp += MainForm_KeyUp;
            this.DoubleBuffered = true;
            this.WindowState = FormWindowState.Maximized;
        }

        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (menuController.CurrentState)
            {
                case GameState.MainMenu:
                    HandleMainMenuInput(e.KeyCode);
                    break;
                case GameState.Settings:
                    HandleSettingsInput(e.KeyCode);
                    break;
                case GameState.Playing:
                    HandleGameplayInput(e.KeyCode);
                    break;
                case GameState.Paused:
                    HandlePauseMenuInput(e.KeyCode);
                    break;
                case GameState.Shop:
                    HandleShopInput(e.KeyCode);
                    break;
                case GameState.ResourcePacks:
                    HandleResourcePackInput(e.KeyCode);
                    break;
            }

            // Единая перерисовка после обработки ввода
            this.Invalidate();
        }

        private void MainForm_KeyUp(object? sender, KeyEventArgs e)
        {
            if (menuController.CurrentState == GameState.Playing)
                pressedKeys.Remove(e.KeyCode);
        }

        private void HandleSettingsInput(Keys key)
        {
            HandleMenuNavigation(key);

            if (key == Keys.Return)
            {
                MenuController.SettingsOption option = menuController.GetSelectedSettingsOption();
                if (option == MenuController.SettingsOption.ResourcePacks)
                    menuController.OpenResourcePacks();
            }

            if (key == Keys.Escape)
                menuController.BackToPreviousState();
        }

        private void HandleGameplayInput(Keys key)
        {
            if (key == Keys.Escape)
            {
                menuController.PauseGame();
                return;
            }

            if (key == Keys.I)
            {
                menuController.CurrentState = GameState.Shop;
                shopController.Reset();
                return;
            }

            if (key == Keys.Space)
            {
                game.CollectOre();
                return;
            }

            pressedKeys.Add(key);
            ProcessMovement();
        }

        private void HandleShopInput(Keys key)
        {
            if (key == Keys.Escape || key == Keys.I)
            {
                menuController.CurrentState = GameState.Playing;
                return;
            }

            if (key == Keys.W)
                shopController.MoveSelection(-1);
            else if (key == Keys.S)
                shopController.MoveSelection(1);
            else if (key == Keys.Return)
                shopController.TryBuyPickaxe(game.Player);
        }

        private void HandleMainMenuInput(Keys key)
        {
            HandleMenuNavigation(key);

            if (key == Keys.Return)
            {
                MenuController.MenuOption option = menuController.GetSelectedOption();
                if (option == MenuController.MenuOption.Play)
                    menuController.SelectOption();
                else if (option == MenuController.MenuOption.Settings)
                    menuController.CurrentState = GameState.Settings;
                else if (option == MenuController.MenuOption.Exit)
                    this.Close();
            }
        }

        private void HandlePauseMenuInput(Keys key)
        {
            HandleMenuNavigation(key);

            if (key == Keys.Return)
            {
                MenuController.PauseMenuOption option = menuController.GetSelectedPauseOption();

                if (option == MenuController.PauseMenuOption.Resume)
                    menuController.CurrentState = GameState.Playing;
                else if (option == MenuController.PauseMenuOption.Shop)
                    menuController.OpenShop();
                else if (option == MenuController.PauseMenuOption.Settings)
                    menuController.CurrentState = GameState.Settings;
                else if (option == MenuController.PauseMenuOption.MainMenu)
                    menuController.BackToMenu();
            }
            else if (key == Keys.Escape)
            {
                menuController.CurrentState = GameState.Playing;
            }
        }

        private void HandleMenuNavigation(Keys key)
        {
            if (key == Keys.W)
                menuController.MoveMenuSelection(-1);
            else if (key == Keys.S)
                menuController.MoveMenuSelection(1);
        }

        private void HandleResourcePackInput(Keys key)
        {
            if (key == Keys.W)
                menuController.MoveResourcePackSelection(-1);
            else if (key == Keys.S)
                menuController.MoveResourcePackSelection(1);
            else if (key == Keys.Escape)
                menuController.CloseResourcePacks();
        }

        private void ProcessMovement()
        {
            int dx = 0, dy = 0;
            if (pressedKeys.Contains(Keys.A)) dx -= 1;
            if (pressedKeys.Contains(Keys.D)) dx += 1;
            if (pressedKeys.Contains(Keys.W)) dy -= 1;
            if (pressedKeys.Contains(Keys.S)) dy += 1;

            if (dx != 0 || dy != 0)
                game.MovePlayer(dx, dy);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            try
            {
                switch (menuController.CurrentState)
                {
                    case GameState.MainMenu:
                        menuRenderer.DrawMainMenu(e.Graphics, this.ClientSize, menuController.GetSelectedMenuIndex());
                        break;
                    case GameState.Settings:
                        bool isInGame = menuController.IsFromGame();

                        // Если из игры - рисуем игру
                        if (isInGame)
                            gameRenderer.Draw(e.Graphics, game);

                        // Рисуем настройки поверх
                        settingsRenderer.DrawSettingsMenu(e.Graphics, this.ClientSize,
                                                         menuController.GetSelectedMenuIndex(),
                                                         isInGame);
                        break;
                    case GameState.Playing:
                        gameRenderer.Draw(e.Graphics, game);
                        DrawGameHUD(e.Graphics);
                        break;
                    case GameState.Paused:
                        gameRenderer.Draw(e.Graphics, game);
                        pauseMenuRenderer.DrawPauseMenu(e.Graphics, this.ClientSize, menuController.GetSelectedMenuIndex());
                        break;
                    case GameState.Shop:
                        gameRenderer.Draw(e.Graphics, game);
                        shopRenderer.DrawShop(e.Graphics, game.Player, shopController.SelectedPickaxeIndex, this.ClientSize);
                        break;
                    case GameState.ResourcePacks:
                        resourcePackRenderer.DrawResourcePackMenu(e.Graphics, this.ClientSize,
                            menuController.GetAvailableResourcePacks(), menuController.GetResourcePackSelectedIndex());
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отрисовке: {ex.Message}\n{ex.StackTrace}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DrawGameHUD(Graphics g)
        {
            const int startX = 10;
            const int startY = 10;
            int lineHeight = (int)this.Font.GetHeight() + 5;

            g.DrawString($"X: {game.Player.X} Y: {game.Player.Y}", this.Font, Brushes.Black, startX, startY);
            g.DrawString($"Деньги: {game.Player.Money}", this.Font, Brushes.Black, startX, startY + lineHeight);
            g.DrawString($"Глубина: {game.MapManager.CurrentDepth}", this.Font, Brushes.Black, startX, startY + lineHeight * 2);
            g.DrawString("Space - Добыча | I - Магазин | ESC - Пауза", this.Font, Brushes.Black, startX, startY + lineHeight * 3);
            g.DrawString($"Кирка: {game.Player.CurrentPickaxe.Name} (Урон: {game.Player.GetTotalDamage()})", this.Font, Brushes.Black, startX, startY + lineHeight * 4);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Освобождаем все IDisposable ресурсы
            gameRenderer?.Dispose();
            shopRenderer?.Dispose();
            pauseMenuRenderer?.Dispose();
            resourcePackRenderer?.Dispose();
            settingsRenderer?.Dispose();
            menuRenderer?.Dispose();
            resourceManager?.Dispose();
        }
    }
}

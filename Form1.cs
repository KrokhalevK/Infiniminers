using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Infiniminers_v0._0
{
    public partial class Form1 : Form
    {
        private GameController game;
        private GameRenderer gameRenderer;
        private MainMenuRenderer menuRenderer;
        private PauseMenuRenderer pauseMenuRenderer;
        private ResourceManager resourceManager;
        private MenuController menuController;
        private ShopController shopController;
        private ShopRenderer shopRenderer;
        private HashSet<Keys> pressedKeys = new HashSet<Keys>();

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
            InitializeForm();
        }

        private void InitializeGame()
        {
            resourceManager = new ResourceManager();
            game = new GameController(this.ClientSize);
            gameRenderer = new GameRenderer(resourceManager);
            menuRenderer = new MainMenuRenderer();
            pauseMenuRenderer = new PauseMenuRenderer();
            menuController = new MenuController();
            shopController = new ShopController();
            shopRenderer = new ShopRenderer();
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
            }
        }

        private void MainForm_KeyUp(object? sender, KeyEventArgs e)
        {
            if (menuController.CurrentState == GameState.Playing)
                pressedKeys.Remove(e.KeyCode);
        }

        private void HandleSettingsInput(Keys key)
        {
            if (key == Keys.Escape)
                menuController.BackToMenu();
            this.Invalidate();
        }

        private void HandleGameplayInput(Keys key)
        {
            if (key == Keys.Escape)
            {
                menuController.PauseGame();
                this.Invalidate();
                return;
            }

            if (key == Keys.I)
            {
                menuController.CurrentState = GameState.Shop;
                shopController.Reset();
                this.Invalidate();
                return;
            }

            if (key == Keys.Space)
            {
                game.CollectOre();
                this.Invalidate();
                return;
            }

            pressedKeys.Add(key);
            ProcessMovement();
            this.Invalidate();
        }

        private void HandleShopInput(Keys key)
        {
            if (key == Keys.I)
            {
                menuController.CurrentState = GameState.Playing;
                this.Invalidate();
                return;
            }

            if (key == Keys.W)
                shopController.MoveSelection(-1);
            else if (key == Keys.S)
                shopController.MoveSelection(1);
            else if (key == Keys.Enter)
                shopController.TryBuyPickaxe(game.Player);

            this.Invalidate();
        }

        private void HandleMainMenuInput(Keys key)
        {
            HandleMenuNavigation(key);

            if (key == Keys.Return)
            {
                MenuController.MenuOption option = menuController.GetSelectedOption();
                if (option != MenuController.MenuOption.Exit)
                    menuController.SelectOption();
                else
                    this.Close();
            }

            this.Invalidate();
        }

        private void HandlePauseMenuInput(Keys key)
        {
            HandleMenuNavigation(key);

            if (key == Keys.Return)
            {
                MenuController.PauseMenuOption option = menuController.GetSelectedPauseOption();
                if (option != MenuController.PauseMenuOption.Exit)
                    menuController.SelectPauseOption();
                else
                    this.Close();
            }

            this.Invalidate();
        }

        private void HandleMenuNavigation(Keys key)
        {
            if (key == Keys.W)
                menuController.MoveMenuSelection(-1);
            else if (key == Keys.S)
                menuController.MoveMenuSelection(1);
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

            switch (menuController.CurrentState)
            {
                case GameState.MainMenu:
                    menuRenderer.DrawMainMenu(e.Graphics, this.ClientSize, menuController.GetSelectedMenuIndex());
                    break;
                case GameState.Settings:
                    menuRenderer.DrawSettingsMenu(e.Graphics, this.ClientSize);
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
            }
        }

        private void DrawGameHUD(Graphics g)
        {
            const int startX = 10;
            const int startY = 10;
            int lineHeight = (int)this.Font.GetHeight() + 5;

            g.DrawString($"X: {game.Player.X} Y: {game.Player.Y}", this.Font, Brushes.Black, startX, startY);
            g.DrawString($"Деньги: {game.Player.Money}", this.Font, Brushes.Black, startX, startY + lineHeight);
            g.DrawString("Space - Добыча | ESC - Пауза", this.Font, Brushes.Black, startX, startY + lineHeight * 2);
            g.DrawString($"Пак: {resourceManager.GetCurrentResourcePackName()}", this.Font, Brushes.Black, startX, startY + lineHeight * 3);
        }
    }
}

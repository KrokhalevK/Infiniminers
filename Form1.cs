using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Infiniminers_v0._0
{
    public partial class 
        Form1 : Form
    {
        private GameController game;
        private GameRenderer gameRenderer;
        private MainMenuRenderer menuRenderer;
        private PauseMenuRenderer pauseMenuRenderer;
        private ResourceManager resourceManager;
        private MenuController menuController;
        private HashSet<Keys> pressedKeys = new HashSet<Keys>();

        public Form1()
        {
            InitializeComponent();

            resourceManager = new ResourceManager();
            game = new GameController(this.ClientSize);
            gameRenderer = new GameRenderer(resourceManager);
            menuRenderer = new MainMenuRenderer();
            pauseMenuRenderer = new PauseMenuRenderer();
            menuController = new MenuController();

            this.KeyPreview = true;
            this.KeyDown += MainForm_KeyDown;
            this.KeyUp += MainForm_KeyUp;
            this.DoubleBuffered = true;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (menuController.CurrentState == GameState.MainMenu)
            {
                HandleMainMenuInput(e.KeyCode);
            }
            else if (menuController.CurrentState == GameState.Settings)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    menuController.BackToMenu();
                }
                this.Invalidate();
            }
            else if (menuController.CurrentState == GameState.Playing)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    menuController.PauseGame();
                    this.Invalidate();
                    return;
                }

                pressedKeys.Add(e.KeyCode);
                ProcessMovement();
                this.Invalidate();
            }
            else if (menuController.CurrentState == GameState.Paused)
            {
                HandlePauseMenuInput(e.KeyCode);
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (menuController.CurrentState == GameState.Playing)
            {
                pressedKeys.Remove(e.KeyCode);
                ProcessMovement();
            }
        }

        private void HandleMainMenuInput(Keys key)
        {
            if (key == Keys.W)
            {
                menuController.MoveMenuSelection(-1);
            }
            else if (key == Keys.S)
            {
                menuController.MoveMenuSelection(1);
            }
            else if (key == Keys.Return)
            {
                MenuController.MenuOption option = menuController.GetSelectedOption();
                if (option == MenuController.MenuOption.Exit)
                {
                    this.Close();
                }
                else
                {
                    menuController.SelectOption();
                }
            }

            this.Invalidate();
        }

        private void HandlePauseMenuInput(Keys key)
        {
            if (key == Keys.W)
            {
                menuController.MoveMenuSelection(-1);
            }
            else if (key == Keys.S)
            {
                menuController.MoveMenuSelection(1);
            }
            else if (key == Keys.Return)
            {
                MenuController.PauseMenuOption option = menuController.GetSelectedPauseOption();
                if (option == MenuController.PauseMenuOption.Exit)
                {
                    this.Close();
                }
                else
                {
                    menuController.SelectPauseOption();
                }
            }

            this.Invalidate();
        }

        private void ProcessMovement()
        {
            int dx = 0, dy = 0;
            if (pressedKeys.Contains(Keys.A)) dx -= 1;
            if (pressedKeys.Contains(Keys.D)) dx += 1;
            if (pressedKeys.Contains(Keys.W)) dy -= 1;
            if (pressedKeys.Contains(Keys.S)) dy += 1;

            if (dx != 0 || dy != 0)
            {
                game.MovePlayer(dx, dy);
                game.CollectOre();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (menuController.CurrentState == GameState.MainMenu)
            {
                menuRenderer.DrawMainMenu(e.Graphics, this.ClientSize, menuController.GetSelectedMenuIndex());
            }
            else if (menuController.CurrentState == GameState.Settings)
            {
                menuRenderer.DrawSettingsMenu(e.Graphics, this.ClientSize);
            }
            else if (menuController.CurrentState == GameState.Playing)
            {
                gameRenderer.Draw(e.Graphics, game);

                int startX = 10;
                int startY = 10;
                int lineHeight = (int)this.Font.GetHeight() + 5;

                e.Graphics.DrawString($"X: {game.Player.X} Y: {game.Player.Y}", this.Font, Brushes.Black, startX, startY);
                e.Graphics.DrawString($"Деньги: {game.Player.Money}", this.Font, Brushes.Black, startX, startY + lineHeight);
                e.Graphics.DrawString("ESC - Пауза", this.Font, Brushes.Black, startX, startY + lineHeight * 2);
                e.Graphics.DrawString($"Пак: {resourceManager.GetCurrentResourcePackName()}", this.Font, Brushes.Black, startX, startY + lineHeight * 3);
            }
            else if (menuController.CurrentState == GameState.Paused)
            {
                gameRenderer.Draw(e.Graphics, game);
                pauseMenuRenderer.DrawPauseMenu(e.Graphics, this.ClientSize, menuController.GetSelectedMenuIndex());
            }
        }
    }
}

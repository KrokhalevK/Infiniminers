using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Infiniminers_v0._0
{
    public partial class Form1 : Form
    {
        private GameController game;
        private GameRenderer renderer;
        private HashSet<Keys> pressedKeys = new HashSet<Keys>();

        public Form1()
        {
            InitializeComponent();

            game = new GameController(this.ClientSize);
            renderer = new GameRenderer();

            this.KeyPreview = true;
            this.KeyDown += MainForm_KeyDown;
            this.KeyUp += MainForm_KeyUp;
            this.DoubleBuffered = true;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            pressedKeys.Add(e.KeyCode);
            ProcessMovement();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
            ProcessMovement();
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
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            renderer.Draw(e.Graphics, game);

            // Отрисовка HUD (тексты с отступами)
            int startX = 10;
            int startY = 10;
            int lineHeight = (int)this.Font.GetHeight() + 5;

            e.Graphics.DrawString($"X: {game.Player.X} Y: {game.Player.Y}", this.Font, Brushes.Black, startX, startY);
            e.Graphics.DrawString($"Деньги: {game.Player.Money}", this.Font, Brushes.Black, startX, startY + lineHeight);
        }
    }
}

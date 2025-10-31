namespace Infiniminers_v0._0
{
    public partial class Form1 : Form
    {
        Player player;
        List<Ore> ores;
        HashSet<Keys> pressedKeys = new HashSet<Keys>();
        public Form1()
        {
            InitializeComponent();

            player = new Player(100, 100);
            ores = new List<Ore>();
            
            Random rnd = new Random();

            for (int i = 0; i < 5; i++)
            {
                int x = rnd.Next(50, this.ClientSize.Width - 50);
                int y = rnd.Next(50, this.ClientSize.Height - 50);
                int value = rnd.Next(10, 101);
                Color color = Color.Gold;
                ores.Add(new Ore(x, y, value, color));
            }

            this.KeyPreview = true;
            this.KeyDown += MainForm_KeyDown;
            this.KeyUp += MainForm_KeyUp;
            this.DoubleBuffered = true;
        }
        
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            pressedKeys.Add(e.KeyCode);

            int dx = 0, dy = 0;
            if (pressedKeys.Contains(Keys.A)) dx = -1;
            if (pressedKeys.Contains(Keys.D)) dx = 1;
            if (pressedKeys.Contains(Keys.W)) dy = -1;
            if (pressedKeys.Contains(Keys.S)) dy = 1;

            if (dx != 0 || dy != 0)
            {
                player.Move(dx, dy);
                this.Invalidate();
            }

            for (int i = ores.Count - 1; i >= 0; i--)
            {
                Ore ore = ores[i];
                if (Math.Abs(player.X - ore.X) < player.Size && Math.Abs(player.Y - ore.Y) < player.Size)
                {
                    player.Money += ore.Value;
                    ores.RemoveAt(i);
                }
            }
            this.Invalidate();
        }
        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            
            
            Brush playerBrush = Brushes.SaddleBrown;
            g.FillRectangle(playerBrush, player.X, player.Y, player.Size, player.Size);
            g.DrawString($"X:{player.X} Y:{player.Y}", this.Font, Brushes.Black, 10, 10);
            g.DrawString($"Δενόγθ: {player.Money}", this.Font, Brushes.Black, 10, 30);


            foreach (Ore ore in ores) 
            {
                Brush oreBrush = new SolidBrush(ore.OreColor);
                e.Graphics.FillRectangle(oreBrush, ore.X, ore.Y, ore.Size, ore.Size);
                e.Graphics.DrawString($"{ore.Value}", this.Font, Brushes.Black, ore.X, ore.Y - 15);
            }
        }
    }
}

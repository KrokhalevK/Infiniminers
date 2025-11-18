using System.Collections.Generic;
using System.Drawing;

namespace Infiniminers_v0._0
{
    public class GameController
    {
        public Player Player { get; private set; }
        public MapManager MapManager { get; private set; }
        public List<Ore> Ores => MapManager.CurrentOres;

        private readonly Size mapSize;

        public GameController(Size mapSize, int numberOfMaps = 3)
        {
            this.mapSize = mapSize;
            Player = new Player(300, 300);
            MapManager = new MapManager(numberOfMaps, mapSize);
        }

        public void MovePlayer(int dx, int dy)
        {
            Player.Move(dx, dy);

            int x = Player.X;
            int y = Player.Y;

            bool mapChanged = MapManager.TrySwitchMap(ref x, ref y, Player.Size, mapSize);

            if (mapChanged)
            {
                Player.X = x;
                Player.Y = y;
            }
        }

        public void CollectOre()
        {
            for (int i = Ores.Count - 1; i >= 0; i--)
            {
                Ore ore = Ores[i];
                if ((Player.X + Player.Size >= ore.X && ore.X + ore.Size >= Player.X) &&
                    (Player.Y + Player.Size >= ore.Y && ore.Y + ore.Size >= Player.Y))
                {
                    Player.Money += ore.Value;
                    Ores.RemoveAt(i);
                }
            }
        }
    }
}

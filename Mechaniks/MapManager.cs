using System;
using System.Collections.Generic;
using System.Drawing;

namespace Infiniminers_v0._0
{
    public class MapManager
    {
        private Dictionary<int, List<Ore>> maps = new Dictionary<int, List<Ore>>();
        private int currentMap = 0;

        public List<Ore> CurrentOres => maps.ContainsKey(currentMap) ? maps[currentMap] : new List<Ore>();

        public MapManager(int numberOfMaps, Size mapSize)
        {
            for (int i = 0; i < numberOfMaps; i++)
            {
                GenerateMap(i, mapSize);
            }
        }

        public void GenerateMap(int mapNumber, Size mapSize)
        {
            Random rnd = new Random(mapNumber);
            List<Ore> newOres = new List<Ore>();
            int count = rnd.Next(10, 20);

            for (int i = 0; i < count; i++)
            {
                int x = rnd.Next(50, mapSize.Width - 50);
                int y = rnd.Next(50, mapSize.Height - 50);
                int value = rnd.Next(20, 50);
                newOres.Add(new Ore(x, y, value, Color.Gold));
            }
            maps[mapNumber] = newOres;
        }

        public bool TrySwitchMap(ref int playerX, ref int playerY, int playerSize, Size mapSize)
        {
            bool mapChanged = false;

            if (playerX + playerSize / 2 > mapSize.Width)
            {
                currentMap = (currentMap + 1) % maps.Count;
                playerX = 0 - playerSize / 2;
                mapChanged = true;
            }
            else if (playerX + playerSize / 2 < 0)
            {
                currentMap--;
                if (currentMap < 0) currentMap = maps.Count - 1;
                playerX = mapSize.Width - playerSize / 2;
                mapChanged = true;
            }

            if (playerY + playerSize / 2 > mapSize.Height)
            {
                currentMap = (currentMap + 1) % maps.Count;
                playerY = 0 - playerSize / 2;
                mapChanged = true;
            }
            else if (playerY + playerSize / 2 < 0)
            {
                currentMap--;
                if (currentMap < 0) currentMap = maps.Count - 1;
                playerY = mapSize.Height - playerSize / 2;
                mapChanged = true;
            }

            return mapChanged;
        }
    }
}

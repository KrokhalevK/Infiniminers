namespace Infiniminers
{
    public class MapManager
    {
        private Dictionary<int, List<Ore>> maps = new Dictionary<int, List<Ore>>();
        private int currentMap = 0;
        public List<Ore> CurrentOres => maps.ContainsKey(currentMap) ? maps[currentMap] : new List<Ore>();

        private const int MIN_ORES = 10;
        private const int MAX_ORES = 20;

        public MapManager(int numberOfMaps, Size mapSize)
        {
            for (int i = 0; i < numberOfMaps; i++)
                GenerateMap(i, mapSize);
        }

        public void GenerateMap(int mapNumber, Size mapSize)
        {
            Random rnd = new Random(mapNumber);
            List<Ore> newOres = new List<Ore>();

            int oreCount = rnd.Next(MIN_ORES, MAX_ORES);
            for (int i = 0; i < oreCount; i++)
            {
                int x = rnd.Next(50, mapSize.Width - 50);
                int y = rnd.Next(50, mapSize.Height - 50);

                // РАСПРЕДЕЛЕНИЕ ТИПОВ РУД (гарантирован спавн)
                OreType type;
                int rand = rnd.Next(0, 100);

                if (rand < 40)
                    type = OreType.Stone;        // 40% камень
                else if (rand < 70)
                    type = OreType.Iron;         // 30% железо
                else if (rand < 90)
                    type = OreType.Gold;         // 20% золото
                else
                    type = OreType.Diamond;      // 10% алмаз

                newOres.Add(new Ore(x, y, type));
            }

            maps[mapNumber] = newOres;
        }

        public bool TrySwitchMap(ref int playerX, ref int playerY, int playerSize, Size mapSize)
        {
            bool shouldSwitch = false;
            int nextMap = currentMap;
            int padding = 20;  // Отступ для телепортации

            // Переход ВПРАВО
            if (playerX + playerSize >= mapSize.Width)
            {
                nextMap = currentMap + 1;
                playerX = padding;  // Телепортируем в начало слева
                shouldSwitch = true;
            }
            // Переход ВЛЕВО
            else if (playerX <= 0)
            {
                nextMap = currentMap - 1;
                playerX = mapSize.Width - playerSize - padding;  // Телепортируем в конец справа
                shouldSwitch = true;
            }
            // Переход ВНИЗ
            else if (playerY + playerSize >= mapSize.Height)
            {
                nextMap = currentMap + 1000;  // Идём далеко вперед по Z
                playerY = padding;
                shouldSwitch = true;
            }
            // Переход ВВЕРХ
            else if (playerY <= 0)
            {
                nextMap = currentMap - 1000;
                playerY = mapSize.Height - playerSize - padding;
                shouldSwitch = true;
            }

            if (shouldSwitch)
            {
                // Если карта в диапазоне, переходим
                if (nextMap >= 0 && nextMap < maps.Count)
                {
                    currentMap = nextMap;
                    return true;
                }
                else
                {
                    // Если карты нет, генерируем новую
                    if (!maps.ContainsKey(nextMap))
                        GenerateMap(nextMap, mapSize);

                    currentMap = nextMap;
                    return true;
                }
            }

            return false;
        }
    }
}

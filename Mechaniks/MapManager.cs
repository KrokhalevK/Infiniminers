using System;
using System.Collections.Generic;
using System.Drawing;

namespace Infiniminers
{
    /// <summary>
    /// Менеджер бесконечного мира: генерация карт, переходы между картами.
    /// Поддерживает прогрессию сложности по глубине.
    /// </summary>
    public class MapManager
    {
        private Dictionary<int, List<Ore>> maps = new Dictionary<int, List<Ore>>();
        private int currentMap = 0;
        public List<Ore> CurrentOres => maps.ContainsKey(currentMap) ? maps[currentMap] : new List<Ore>();

        // Текущая глубина (для UI и изменения бэкграунда)
        public int CurrentDepth => Math.Abs(currentMap / VERTICAL_MAP_OFFSET);

        // Константы генерации
        private const int MIN_ORES = 10;
        private const int MAX_ORES = 20;

        // Константы спавна руд (базовые значения для глубины 0)
        private const int BASE_STONE_CHANCE = 40;
        private const int BASE_IRON_CHANCE = 30;
        private const int BASE_GOLD_CHANCE = 20;
        private const int BASE_DIAMOND_CHANCE = 10;

        // Константы прогрессии (как меняются проценты с глубиной)
        private const int STONE_DECREASE_PER_DEPTH = 5;
        private const int IRON_DECREASE_PER_DEPTH = 0;  // Железо стабильно
        private const int GOLD_INCREASE_PER_DEPTH = 3;
        // Алмазы = остаток

        // Константы переходов
        private const int VERTICAL_MAP_OFFSET = 1000;
        private const int TELEPORT_PADDING = 20;

        public MapManager(int numberOfMaps, Size mapSize)
        {
            for (int i = 0; i < numberOfMaps; i++)
                GenerateMap(i, mapSize);
        }

        public void GenerateMap(int mapNumber, Size mapSize)
        {
            Random rnd = new Random(mapNumber.GetHashCode());
            List<Ore> newOres = new List<Ore>();

            int oreCount = rnd.Next(MIN_ORES, MAX_ORES);
            for (int i = 0; i < oreCount; i++)
            {
                int x = rnd.Next(50, mapSize.Width - 50);
                int y = rnd.Next(50, mapSize.Height - 50);
                OreType type = GetRandomOreType(rnd, mapNumber);

                newOres.Add(new Ore(x, y, type));
            }

            maps[mapNumber] = newOres;
        }

        /// <summary>
        /// Генерирует тип руды с учётом глубины.
        /// На больших глубинах больше редких руд, меньше камня.
        /// </summary>
        private OreType GetRandomOreType(Random rnd, int mapNumber)
        {
            int depth = Math.Abs(mapNumber / VERTICAL_MAP_OFFSET);
            int rand = rnd.Next(0, 100);

            // Рассчитываем проценты с учётом глубины
            int stoneChance = Math.Max(10, BASE_STONE_CHANCE - depth * STONE_DECREASE_PER_DEPTH);
            int ironChance = stoneChance + Math.Max(15, BASE_IRON_CHANCE - depth * IRON_DECREASE_PER_DEPTH);
            int goldChance = ironChance + Math.Min(30, BASE_GOLD_CHANCE + depth * GOLD_INCREASE_PER_DEPTH);
            // Алмазы = остальное (100 - goldChance)

            if (rand < stoneChance) return OreType.Stone;
            if (rand < ironChance) return OreType.Iron;
            if (rand < goldChance) return OreType.Gold;
            return OreType.Diamond;
        }

        /// <summary>
        /// Возвращает имя текстуры фона для текущей глубины.
        /// Легко расширять для разных уровней.
        /// </summary>
        public string GetBackgroundTextureName()
        {
            int depth = CurrentDepth;

            // Добавь новые уровни по мере рисования текстур
            if (depth == 0) return "background/bg_grass";        // Поверхность
            if (depth == 1) return "background/bg_dirt";         // Земля
            if (depth == 2) return "background/bg_stone";        // Камень
            if (depth == 3) return "background/bg_deepstone";    // Глубокий камень
            if (depth >= 4) return "background/bg_cave";         // Пещеры

            return "background/bg_grass";  // Fallback
        }

        public bool TrySwitchMap(ref int playerX, ref int playerY, int playerSize, Size mapSize)
        {
            bool shouldSwitch = false;
            int nextMap = currentMap;

            // Переход ВПРАВО
            if (playerX + playerSize >= mapSize.Width)
            {
                nextMap = currentMap + 1;
                playerX = TELEPORT_PADDING;
                shouldSwitch = true;
            }
            // Переход ВЛЕВО
            else if (playerX <= 0)
            {
                nextMap = currentMap - 1;
                playerX = mapSize.Width - playerSize - TELEPORT_PADDING;
                shouldSwitch = true;
            }
            // Переход ВНИЗ
            else if (playerY + playerSize >= mapSize.Height)
            {
                nextMap = currentMap + VERTICAL_MAP_OFFSET;
                playerY = TELEPORT_PADDING;
                shouldSwitch = true;
            }
            // Переход ВВЕРХ
            else if (playerY <= 0)
            {
                nextMap = currentMap - VERTICAL_MAP_OFFSET;
                playerY = mapSize.Height - playerSize - TELEPORT_PADDING;
                shouldSwitch = true;
            }

            if (shouldSwitch)
            {
                // Генерируем карту, если её ещё нет
                if (!maps.ContainsKey(nextMap))
                    GenerateMap(nextMap, mapSize);

                currentMap = nextMap;
                return true;
            }

            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Infiniminers
{
    /// <summary>
    /// Основной контроллер игры: управляет игроком, картами, рудами и взаимодействиями.
    /// </summary>
    public class GameController
    {
        public Player Player { get; private set; }
        public MapManager MapManager { get; private set; }
        public List<Ore> Ores => MapManager.CurrentOres;

        private readonly Size mapSize;
        private int contactDamageDelay = 0;

        // Константы
        private const int CONTACT_DAMAGE_DELAY = 20;
        private const int PUSH_DISTANCE_STRAIGHT = 30;
        private const int PUSH_DISTANCE_DIAGONAL = 20;

        public GameController(Size mapSize, int numberOfMaps = 3)
        {
            this.mapSize = mapSize;
            Player = new Player(300, 300);
            MapManager = new MapManager(numberOfMaps, mapSize);
        }

        public void MovePlayer(int dx, int dy)
        {
            int oldX = Player.X;
            int oldY = Player.Y;

            Player.Move(dx, dy);

            HandleOreCollision(oldX, oldY);

            // Проверка смены карты
            int x = Player.X;
            int y = Player.Y;
            bool mapChanged = MapManager.TrySwitchMap(ref x, ref y, Player.Size, mapSize);
            if (mapChanged)
            {
                Player.X = x;
                Player.Y = y;
            }

            UpdateContactDamageDelay();
        }

        private void UpdateContactDamageDelay()
        {
            if (contactDamageDelay > 0)
                contactDamageDelay--;
        }

        private void HandleOreCollision(int oldX, int oldY)
        {
            List<Ore> destroyedOres = new List<Ore>();

            foreach (var ore in Ores)
            {
                if (IsCollidingWithOre(ore))
                {
                    PushPlayerAway(oldX, oldY);
                    ApplyContactDamage(ore, destroyedOres);
                }
            }

            RemoveDestroyedOres(destroyedOres);
        }

        private bool IsCollidingWithOre(Ore ore)
        {
            return (Player.X + Player.Size > ore.X && Player.X < ore.X + ore.Size) &&
                   (Player.Y + Player.Size > ore.Y && Player.Y < ore.Y + ore.Size);
        }

        private void PushPlayerAway(int oldX, int oldY)
        {
            // Определяем направление движения игрока
            int dx = Player.X - oldX;
            int dy = Player.Y - oldY;

            // Толкаем в противоположную сторону движения
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                // Двигался больше горизонтально
                Player.X = oldX - (dx > 0 ? PUSH_DISTANCE_STRAIGHT : -PUSH_DISTANCE_STRAIGHT);
            }
            else if (Math.Abs(dy) > Math.Abs(dx))
            {
                // Двигался больше вертикально
                Player.Y = oldY - (dy > 0 ? PUSH_DISTANCE_STRAIGHT : -PUSH_DISTANCE_STRAIGHT);
            }
            else
            {
                // Двигался диагонально
                Player.X = oldX - (dx > 0 ? PUSH_DISTANCE_DIAGONAL : -PUSH_DISTANCE_DIAGONAL);
                Player.Y = oldY - (dy > 0 ? PUSH_DISTANCE_DIAGONAL : -PUSH_DISTANCE_DIAGONAL);
            }
        }

        private void ApplyContactDamage(Ore ore, List<Ore> destroyedOres)
        {
            if (contactDamageDelay <= 0)
            {
                int damage = ore.TakeDamage(Player.GetTotalDamage());
                contactDamageDelay = CONTACT_DAMAGE_DELAY;

                if (ore.IsDestroyed)
                {
                    Player.Money += ore.Data.Value;
                    destroyedOres.Add(ore);
                }
            }
        }

        public void CollectOre()
        {
            List<Ore> destroyedOres = new List<Ore>();

            foreach (var ore in Ores)
            {
                if (IsInMiningRange(ore))
                {
                    int damage = ore.TakeDamage(Player.GetTotalDamage());

                    if (ore.IsDestroyed)
                    {
                        Player.Money += ore.Data.Value;
                        destroyedOres.Add(ore);
                    }
                }
            }

            RemoveDestroyedOres(destroyedOres);
        }

        private bool IsInMiningRange(Ore ore)
        {
            int range = Player.MiningRange;
            return (Player.X + Player.Size + range >= ore.X &&
                    Player.X - range <= ore.X + ore.Size) &&
                   (Player.Y + Player.Size + range >= ore.Y &&
                    Player.Y - range <= ore.Y + ore.Size);
        }

        private void RemoveDestroyedOres(List<Ore> destroyedOres)
        {
            var destroyedSet = new HashSet<Ore>(destroyedOres);
            Ores.RemoveAll(ore => destroyedSet.Contains(ore));
        }
    }
}

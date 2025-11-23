namespace Infiniminers
{
    public class GameController
    {
        public Player Player { get; private set; }
        public MapManager MapManager { get; private set; }
        public List<Ore> Ores => MapManager.CurrentOres;

        private readonly Size mapSize;
        private int contactDamageDelay = 0;
        private const int CONTACT_DAMAGE_DELAY = 20;
        private const int MINING_RANGE_BONUS = 50;

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
            int x = Player.X;
            int y = Player.Y;

            bool mapChanged = MapManager.TrySwitchMap(ref x, ref y, Player.Size, mapSize);
            if (mapChanged)
            {
                Player.X = x;
                Player.Y = y;
            }
            HandleMapTransition();
            UpdateContactDamageDelay();
        }

        private void HandleMapTransition()
        {
            int x = Player.X;
            int y = Player.Y;

            bool mapChanged = MapManager.TrySwitchMap(ref x, ref y, Player.Size, mapSize);
            if (mapChanged)
            {
                Player.X = x;
                Player.Y = y;
            }
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
                    PushPlayerAwayImproved(oldX, oldY);
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

        private void PushPlayerAwayImproved(int oldX, int oldY)
        {
            // Определяем направление движения игрока
            int dx = Player.X - oldX;
            int dy = Player.Y - oldY;

            // Толкаем в противоположную сторону движения
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                // Двигался больше горизонтально
                Player.X = oldX - (dx > 0 ? 30 : -30);
            }
            else if (Math.Abs(dy) > Math.Abs(dx))
            {
                // Двигался больше вертикально
                Player.Y = oldY - (dy > 0 ? 30 : -30);
            }
            else
            {
                // Двигался диагонально
                Player.X = oldX - (dx > 0 ? 20 : -20);
                Player.Y = oldY - (dy > 0 ? 20 : -20);
            }
        }

        private void ApplyContactDamage(Ore ore, List<Ore> destroyedOres)
        {
            if (contactDamageDelay <= 0)
            {
                int damage = ore.TakeDamage(Player.GetAttackPower());
                contactDamageDelay = CONTACT_DAMAGE_DELAY;

                if (ore.IsDestroyed)
                {
                    Player.Money += ore.GetValue();
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
                    int damage = ore.TakeDamage(Player.GetAttackPower());

                    if (ore.IsDestroyed)
                    {
                        Player.Money += ore.GetValue();
                        destroyedOres.Add(ore);
                    }
                }
            }

            RemoveDestroyedOres(destroyedOres);
        }

        private bool IsInMiningRange(Ore ore)
        {
            return (Player.X + Player.Size + MINING_RANGE_BONUS >= ore.X &&
                    Player.X - MINING_RANGE_BONUS <= ore.X + ore.Size) &&
                   (Player.Y + Player.Size + MINING_RANGE_BONUS >= ore.Y &&
                    Player.Y - MINING_RANGE_BONUS <= ore.Y + ore.Size);
        }

        private void RemoveDestroyedOres(List<Ore> destroyedOres)
        {
            foreach (var ore in destroyedOres)
                Ores.Remove(ore);
        }
    }
}

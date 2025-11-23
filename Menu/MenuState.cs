namespace Infiniminers
{
    /// <summary>
    /// Состояния игры: меню, настройки, геймплей и магазин.
    /// </summary>
    public enum GameState
    {
        MainMenu,       // Главное меню
        Settings,       // Настройки
        ResourcePacks,  // Выбор ресурс-паков
        Playing,        // Игровой процесс
        Paused,         // Меню паузы
        Shop            // Магазин кирок
    }
}

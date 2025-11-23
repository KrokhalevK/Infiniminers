using System;
using System.Collections.Generic;

namespace Infiniminers
{
    /// <summary>
    /// Контроллер управления меню и состояниями игры.
    /// </summary>
    public class MenuController
    {
        private GameState currentState = GameState.MainMenu;
        private GameState previousState = GameState.MainMenu;  // ← НОВОЕ: отслеживаем предыдущее состояние

        public GameState CurrentState
        {
            get => currentState;
            set
            {
                previousState = currentState;  // ← Сохраняем предыдущее при изменении
                currentState = value;
            }
        }

        private int menuSelectionIndex = 0;
        private ResourcePackManager resourcePackManager = null!;

        public void MoveMenuSelection(int direction)
        {
            switch (currentState)
            {
                case GameState.MainMenu:
                    CycleSelection(direction, GetMenuOptionsCount<MenuOption>());
                    break;
                case GameState.Settings:
                    CycleSelection(direction, GetMenuOptionsCount<SettingsOption>());
                    break;
                case GameState.Paused:
                    CycleSelection(direction, GetMenuOptionsCount<PauseMenuOption>());
                    break;
            }
        }

        private void CycleSelection(int direction, int maxIndex)
        {
            menuSelectionIndex += direction;
            if (menuSelectionIndex < 0)
                menuSelectionIndex = maxIndex;
            if (menuSelectionIndex > maxIndex)
                menuSelectionIndex = 0;
        }

        private int GetMenuOptionsCount<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Length - 1;
        }

        public int GetSelectedMenuIndex() => menuSelectionIndex;

        public void SelectOption()
        {
            if (currentState == GameState.MainMenu)
            {
                MenuOption option = GetSelectedOption();
                switch (option)
                {
                    case MenuOption.Play:
                        currentState = GameState.Playing;
                        break;
                    case MenuOption.Settings:
                        currentState = GameState.Settings;
                        break;
                    case MenuOption.Exit:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        public MenuOption GetSelectedOption()
        {
            return (MenuOption)menuSelectionIndex;
        }

        public SettingsOption GetSelectedSettingsOption()
        {
            return (SettingsOption)menuSelectionIndex;
        }

        public PauseMenuOption GetSelectedPauseOption()
        {
            return (PauseMenuOption)menuSelectionIndex;
        }

        public void PauseGame()
        {
            currentState = GameState.Paused;
            menuSelectionIndex = 0;
        }

        public void OpenShop()
        {
            currentState = GameState.Shop;
        }

        public void OpenResourcePacks()
        {
            currentState = GameState.ResourcePacks;
            menuSelectionIndex = 0;
        }

        public void CloseResourcePacks()
        {
            BackToPreviousState();  // ← Возвращаемся на предыдущий экран
        }

        public void BackToMenu()
        {
            currentState = GameState.MainMenu;
            previousState = GameState.MainMenu;
            menuSelectionIndex = 0;
        }

        /// <summary>
        /// Возврат на предыдущий экран (из настроек).
        /// </summary>
        public void BackToPreviousState()
        {
            // Если пришли из игры (паузы), вернуться в паузу
            if (previousState == GameState.Playing || previousState == GameState.Paused)
                currentState = GameState.Paused;
            // Если пришли из главного меню, вернуться туда
            else
                currentState = GameState.MainMenu;

            menuSelectionIndex = 0;
        }

        /// <summary>
        /// Проверяет, пришли ли в настройки из игры.
        /// </summary>
        public bool IsFromGame()
        {
            return previousState == GameState.Playing || previousState == GameState.Paused;
        }

        public void InitializeResourcePackManager(ResourceManager resourceManager)
        {
            resourcePackManager = new ResourcePackManager(resourceManager);
        }

        public void MoveResourcePackSelection(int direction)
        {
            if (direction > 0)
                resourcePackManager.NextPack();
            else
                resourcePackManager.PreviousPack();
        }

        public int GetResourcePackSelectedIndex() => resourcePackManager.GetSelectedPackIndex();

        public List<string> GetAvailableResourcePacks() => resourcePackManager.GetAvailablePacks();

        // Enum'ы опций меню
        public enum MenuOption
        {
            Play,
            Settings,
            Exit
        }

        public enum SettingsOption
        {
            ResourcePacks
            // Audio (в будущем)
            // Controls (в будущем)
        }

        public enum PauseMenuOption
        {
            Resume,
            Shop,
            Settings,
            MainMenu
        }
    }
}

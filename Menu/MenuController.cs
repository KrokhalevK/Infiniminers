using System;
using System.Collections.Generic;

namespace Infiniminers
{
    public class MenuController
    {
        private GameState currentState = GameState.MainMenu;
        public GameState CurrentState
        {
            get => currentState;
            set => currentState = value;
        }

        private int menuSelectionIndex = 0;
        private ResourcePackManager resourcePackManager = null!;

        public void MoveMenuSelection(int direction)
        {
            if (currentState == GameState.MainMenu)
            {
                menuSelectionIndex += direction;
                if (menuSelectionIndex < 0) menuSelectionIndex = 2;
                if (menuSelectionIndex > 2) menuSelectionIndex = 0;
            }
            else if (currentState == GameState.Settings)
            {
                menuSelectionIndex += direction;
                if (menuSelectionIndex < 0) menuSelectionIndex = 2;
                if (menuSelectionIndex > 2) menuSelectionIndex = 0;
            }
            else if (currentState == GameState.Paused)
            {
                menuSelectionIndex += direction;
                if (menuSelectionIndex < 0) menuSelectionIndex = 2;
                if (menuSelectionIndex > 2) menuSelectionIndex = 0;
            }
        }

        public int GetSelectedMenuIndex() => menuSelectionIndex;

        // ← ДОБАВИТЬ этот метод
        public void SelectOption()
        {
            if (currentState == GameState.MainMenu)
            {
                MenuOption option = GetSelectedOption();
                if (option == MenuOption.Play)
                    currentState = GameState.Playing;
                else if (option == MenuOption.Settings)
                    currentState = GameState.Settings;
                else if (option == MenuOption.Exit)
                    Environment.Exit(0);
            }
        }

        public MenuOption GetSelectedOption()
        {
            return (MenuOption)menuSelectionIndex;
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
            currentState = GameState.Settings;
            menuSelectionIndex = 0;
        }

        public void BackToMenu()
        {
            currentState = GameState.MainMenu;
            menuSelectionIndex = 0;
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

        public enum MenuOption
        {
            Play,
            Settings,
            Exit
        }

        public enum PauseMenuOption
        {
            Resume,
            Shop,
            MainMenu
        }
    }
}

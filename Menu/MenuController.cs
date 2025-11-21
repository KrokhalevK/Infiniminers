namespace Infiniminers_v0._0
{
    public class MenuController
    {
        public GameState CurrentState { get; set; } = GameState.MainMenu;
        private int selectedMenuIndex = 0;

        public enum MenuOption
        {
            StartGame,
            Settings,
            Exit
        }

        public enum PauseMenuOption
        {
            Resume,
            MainMenu,
            Exit
        }

        public MenuOption GetSelectedOption()
        {
            return (MenuOption)selectedMenuIndex;
        }

        public PauseMenuOption GetSelectedPauseOption()
        {
            return (PauseMenuOption)selectedMenuIndex;
        }

        public void MoveMenuSelection(int direction)
        {
            selectedMenuIndex += direction;
            if (selectedMenuIndex < 0) selectedMenuIndex = 2;
            if (selectedMenuIndex > 2) selectedMenuIndex = 0;
        }

        public void SelectOption()
        {
            MenuOption option = GetSelectedOption();
            switch (option)
            {
                case MenuOption.StartGame:
                    CurrentState = GameState.Playing;
                    break;
                case MenuOption.Settings:
                    CurrentState = GameState.Settings;
                    break;
                case MenuOption.Exit:
                    break;
            }
        }

        public void SelectPauseOption()
        {
            PauseMenuOption option = GetSelectedPauseOption();
            switch (option)
            {
                case PauseMenuOption.Resume:
                    CurrentState = GameState.Playing;
                    break;
                case PauseMenuOption.MainMenu:
                    CurrentState = GameState.MainMenu;
                    selectedMenuIndex = 0;
                    break;
                case PauseMenuOption.Exit:
                    break;
            }
        }

        public void BackToMenu()
        {
            CurrentState = GameState.MainMenu;
            selectedMenuIndex = 0;
        }

        public void PauseGame()
        {
            CurrentState = GameState.Paused;
            selectedMenuIndex = 0;
        }

        public int GetSelectedMenuIndex()
        {
            return selectedMenuIndex;
        }
    }
}

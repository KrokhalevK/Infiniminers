using System;
using System.Collections.Generic;
using System.Drawing;

namespace Infiniminers
{
    public class Button
    {
        public string Name { get; set; }
        public Rectangle Bounds { get; set; }
        public bool IsHovered { get; set; }
        public bool IsSelected { get; set; }  // ← ДОБАВИТЬ для клавиатуры
        public Action? OnClick { get; set; }

        public Button(string name, Rectangle bounds, Action? onClick = null)
        {
            Name = name;
            Bounds = bounds;
            OnClick = onClick;
            IsHovered = false;
            IsSelected = false;
        }

        public bool IsMouseOver(Point mousePos)
        {
            return Bounds.Contains(mousePos);
        }

        public void Click()
        {
            OnClick?.Invoke();
        }
    }

    public class ButtonManager
    {
        private List<Button> buttons = new List<Button>();
        private int selectedIndex = 0;

        public void AddButton(Button button)
        {
            buttons.Add(button);
            if (buttons.Count == 1)
                buttons[0].IsSelected = true;  // Первая кнопка выбрана по умолчанию
        }

        public void ClearButtons()
        {
            buttons.Clear();
            selectedIndex = 0;
        }

        public Button? GetButtonAtPosition(Point mousePos)
        {
            foreach (var button in buttons)
            {
                if (button.IsMouseOver(mousePos))
                    return button;
            }
            return null;
        }

        public void UpdateHoverState(Point mousePos)
        {
            foreach (var button in buttons)
            {
                button.IsHovered = button.IsMouseOver(mousePos);
            }
        }

        // ← НОВАЯ: Навигация с клавиатуры
        public void SelectNext()
        {
            if (buttons.Count == 0) return;

            buttons[selectedIndex].IsSelected = false;
            selectedIndex = (selectedIndex + 1) % buttons.Count;
            buttons[selectedIndex].IsSelected = true;
        }

        public void SelectPrevious()
        {
            if (buttons.Count == 0) return;

            buttons[selectedIndex].IsSelected = false;
            selectedIndex = (selectedIndex - 1 + buttons.Count) % buttons.Count;
            buttons[selectedIndex].IsSelected = true;
        }

        // ← НОВАЯ: Нажать на выбранную кнопку
        public void ClickSelected()
        {
            if (buttons.Count > 0 && selectedIndex < buttons.Count)
            {
                buttons[selectedIndex].Click();
            }
        }

        // ← НОВАЯ: Нажать на кнопку по индексу (для прямого клика)
        public void ClickAtIndex(int index)
        {
            if (index >= 0 && index < buttons.Count)
            {
                buttons[index].Click();
            }
        }

        public List<Button> GetAllButtons() => new List<Button>(buttons);
        public int GetSelectedIndex() => selectedIndex;
    }
}

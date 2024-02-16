using Microsoft.Win32;
using System.CodeDom;
using System.Windows;
using System.Windows.Controls;

namespace EmployeeAtestation.Pages
{
    public static class Navigation
    {
        private static Stack<Page> _stack = new();

        public static SaveFileDialog SaveFileDialog { get; set; } = new();

        // public static Page? Current => _stack.Count != 0 ? _stack.Peek() : null;

        public static void Push(Page page)
        {
            _stack.Push(page);
            Application.Current.MainWindow.Content = page;
        }

        public static void Pop()
        {
            _stack.Pop();
            Application.Current.MainWindow.Content = _stack.Peek();
        }

        public static void PopTo<TPage>() where TPage : Page
        {
            if (_stack.Where(p => p.GetType() == typeof(TPage)).Count() == 0)
            {
                throw new InvalidOperationException($"Has no page type {typeof(TPage).Name} in stack");
            }

            while (_stack.Peek().GetType() != typeof(TPage))
            {
                _stack.Pop();
            }

            Application.Current.MainWindow.Content = _stack.Peek();
        }

        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static MessageBoxResult ShowYesNo(string message)
        {
            return MessageBox.Show(message, "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}

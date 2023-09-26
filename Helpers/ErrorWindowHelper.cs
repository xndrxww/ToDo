using ToDo.Windows;

namespace ToDo.Helpers
{
    public static class ErrorWindowHelper
    {
        public static void ShowError(string errorText)
        {
            var errorWindow = new ErrorWindow(errorText);
            errorWindow.Topmost = true;
            errorWindow.Show();
        }
    }
}

using ToDo.Windows;

namespace ToDo.Helpers
{
    public class ErrorWindowHelper
    {
        public static void ShowError(string errorText)
        {
            var errorWindow = new ErrorWindow(errorText)
            {
                Topmost = true
            };
            errorWindow.Show();
        }
    }
}

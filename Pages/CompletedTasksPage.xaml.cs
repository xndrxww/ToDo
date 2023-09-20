using System.Linq;
using System.Windows.Controls;

namespace ToDo.Pages
{
    public partial class CompletedTasksPage : Page
    {
        public CompletedTasksPage()
        {
            InitializeComponent();

            MainWindow.CompletedTasksStackPanel = completedTasksStackPanel;
            MainWindow.RefreshCompletedTasksStackPanel();
        }
    }
}

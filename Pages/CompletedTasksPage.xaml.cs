using System.Windows.Controls;

namespace ToDo.Pages
{
    /// <summary>
    /// Interaction logic for CompletedTasksPage.xaml
    /// </summary>
    public partial class CompletedTasksPage : Page
    {
        public CompletedTasksPage()
        {
            InitializeComponent();
            MainWindow.CompletedTasksStackPanel = completedTasksStackPanel;
            MainWindow.LoadTasks(null);
        }
    }
}

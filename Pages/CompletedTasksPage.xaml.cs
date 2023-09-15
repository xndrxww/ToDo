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
            
            //TODO
            var group = MainWindow.GroupsList.Where(g => g.Name == MainWindow.CurrentPageName).FirstOrDefault();
            MainWindow.RefreshTasksStackPanel(group.Tasks);
        }
    }
}

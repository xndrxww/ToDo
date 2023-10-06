using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToDo.Controls;
using ToDo.Helpers;
using ToDo.Models;

namespace ToDo.Windows
{
    public partial class EditTaskWindow : Window
    {
        private Models.Task Task;

        public EditTaskWindow(Models.Task taskModel)
        {
            InitializeComponent();

            Task = taskModel;
            SetData();
        }

        private void saveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var currentGroup = GroupHelper.GetGroup();

            var task = TaskHelper.GetTask(currentGroup, Task.Id);
            task.Name = taskNameText.Text;
            task.Description = taskDescriptionText.Text;
            task.DeadLine = taskDeadLineTime.SelectedDate;
            task.Files = task.Files;

            Close();

            MainWindow.CheckTaskAndRefresh(currentGroup, Task);
        }

        private void deadLineMenuItem_Click(object sender, RoutedEventArgs e)
        {
            taskDeadLineTime.IsDropDownOpen = true;
        }

        private void deadLineTime_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            deadLineMenuItem.Header = taskDeadLineTime.SelectedDate.Value.ToString("dd.MM.yyyy");
        }

        private void SetData()
        {
            MainWindow.FilesStackPanel = filesStackPanel;
            taskNameText.Text = Task.Name;
            taskDescriptionText.Text = Task.Description;
            taskDeadLineTime.SelectedDate = Task.DeadLine;
            SetFiles();
        }

        private void SetFiles()
        {
            if (Task.Files?.Any() == true)
            {
                Height = 500;
                bottonStackPanel.Visibility = Visibility.Visible;
                foreach (var file in Task.Files)
                {
                    MainWindow.FilesStackPanel.Children.Add(new FilesControl(file, Task));
                }
            }
        }
    }
}

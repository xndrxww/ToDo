using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToDo.Controls;

namespace ToDo.Windows
{
    /// <summary>
    /// Interaction logic for EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        Models.Task Task;

        public EditTaskWindow(Models.Task taskModel)
        {
            InitializeComponent();

            Task = taskModel;
            taskNameText.Text = Task.Name;
            taskDescriptionText.Text = Task.Description;
            taskDeadLineTime.SelectedDate = Task.DeadLine;

            //изменить
            MainWindow.FilesStackPanel = filesStackPanel;
            if (Task.Files != null && Task.Files.Count > 0)
            {
                Height = 500;
                bottonStackPanel.Visibility = Visibility.Visible;
                MainWindow.LoadFiles(Task);
            }
        }

        private void saveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var task in MainWindow.TasksList.Where(task => !task.IsCompleted && task.Id == Task.Id))
            {
                task.Name = taskNameText.Text;
                task.Description = taskDescriptionText.Text;
                task.DeadLine = taskDeadLineTime.SelectedDate;
            }
            Close();
            MainWindow.LoadTasks(null);
        }

        private void deadLineMenuItem_Click(object sender, RoutedEventArgs e)
        {
            taskDeadLineTime.IsDropDownOpen = true;
        }

        private void deadLineTime_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            deadLineMenuItem.Header = taskDeadLineTime.SelectedDate.Value.ToString("dd.MM.yyyy");
        }

        private void LoadFiles()
        {
            if (Task.Files != null && Task.Files.Count > 0)
            {
                Height = 500;
                bottonStackPanel.Visibility = Visibility.Visible;
                foreach (var file in Task.Files)
                {
                    filesStackPanel.Children.Add(new FilesControl(file, Task));
                }
            }
        }
    }
}

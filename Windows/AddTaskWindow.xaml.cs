using System.Windows;

namespace ToDo.Windows
{
    /// <summary>
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        public AddTaskWindow()
        {
            InitializeComponent();
        }

        private void addTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var task = new Models.Task
            {
                Id = MainWindow.TasksList.Count + 1,
                Name = taskNameText.Text,
                Description = taskDescriptionText.Text,
                DeadLine = deadLineTime.SelectedDate
            };

            MainWindow.TasksList.Add(task);
            Close();
        }

        private void closeWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

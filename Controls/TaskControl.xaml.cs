using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ToDo.Windows;

namespace ToDo.Controls
{
    /// <summary>
    /// Interaction logic for TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl
    {
        Models.Task Task;
        public TaskControl(Models.Task taskModel)
        {
            InitializeComponent();
            Task = taskModel;
            taskControl.Name = Task.ControlName;
            DataContext = CheckData();
        }

        private void completeTaskCheck_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var task in MainWindow.TasksList.Where(task => !task.IsCompleted && task.ControlName == taskControl.Name))
            {
                task.IsCompleted = true;
            }

            MainWindow.LoadTasks(null);
        }

        private Models.Task CheckData()
        {
            if (Task.DeadLine != null && Task.DeadLine < DateTime.Now)
                Task.IsOverdue = true;
            else
                Task.IsOverdue = false;

            if (Task.DeadLine != null)
                taskDeadLineText.Text = Task.DeadLine.Value.ToShortDateString();

            if (Task.IsOverdue)
                taskDeadLineText.Foreground = new SolidColorBrush(Colors.Salmon);

            if (Task.IsPriority)
            {
                priorityTaskImage.Source = new BitmapImage(new Uri(@"\Icons\filled_star.png", UriKind.Relative));
                priorityTaskImage.Opacity = 1;
            }
            else
            {
                priorityTaskImage.Source = new BitmapImage(new Uri(@"\Icons\star.png", UriKind.Relative));
                priorityTaskImage.Opacity = 0.3;
            }

            if (Task.IsCompleted)
                completeTaskCheck.Visibility = Visibility.Collapsed;


            return Task;
        }

        private void deleteTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.TasksList.Remove(Task);
            MainWindow.LoadTasks(null);
        }

        private void EditTask()
        {
            if (!string.IsNullOrEmpty(taskDeadLineText.Text))
                Task.DeadLine = DateTime.Parse(taskDeadLineText.Text);
            else
                Task.DeadLine = DateTime.Now;

            EditTaskWindow editTaskWindow = new EditTaskWindow(Task);
            editTaskWindow.ShowDialog();
        }

        private void editTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EditTask();
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditTask();
        }

        private void copyTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var copyTask = new Models.Task
            {
                Id = MainWindow.TasksList.Count + 1,
                Name = Task.Name,
                Description = Task.Description,
                DeadLine = Task.DeadLine,
                IsCompleted = Task.IsCompleted,
                IsOverdue = Task.IsOverdue
            };
            MainWindow.TasksList.Add(copyTask);
            MainWindow.LoadTasks(null);
        }


        private void priorityTaskImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Task.IsPriority)
                Task.IsPriority = false;
            else
                Task.IsPriority = true;

            MainWindow.LoadTasks(null);
        }
    }
}

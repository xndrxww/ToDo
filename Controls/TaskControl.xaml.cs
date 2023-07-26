using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            Task = taskModel ;
            taskControl.Name = Task.ControlName;
            DataContext = CheckData();
        }

        private void completeTaskCheck_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var task in MainWindow.TasksList.Where(task => !task.IsCompleted && task.ControlName == taskControl.Name))
            {
                    task.IsCompleted = true;
            }

            MainWindow.LoadTasks();
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

             //if (Task.IsCompleted)
             //   completeTaskCheck.IsChecked = true;

            return Task;
        }

        private void deleteTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.TasksList.Remove(Task);
            MainWindow.LoadTasks();
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
            MainWindow.LoadTasks();
        }
    }
}

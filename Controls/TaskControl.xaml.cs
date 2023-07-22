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
        public TaskControl(Models.Task taskModel)
        {
            InitializeComponent();
            SetData(taskModel);
        }

        private void completeTaskCheck_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var task in MainWindow.TasksList.Where(task => !task.IsCompleted && task.Id == taskControl.Name))
            {
                    task.IsCompleted = true;
            }

            MainWindow.LoadTasks();
        }

        private void SetData(Models.Task taskModel)
        {
            taskControl.Name = taskModel.Id;

            taskNameText.Text = taskModel.Name;
            taskDesciptionText.Text = taskModel.Description;
            
            if (taskModel.DeadLine != null)
                taskDeadLineText.Text = taskModel.DeadLine.Value.ToShortDateString();

            if (taskModel.IsOverdue)
                taskDeadLineText.Foreground = new SolidColorBrush(Colors.Salmon);

            //if (taskModel.IsCompleted)
            //    completeTaskCheck.IsChecked = true;
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var taskModel = new Models.Task
            {
                Id = taskControl.Name,
                Name = taskNameText.Text,
                Description = taskDesciptionText.Text,
                DeadLine = !string.IsNullOrEmpty(taskDeadLineText.Text) ? DateTime.Parse(taskDeadLineText.Text) : DateTime.Now
            };

            EditTaskWindow editTaskWindow = new EditTaskWindow(taskModel);
            editTaskWindow.ShowDialog();
        }
    }
}

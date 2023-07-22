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
using System.Windows.Shapes;

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
            MainWindow.LoadTasks();
        }
    }
}

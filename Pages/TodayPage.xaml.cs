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
using ToDo.Controls;
using ToDo.Windows;

namespace ToDo.Pages
{
    /// <summary>
    /// Interaction logic for TodayPage.xaml
    /// </summary>
    public partial class TodayPage : Page
    {
        public TodayPage()
        {
            InitializeComponent();
            MainWindow.TasksStackPanel = tasksStackPanel;
            MainWindow.LoadTasks();
            SetTodayDate();
        }

        private void addTaskButton_Click(object sender, RoutedEventArgs e)
        {
            AddTaskWindow addTaskWindow = new AddTaskWindow();
            addTaskWindow.ShowDialog();

            MainWindow.LoadTasks();
        }

        private void SetTodayDate()
        {
            todayDateText.Text = DateTime.Now.ToShortDateString();
        }

        private void searhText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(searhText.Text))
            {
                string search = searhText.Text;

                tasksStackPanel.Children.Clear();

                foreach (var task in MainWindow.TasksList)
                {
                    if (!task.IsCompleted && (task.Name.ToLower().StartsWith(search.Trim().ToLower()) || task.Description.ToLower().StartsWith(search.Trim().ToLower())))
                    {
                        Models.Task taskModel = new Models.Task
                        {
                            Name = task.Name,
                            Description = task.Description,
                            DeadLine = task.DeadLine
                        };

                        var taskControl = new TaskControl(taskModel);
                        tasksStackPanel.Children.Add(taskControl);
                    }
                }
            }

            if (string.IsNullOrEmpty(searhText.Text))
                MainWindow.LoadTasks();
        }
    }
}

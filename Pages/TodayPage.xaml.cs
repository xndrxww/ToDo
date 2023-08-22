using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            MainWindow.LoadTasks(null);
            SetTodayDate();
        }

        private void addTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = new AddTaskWindow();
            addTaskWindow.ShowDialog();

            MainWindow.LoadTasks(null);
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
                        if (task.DeadLine != null && task.DeadLine < DateTime.Now)
                            task.IsOverdue = true;

                        var taskControl = new TaskControl(task);
                        tasksStackPanel.Children.Add(taskControl);
                    }
                }
            }

            if (string.IsNullOrEmpty(searhText.Text))
                MainWindow.LoadTasks(null);
        }

        private void sortTaskByDateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var tasksList = MainWindow.TasksList.OrderBy(t => t.DeadLine.HasValue).ThenBy(p => p.DeadLine).ToList();
            MainWindow.LoadTasks(tasksList);
        }

        private void sortTaskByPriority_Click(object sender, RoutedEventArgs e)
        {
            var tasksList = MainWindow.TasksList.OrderByDescending(t => t.IsPriority).ToList();
            MainWindow.LoadTasks(tasksList);
        }
    }
}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ToDo.Controls;
using ToDo.Models;
using ToDo.Windows;

namespace ToDo.Pages
{
    public partial class TodayPage : Page
    {
        private Group Group;

        public TodayPage(Group group)
        {
            InitializeComponent();

            Initialization(group);
            LoadData();
        }

        private void Initialization(Group group)
        {
            Group = group;
            MainWindow.CurrentGroupId = Group.Id;
            if (Group.IsDefault && Group.Name == "Сегодня")
                SetTodayDate();

            MainWindow.TasksStackPanel = tasksStackPanel;
        }

        private void LoadData()
        {
            MainWindow.RefreshTasksStackPanel(Group.Tasks);

            pageNameText.Text = Group.Name;

            if (!Group.IsDefault && Group.Name != "Сегодня")
                todayDateText.Visibility = Visibility.Collapsed;
        }

        private void addTaskMenu_Click(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = new AddTaskWindow();
            addTaskWindow.ShowDialog();
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

                if (Group?.Tasks?.Any() == true)
                {
                    foreach (var task in Group.Tasks)
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
            }
            else
            {
                MainWindow.RefreshTasksStackPanel(Group.Tasks);
            }
        }

        private void sortTaskByDateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var tasks = Group.Tasks.OrderBy(t => t.DeadLine.HasValue).ThenBy(p => p.DeadLine).ToList();
            MainWindow.RefreshTasksStackPanel(tasks);
        }

        private void sortTaskByPriority_Click(object sender, RoutedEventArgs e)
        {
            var tasks = Group.Tasks.OrderByDescending(t => t.IsPriority).ToList();
            MainWindow.RefreshTasksStackPanel(tasks);
        }
    }
}

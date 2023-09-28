using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ToDo.Controls;
using ToDo.Helpers;
using ToDo.Models;
using ToDo.Windows;

namespace ToDo.Pages
{
    public partial class TodayPage : Page
    {
        private Group Group;

        public TodayPage(Group group = null)
        {
            InitializeComponent();

            Initialization(group);
            LoadData();
        }

        private void Initialization(Group group)
        {
            if (group != null)
            {
                Group = group;
                MainWindow.CurrentGroupId = Group.Id;
                MainWindow.CurrentPageName = group.Name;
            }
            else
            {
                SetTodayDate();
                MainWindow.CurrentGroupId = Guid.Empty;
                Group = GroupHelper.GetGroup(MainWindow.CurrentGroupId);
                MainWindow.CurrentPageName = "Сегодня";
            }

            MainWindow.TasksStackPanel = tasksStackPanel;
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
                if (Group?.Tasks?.Any() == true)
                    MainWindow.RefreshTasksStackPanel(Group.Tasks);
            }
        }

        private void sortTaskByDateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Group != null)
            {
                var tasks = Group.Tasks.OrderBy(t => t.DeadLine.HasValue).ThenBy(p => p.DeadLine).ToList();
                MainWindow.RefreshTasksStackPanel(tasks);
            }
        }

        private void sortTaskByPriority_Click(object sender, RoutedEventArgs e)
        {
            if (Group != null)
            {
                var tasks = Group.Tasks.OrderByDescending(t => t.IsPriority).ToList();
                MainWindow.RefreshTasksStackPanel(tasks);
            }
        }

        private void LoadData()
        {
            if (MainWindow.GroupsList?.Any() == true)
            {
                if (MainWindow.CurrentPageName == "Сегодня")
                {
                    var group = GroupHelper.GetGroup("Сегодня");
                    
                    if (group != null) 
                        MainWindow.RefreshTasksStackPanel(group.Tasks);
                }
                else
                {
                    pageNameText.Text = Group.Name;
                    todayDateText.Visibility = Visibility.Collapsed;
                    MainWindow.RefreshTasksStackPanel(Group.Tasks);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
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
    public partial class TaskControl : UserControl
    {
        private Models.Task Task;

        public TaskControl(Models.Task taskModel)
        {
            InitializeComponent();
            Task = taskModel;
            DataContext = SetData();
        }

        private void completeTaskCheck_Checked(object sender, RoutedEventArgs e)
        {
            var currentGroup = MainWindow.GroupsList.Where(g => g.Name == MainWindow.CurrentPageName).FirstOrDefault();

            if (currentGroup.Tasks != null)
            {
                foreach (var task in currentGroup.Tasks.Where(task => !task.IsCompleted && task.ControlName == taskControl.Name))
                {
                    task.IsCompleted = true;
                }
            }
            
            MainWindow.RefreshTasksStackPanel(currentGroup.Tasks);
        }

        private Models.Task SetData()
        {
            taskControl.Name = Task.ControlName;

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
            {
                completeTaskCheck.Visibility = Visibility.Collapsed;
                restoreTaskMenuItem.Visibility = Visibility.Visible;
            }

            if (Task.Files != null && Task.Files.Count > 0)
                attachedFilesStackPanel.Visibility = Visibility.Visible;

            return Task;
        }

        private void deleteTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var currentGroup = MainWindow.GroupsList.Where(g => g.Name == MainWindow.CurrentPageName).FirstOrDefault();
            if (currentGroup.Tasks != null)
            {
                currentGroup.Tasks.Remove(Task);
                MainWindow.RefreshTasksStackPanel(currentGroup.Tasks);
            }
        }

        private void EditTask()
        {
            if (!string.IsNullOrEmpty(taskDeadLineText.Text))
                Task.DeadLine = DateTime.Parse(taskDeadLineText.Text);
            else
                Task.DeadLine = DateTime.Now;
            
            var currentGroup = MainWindow.GroupsList.Where(g => g.Name == MainWindow.CurrentPageName).FirstOrDefault();

            EditTaskWindow editTaskWindow = new EditTaskWindow(Task, currentGroup);
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
            var currentGroup = MainWindow.GroupsList.Where(g => g.Name == MainWindow.CurrentPageName).FirstOrDefault();

            if (currentGroup != null)
            {
                var copyTask = new Models.Task
                {
                    Id = currentGroup.Tasks != null ? currentGroup.Tasks.Count + 1 : 1,
                    Name = Task.Name,
                    Description = Task.Description,
                    DeadLine = Task.DeadLine,
                    IsCompleted = Task.IsCompleted,
                    IsOverdue = Task.IsOverdue,
                    IsPriority = Task.IsPriority,
                    Files = Task.Files
                };
                currentGroup.Tasks.Add(copyTask);
                MainWindow.RefreshTasksStackPanel(currentGroup.Tasks);
            }
        }

        private void priorityTaskImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Task.IsPriority)
                Task.IsPriority = false;
            else
                Task.IsPriority = true;
            
            var currentGroup = MainWindow.GroupsList.Where(g => g.Name == MainWindow.CurrentPageName).FirstOrDefault();
            MainWindow.RefreshTasksStackPanel(currentGroup.Tasks);
        }

        private void restoreTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Task.IsCompleted = false;

            MainWindow.RefreshCompletedTasksStackPanel();
        }

        private void menu_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            LoadGroups();
        }

        private void LoadGroups()
        {
            groupTaskMenuItem.Items.Clear();
            if (MainWindow.GroupsList.Any())
            {
                foreach (var group in MainWindow.GroupsList)
                {
                    var menuItem = new MenuItem
                    {
                        Header = group.Name,
                        Name = group.MenuItemName
                    };
                    groupTaskMenuItem.Items.Add(menuItem);
                }
            }
        }

        private void groupTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = e.Source as MenuItem;

            var swapGroup = MainWindow.GroupsList.Where(g => g.MenuItemName == menuItem.Name).FirstOrDefault();

            if (swapGroup != null)
                SetTaskToGroup(Task, swapGroup);
        }

        private void SetTaskToGroup(Models.Task task, Models.Group swapGroup)
        {
            var currentGroup = MainWindow.GroupsList.Where(g => g.Name == MainWindow.CurrentPageName).FirstOrDefault();
            currentGroup.Tasks.Remove(Task);

            if (swapGroup.Tasks != null)
                swapGroup.Tasks.Add(task);
            else
                swapGroup.Tasks = new List<Models.Task> { task };

            MainWindow.RefreshTasksStackPanel(currentGroup.Tasks);
        }
    }
}

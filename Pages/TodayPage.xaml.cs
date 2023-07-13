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
            SetTodayDate();
        }

        private void addTaskButton_Click(object sender, RoutedEventArgs e)
        {
            AddTaskWindow addTaskWindow = new AddTaskWindow();
            addTaskWindow.ShowDialog();

            LoadTasks();
        }

        private void SetTodayDate()
        {
            todayDateText.Text = DateTime.Now.ToShortDateString();
        }

        private void LoadTasks()
        {
            if (MainWindow.TasksList != null)
            {
                tasksStackPanel.Children.Clear();

                foreach (var task in MainWindow.TasksList)
                {
                    var taskControl = new TaskControl();
                    //taskControl.TaskName = task.Name;
                    //taskControl.TaskDescription = task.Description;
                    tasksStackPanel.Children.Add(taskControl);
                }
            }
        }
    }
}

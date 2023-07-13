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
using ToDo.Models;

namespace ToDo.Windows
{
    /// <summary>
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        public AddTaskWindow()
        {
            InitializeComponent();
        }

        private void addTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var task = new Models.Task
            {
                Name = taskNameText.Text,
                Description = taskDescriptionText.Text
            };

            MainWindow.TasksList.Add(task);
            this.Close();
        }

        private void closeWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

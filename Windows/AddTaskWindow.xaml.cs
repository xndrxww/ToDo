using Microsoft.Win32;
using System.IO;
using System.Windows;
using ToDo.Controls;

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
                Id = MainWindow.TasksList.Count + 1,
                Name = taskNameText.Text,
                Description = taskDescriptionText.Text,
                DeadLine = deadLineTime.SelectedDate
            };

            MainWindow.TasksList.Add(task);
            Close();
        }

        private void addFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Multiselect = true
            };

            if (fileDialog.ShowDialog() == true)
            {
                foreach (var file in fileDialog.FileNames)
                {
                    filesStackPanel.Children.Add(new FilesControl(Path.GetFileName(file)));
                }
            }
        }

        private void deadLineMenuItem_Click(object sender, RoutedEventArgs e)
        {
            deadLineTime.IsDropDownOpen = true;
        }

        private void deadLineTime_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            deadLineMenuItem.Header = deadLineTime.SelectedDate.Value.ToString("dd.MM.yyyy");
        }

        //public FileStream SaveFile(string filename)
        //{
        //    if (!Directory.Exists("files"))
        //        Directory.CreateDirectory("files");


        //    return File.Create($"{filename}");
        //}
    }
}

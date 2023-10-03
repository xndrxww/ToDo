using System;
using System.Windows;
using System.Windows.Controls;
using ToDo.Helpers;

namespace ToDo.Controls
{
    public partial class FilesControl : UserControl
    {
        private Models.File File;
        private Models.Task Task;

        public FilesControl(Models.File file, Models.Task task)
        {
            InitializeComponent();

            Initialization(file, task);
        }

        private void Initialization(Models.File file, Models.Task task)
        {
            File = file;
            Task = task;
            fileNameText.Text = File.Name;
        }

        private void deleteFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Task.Files.Remove(File);
            MainWindow.FilesStackPanel.Children.Clear();
            foreach (var file in Task.Files)
            {
                MainWindow.FilesStackPanel.Children.Add(new FilesControl(file, Task));
            }
        }

        private void openFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(File.UserPath);
            }
            catch (Exception)
            {
                ErrorWindowHelper.ShowError("При открытии файла произошла ошибка");
            }
        }
    }
}

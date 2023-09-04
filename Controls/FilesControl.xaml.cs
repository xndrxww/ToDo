using System;
using System.Windows;
using System.Windows.Controls;

namespace ToDo.Controls
{
    /// <summary>
    /// Interaction logic for FilesControl.xaml
    /// </summary>
    public partial class FilesControl : UserControl
    {
        private Models.File File;
        private Models.Task Task;

        public FilesControl(Models.File fileModel, Models.Task task = null)
        {
            InitializeComponent();

            File = fileModel;
            Task = task;
            fileNameText.Text = File.Name;
        }

        private void deleteFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.FilesList.Remove(File);
            MainWindow.FilesStackPanel.Children.Clear();
            foreach (var file in MainWindow.FilesList)
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
                MessageBox.Show("При открытии файла произошла ошибка");
            }
        }
    }
}

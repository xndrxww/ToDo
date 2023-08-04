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
        public FilesControl(string fileName)
        {
            InitializeComponent();

            fileNameText.Text = fileName;
        }

        private void deleteFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void openFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"D:\Данные\Материальная помощь\Заявление.jpeg");
            }
            catch (Exception)
            {
                MessageBox.Show("При открытии файла произошла ошибка");
            }
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using ToDo.Pages;

namespace ToDo
{
    public partial class MainWindow : Window
    {
        public static Frame MainFrameInstance;

        public MainWindow()
        {
            InitializeComponent();

            MainFrameInstance = MainFrame;
            MainFrameInstance.Navigate(new TodayPage());
        }
    }
}

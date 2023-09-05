using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ToDo.Controls
{
    /// <summary>
    /// Interaction logic for GroupControl.xaml
    /// </summary>
    public partial class GroupControl : UserControl
    {
        private Models.Group Group;

        public GroupControl(Models.Group group)
        {
            InitializeComponent();
            
            Group = group;
            groupNameTxt.Text = group.Name;
        }

        private void Grid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            groupMenu.IsSubmenuOpen = true;
        }

        private void deleteGroup_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GroupsList.Remove(Group);
            LoadGroups();
        }

        private void LoadGroups()
        {
            MainWindow.GroupStackPanel.Children.Clear();
            foreach (var group in MainWindow.GroupsList)
            {
                MainWindow.GroupStackPanel.Children.Add(new GroupControl(group));
            }
        }
    }
}

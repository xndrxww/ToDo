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
            MainWindow.LoadGroups();
        }

        private void renameGroup_Click(object sender, RoutedEventArgs e)
        {
            groupNameTxt.Visibility = Visibility.Collapsed;
            renameGroupNameTxt.Visibility = Visibility.Visible;
            renameGroupNameTxt.Text = groupNameTxt.Text;
            renameGroupNameTxt.Focus();
        }

        private void renameGroupNameTxt_LostFocus(object sender, RoutedEventArgs e)
        {
            var changedGroup = MainWindow.GroupsList.Where(group => group.Id == Group.Id).FirstOrDefault();
            int listIndex = MainWindow.GroupsList.IndexOf(changedGroup);

            changedGroup.Name = renameGroupNameTxt.Text;
            MainWindow.GroupsList[listIndex] = changedGroup;
            
            MainWindow.LoadGroups();

            groupNameTxt.Visibility = Visibility.Visible;
            renameGroupNameTxt.Visibility = Visibility.Collapsed;
        }
    }
}

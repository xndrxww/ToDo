using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToDo.Helpers;
using ToDo.Models;
using ToDo.Pages;

namespace ToDo.Controls
{
    public partial class GroupControl : UserControl
    {
        private Group Group;

        public GroupControl(Group group)
        {
            InitializeComponent();

            Initialization(group);
        }

        private void Initialization(Group group)
        {
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
            var changedGroup = GroupHelper.GetGroup(Group.Id);
            int listIndex = MainWindow.GroupsList.IndexOf(changedGroup);

            changedGroup.Name = renameGroupNameTxt.Text;
            MainWindow.GroupsList[listIndex] = changedGroup;

            MainWindow.LoadGroups();

            groupNameTxt.Visibility = Visibility.Visible;
            renameGroupNameTxt.Visibility = Visibility.Collapsed;

            MainWindow.MainFrameInstance.Navigate(new TodayPage(Group));
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.MainFrameInstance.Navigate(new TodayPage(Group));
        }
    }
}

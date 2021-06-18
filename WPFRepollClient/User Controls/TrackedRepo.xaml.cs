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

namespace WPFRepollClient.User_Controls
{
    /// <summary>
    /// Interaction logic for TrackedRepo.xaml
    /// </summary>
    public partial class TrackedRepo : UserControl
    {
        public TrackedRepo(string repoName, string repoDirectory)
        {
            InitializeComponent();
            RepoNameLabel.Content = repoName;
            RepoDirectoryLabel.Content = repoDirectory;
        }

        private void RepoRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var conditionUserControl = FindParent<TrackedRepo>(btn);
            if (conditionUserControl != null)
            {
                var sp = FindParent<StackPanel>(conditionUserControl);
                if (sp != null)
                    sp.Children.Remove(conditionUserControl);
            }
        }
        private static T FindParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null) return null;

            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }
    }
}

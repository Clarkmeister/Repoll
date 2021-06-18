using RepollInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
            try
            {
                if (conditionUserControl != null)
                {
                    MainWindow parentWindow = (MainWindow)Window.GetWindow(this);
                    Tuple<string, string> rem = null;
                    foreach (var tuple in MainWindow.trackedRepos)
                    {
                        if (tuple.Item1 == (string)RepoNameLabel.Content && tuple.Item2 == (string)RepoDirectoryLabel.Content)
                        {
                            if(!parentWindow.Remove_Repo(tuple))
                            {
                                throw new Exception("Failed to remove from server!");
                            }
                            rem = tuple;
                        }
                    }
                    if(rem != null)
                    {
                        MainWindow.trackedRepos.Remove(rem);
                        var sp = FindParent<StackPanel>(conditionUserControl);
                        if (sp != null)
                            sp.Children.Remove(conditionUserControl);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
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

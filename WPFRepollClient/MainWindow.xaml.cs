using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RepollInterfaces;
using WPFRepollClient.User_Controls;
using MessageBox = System.Windows.MessageBox;

namespace WPFRepollClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Tuple<string,string>> trackedRepos;
        bool isSetup = false;
        public MainWindow()
        {
            InitializeComponent();
            isSetup = false;
            ServiceController[] scServices;
            scServices = ServiceController.GetServices();
            foreach (ServiceController scTemp in scServices)
            {
                if (scTemp.ServiceName == "RepollService")
                {
                    ServiceController sc = new ServiceController("RepollService");
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        sc.Start();
                        while (sc.Status == ServiceControllerStatus.Stopped)
                        {
                            Thread.Sleep(1000);
                            sc.Refresh();
                        }
                    }
                }
            }
        }

        private void MainWindow_isLoaded(object sender, RoutedEventArgs e)
        {
            if (!isSetup)
            {
                try
                {
                    string uri = "net.tcp://localhost:6565/RepollService";
                    NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                    ChannelFactory<IWCFRepollService> channel = new ChannelFactory<IWCFRepollService>(binding);
                    EndpointAddress endpoint = new EndpointAddress(uri);
                    IWCFRepollService proxy = channel.CreateChannel(endpoint);
                    trackedRepos = proxy.GetTrackedRepos();
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show("Make sure Repoll Service has been installed. " + ex.Message, "Submission Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }

                foreach (var repo in trackedRepos)
                {
                    TrackedRepoStack.Children.Add(new TrackedRepo(repo.Item1, repo.Item2));
                }

                isSetup = true;
            }

        }

        private void Add_Directory_Button_Click(object sender, RoutedEventArgs e)
        {
            DirectoryPathTextBox.Text = "";
            using (var dialog = new FolderBrowserDialog()
            {
                Description = "Select a repo to track and update.",
                RootFolder = Environment.SpecialFolder.MyComputer,
                ShowNewFolderButton = false
            })
            {
                var result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    DirectoryPathTextBox.Text = dialog.SelectedPath;
                }
            }
        }

        private void NicknameTextBox_Text_Input(object sender, TextCompositionEventArgs e)
        {
            NicknameTextBox.Focus();
            NicknameTextBox.Select(NicknameTextBox.Text.Length, 0);
        }

        private void AddRepo_Button_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(NicknameTextBox.Text) && !string.IsNullOrWhiteSpace(DirectoryPathTextBox.Text))
                {

                    string uri = "net.tcp://localhost:6565/RepollService";
                    NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                    var channel = new ChannelFactory<IWCFRepollService>(binding);
                    var endpoint = new EndpointAddress(uri);
                    var proxy = channel.CreateChannel(endpoint);
                    var retval = proxy.SubmitTrackedRepo(NicknameTextBox.Text, DirectoryPathTextBox.Text);
                    if (retval)
                    {
                        TrackedRepoStack.Children.Add(new TrackedRepo(NicknameTextBox.Text, DirectoryPathTextBox.Text));
                    }
                    else
                    {
                        throw new Exception("Something went wrong with submitting a new repo to be tracked.");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter nickname and select directory.", "Try Again", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Make sure Repoll Service has been installed. " + ex.Message, "Submission Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private void ManualUpdate_Button_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string uri = "net.tcp://localhost:6565/RepollService";
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            var channel = new ChannelFactory<IWCFRepollService>(binding);
            var endpoint = new EndpointAddress(uri);
            var proxy = channel.CreateChannel(endpoint);
            //trackedTuples = proxy.GitPull();

            foreach (var product in trackedRepos)
            {
            }
        }
    }
}

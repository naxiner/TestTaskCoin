using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using TestTaskCoin.MVVM.Views;
using TestTaskCoin.Services;

namespace TestTaskCoin
{
    public partial class MainWindow : Window
    {
        private bool isMenuOpen = false;

        public MainWindow()
        {
            InitializeComponent();
            NavigationService.Initialize(MainFrame);
            MainFrame.Navigate(new MainPage());
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            var storyboard = (Storyboard)FindResource(isMenuOpen ? "MenuClose" : "MenuOpen");
            storyboard.Begin();
            isMenuOpen = !isMenuOpen;
        }

        private void DarkOverlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isMenuOpen)
            {
                var storyboard = (Storyboard)FindResource("MenuClose");
                storyboard.Begin();
                isMenuOpen = false;
            }
        }

        private void NavigateToMain_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MainPage());
            CloseMenu();
        }

        private void NavigateToTopList_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TopListPage());
            CloseMenu();
        }

        private void NavigateToSearch_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SearchPage());
            CloseMenu();
        }

        private void CloseMenu()
        {
            if (isMenuOpen)
            {
                var storyboard = (Storyboard)FindResource("MenuClose");
                storyboard.Begin();
                isMenuOpen = false;
            }
        }
    }
}

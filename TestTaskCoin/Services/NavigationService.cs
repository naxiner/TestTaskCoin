using System.Windows.Controls;

namespace TestTaskCoin.Services
{
    public class NavigationService
    {
        private static Frame _mainFrame;

        public static void Initialize(Frame frame)
        {
            _mainFrame = frame;
        }

        public static void NavigateToPage(Page page)
        {
            _mainFrame.Navigate(page);
        }
    }
}

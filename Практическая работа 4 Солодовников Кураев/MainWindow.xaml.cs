using System.ComponentModel;
using System.Windows;

namespace Практическая_работа_4_Солодовников_Кураев
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Page1());
        }

        private void ButtonPage1_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page1());
        }

        private void ButtonPage2_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page2());
        }

        private void ButtonPage3_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Page3());
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var result = MessageBox.Show(
                "Вы уверены, что хотите выйти из приложения?",
                "Подтверждение выхода",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            base.OnClosing(e);
        }
    }
}
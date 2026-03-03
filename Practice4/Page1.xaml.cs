using System;
using System.Windows;
using System.Windows.Controls;

namespace Практическая_работа_4_Солодовников_Кураев
{
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!double.TryParse(tbx.Text, out double x))
                    throw new FormatException("Поле X должно быть заполнено числом!");
                if (!double.TryParse(tby.Text, out double y))
                    throw new FormatException("Поле Y должно быть заполнено числом!");
                if (!double.TryParse(tbz.Text, out double z))
                    throw new FormatException("Поле Z должно быть заполнено числом!");
                if (y == 0)
                    throw new Exception("Y не может быть равен 0 (деление на ноль)");

                double yPowX = Math.Pow(y, x);
                double firstTerm = Math.Pow(2, yPowX);
                double secondTerm = Math.Pow(3, x) * y;
                double arctgZ = Math.Atan(z);
                if (arctgZ == 0)
                    throw new Exception("arctg(z) не может быть равен 0");

                double arctgPower = Math.Pow(arctgZ, -Math.PI / 6.0);
                double denominator = Math.Abs(x) + 1.0 / (y * y + 1);
                double fraction = y * arctgPower / denominator;
                double result = firstTerm + secondTerm - fraction;
                ResultTextBox.Text = result.ToString("F5");

            }
            catch (FormatException ex)
            {
                ResultTextBox.Text = "ERROR";
                MessageBox.Show($"{ex.Message}", "Ошибка формата",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (DivideByZeroException)
            {
                ResultTextBox.Text = "0";
                MessageBox.Show("Произошло деление на ноль", "Деление на ноль",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (OverflowException)
            {
                ResultTextBox.Text = "ERROR";
                MessageBox.Show(
                    "Слишком большие значения для вычисления!\n" +
                    "Пожалуйста, введите меньшие значения X, Y и Z",
                    "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "The winner is you!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            tbx.Clear();
            tby.Clear();
            tbz.Clear();
            ResultTextBox.Clear();
        }
    }
}
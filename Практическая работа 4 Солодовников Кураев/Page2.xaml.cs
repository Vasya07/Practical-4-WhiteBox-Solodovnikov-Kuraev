using System;
using System.Windows;
using System.Windows.Controls;

namespace Практическая_работа_4_Солодовников_Кураев
{
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        private double CalculateFX(double x)
        {
            if (RadioShX.IsChecked == true)
                return Math.Sinh(x);
            else if (RadioX2.IsChecked == true)
                return Math.Pow(x, 2);
            else if (RadioEx.IsChecked == true)
                return Math.Exp(x);
            else
                throw new Exception("Выберите функцию f(x)");
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (RadioShX.IsChecked != true && RadioX2.IsChecked != true && RadioEx.IsChecked != true)
                {
                    MessageBox.Show("Пожалуйста, выберите функцию f(x)!", "Ошибка выбора функции",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(tbx.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните поле X числом", "Ошибка заполнения полей",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    tbx.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(tby.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните поле Y числом", "Ошибка заполнения полей",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    tby.Focus();
                    return;
                }

                if (!double.TryParse(tbx.Text, out double x))
                    throw new FormatException("Поле X должно быть заполнено числом!");
                if (!double.TryParse(tby.Text, out double y))
                    throw new FormatException("Поле Y должно быть заполнено числом!");

                double fx = CalculateFX(x);
                double result;

                if (y == 0)
                {
                    result = 0;
                }
                else if (x == 0)
                {
                    result = Math.Pow(fx * fx + y, 3);
                }
                else if (x / y > 0)
                {
                    if (fx <= 0)
                        throw new Exception("Логарифм из неположительного числа (f(x) <= 0) при x/y > 0");
                    result = Math.Log(fx) + Math.Pow(fx * fx + y, 3);
                }
                else if (x / y < 0)
                {
                    if (fx / y <= 0)
                        throw new Exception("Логарифм из неположительного числа (|f(x)/y| <= 0)");
                    result = Math.Log(Math.Abs(fx / y)) + Math.Pow(fx + y, 3);
                }
                else
                {
                    result = double.NaN;
                }

                if (double.IsInfinity(result))
                {
                    ResultTextBox.Text = "∞";
                    MessageBox.Show("Результат слишком большой",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (double.IsNaN(result))
                {
                    ResultTextBox.Text = "NaN";
                    MessageBox.Show("Результат не определен (NaN)",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (Math.Abs(result) > 1e15)
                {
                    ResultTextBox.Text = result.ToString("E5");
                }
                else
                {
                    ResultTextBox.Text = result.ToString("F5");
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка формата",
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
            ResultTextBox.Clear();
            RadioShX.IsChecked = false;
            RadioX2.IsChecked = false;
            RadioEx.IsChecked = false;
        }
    }
}
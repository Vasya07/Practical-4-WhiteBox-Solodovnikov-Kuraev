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
                    MessageBox.Show("Выберите функцию f(x)!", "Ошибка выбора функции",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(tbx.Text) || string.IsNullOrWhiteSpace(tby.Text))
                {
                    MessageBox.Show("Поля X и Y должны быть заполнены!", "Ошибка заполнения полей",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(tbx.Text, out double x))
                    throw new FormatException("Поле X должно быть числом!");
                if (!double.TryParse(tby.Text, out double y))
                    throw new FormatException("Поле Y должно быть числом!");

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

                ResultTextBox.Text = result.ToString("F5");
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Ошибка формата: {ex.Message}", "Ошибка формата",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ОШИБКА: {ex.Message}", "Упс. Всё сломалось :(",
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
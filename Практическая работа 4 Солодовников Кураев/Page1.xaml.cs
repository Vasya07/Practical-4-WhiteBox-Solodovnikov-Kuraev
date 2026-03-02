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
                if (string.IsNullOrWhiteSpace(tbx.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните поле X числом", "Ошибка заполнения полей",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    tbx.Focus();
                    return;
                }
                else if (string.IsNullOrWhiteSpace(tby.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните поле Y числом", "Ошибка заполнения полей",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    tby.Focus();
                    return;
                }
                else if (string.IsNullOrWhiteSpace(tbz.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните поле Z числом", "Ошибка заполнения полей",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    tbz.Focus();
                    return;
                }

                if (!decimal.TryParse(tbx.Text, out decimal x))
                    throw new FormatException("Поле X должно быть заполнено числом!");
                if (!decimal.TryParse(tby.Text, out decimal y))
                    throw new FormatException("Поле Y должно быть заполнено числом!");
                if (!decimal.TryParse(tbz.Text, out decimal z))
                    throw new FormatException("Поле Z должно быть заполнено числом!");

                if (x + y <= 0)
                    throw new Exception("Область определения должна быть строго больше 0");

                double xDouble = (double)x;
                double yDouble = (double)y;
                double zDouble = (double)z;
                double firstPart = Math.Pow(yDouble, Math.Pow(Math.Abs(xDouble), 1.0 / 3.0));
                double sinZ = Math.Sin(zDouble);
                double sinSquared = sinZ * sinZ;
                double sqrtXY = Math.Sqrt(xDouble + yDouble);
                double numerator = Math.Abs(xDouble - yDouble) * (1 + (sinSquared / sqrtXY));
                double exponent = Math.Abs(xDouble - yDouble) + (zDouble / 2);
                double denominator = Math.Exp(exponent);
                double fraction = numerator / denominator;
                double cosY = Math.Cos(yDouble);
                double cosCubed = Math.Pow(cosY, 3);
                double secondPart = cosCubed * fraction;
                double resultDouble = firstPart + secondPart;
                decimal result = (decimal)resultDouble;
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
                ResultTextBox.Text = "ZERO";
                MessageBox.Show("Произошло деление на ноль", "Деление на ноль",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (OverflowException)
            {
                ResultTextBox.Text = "ERROR";
                MessageBox.Show(
                    "Результат слишком велик для отображения!\n" +
                    "Пожалуйста, введите меньшие значения X, Y и Z",
                    "Переполнение",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"КРИТИЧЕСКАЯ ОШИБКА:\n{ex.Message}", "The winner is you!",
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
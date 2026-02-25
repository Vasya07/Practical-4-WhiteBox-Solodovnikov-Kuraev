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
                // 1. Проверка на пустоту
                if (string.IsNullOrWhiteSpace(tbx.Text) ||
                    string.IsNullOrWhiteSpace(tby.Text) ||
                    string.IsNullOrWhiteSpace(tbz.Text))
                {
                    MessageBox.Show("Все поля должны быть заполнены!", "Ошибка ввода",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 2. Парсинг с проверкой
                if (!double.TryParse(tbx.Text, out double x))
                    throw new FormatException("Поле X должно быть числом.");
                if (!double.TryParse(tby.Text, out double y))
                    throw new FormatException("Поле Y должно быть числом.");
                if (!double.TryParse(tbz.Text, out double z))
                    throw new FormatException("Поле Z должно быть числом.");

                // 3. Проверка области определения
                if (x + y <= 0)
                    throw new Exception("Область определения: (x + y) должно быть строго больше 0 (под корнем в знаменателе).");

                // Проверка на возможное переполнение
                if (Math.Abs(x - y) > 700)
                    throw new Exception("|x-y| слишком большое значение для экспоненты.");

                // 4. Вычисление по частям
                // Первая часть: y^(|x|^(1/3))
                double firstPart = Math.Pow(y, Math.Pow(Math.Abs(x), 1.0 / 3.0));

                // Вторая часть (дробь)
                double sinZ = Math.Sin(z);
                double sinSquared = sinZ * sinZ;
                double sqrtXY = Math.Sqrt(x + y);
                double numerator = Math.Abs(x - y) * (1 + (sinSquared / sqrtXY));

                double exponent = Math.Abs(x - y) + (z / 2);
                double denominator = Math.Exp(exponent);

                double fraction = numerator / denominator;

                double cosY = Math.Cos(y);
                double cosCubed = Math.Pow(cosY, 3);
                double secondPart = cosCubed * fraction;

                // Итоговый результат
                double result = firstPart + secondPart;

                if (double.IsInfinity(result) || double.IsNaN(result))
                    throw new Exception("Результат вычисления вышел за пределы допустимых значений.");

                // 5. Вывод результата
                ResultTextBox.Text = result.ToString("F5");
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Ошибка формата: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Ошибка: деление на ноль.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка вычисления: {ex.Message}", "Ошибка",
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
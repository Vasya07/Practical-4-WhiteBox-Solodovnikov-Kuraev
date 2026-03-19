using System;
using System.Windows;
using System.Windows.Controls;

namespace Практическая_работа_4_Солодовников_Кураев
{
    public partial class Page2 : Page
    {
        public enum FunctionType
        {
            Sinh,
            Square,
            Exp
        }

        public Page2()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Вычисляет значение f(x) по выбранному типу функции
        /// </summary>
        public double CalculateFX(double x, FunctionType funcType)
        {
            switch (funcType)
            {
                case FunctionType.Sinh:
                    return Math.Sinh(x);
                case FunctionType.Square:
                    return Math.Pow(x, 2);
                case FunctionType.Exp:
                    return Math.Exp(x);
                default:
                    throw new ArgumentException("Неизвестный тип функции");
            }
        }

        /// <summary>
        /// Вычисляет значение условной функции
        /// </summary>
        public double CalculateFunction(double x, double y, FunctionType funcType)
        {
            double fx = CalculateFX(x, funcType);
            double result;

            if (y == 0)
            {
                result = 0;
            }
            else if (Math.Abs(x) < 1e-10)
            {
                result = Math.Pow(fx * fx + y, 3);
            }
            else if (x / y > 0)
            {
                if (fx <= 0)
                    throw new ArgumentException("ln(f(x)): f(x) должно быть > 0");
                result = Math.Log(fx) + Math.Pow(fx * fx + y, 3);
            }
            else if (x / y < 0)
            {
                if (fx == 0)
                    throw new ArgumentException("ln|f(x)/y|: f(x) не может быть равен 0");
                result = Math.Log(Math.Abs(fx / y)) + Math.Pow(fx + y, 3);
            }
            else
            {
                result = double.NaN;
            }

            return result;
        }

        private FunctionType GetSelectedFunctionType()
        {
            if (RadioShX.IsChecked == true)
                return FunctionType.Sinh;
            else if (RadioX2.IsChecked == true)
                return FunctionType.Square;
            else if (RadioEx.IsChecked == true)
                return FunctionType.Exp;
            else
                throw new InvalidOperationException("Выберите функцию f(x)");
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

                if (!double.TryParse(tbx.Text, out double x))
                    throw new FormatException("Поле X должно быть заполнено числом!");
                if (!double.TryParse(tby.Text, out double y))
                    throw new FormatException("Поле Y должно быть заполнено числом!");

                FunctionType funcType = GetSelectedFunctionType();
                double result = CalculateFunction(x, y, funcType);

                if (double.IsInfinity(result))
                {
                    ResultTextBox.Text = "ERROR";
                    MessageBox.Show("Слишком большие значения для вычисления!",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (double.IsNaN(result))
                {
                    ResultTextBox.Text = "NaN";
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
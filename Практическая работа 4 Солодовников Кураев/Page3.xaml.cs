using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace Практическая_работа_4_Солодовников_Кураев
{
    public partial class Page3 : Page
    {
        public PlotModel MyModel { get; set; }

        public Page3()
        {
            InitializeComponent();
            MyModel = new PlotModel { };
            MyModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X" });
            MyModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y" });

            DataContext = this;
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbx0.Text) ||
                    string.IsNullOrWhiteSpace(tbxk.Text) ||
                    string.IsNullOrWhiteSpace(tbdx.Text) ||
                    string.IsNullOrWhiteSpace(tbb.Text))
                {
                    MessageBox.Show("Все поля должны быть заполнены!", "Ошибка заполнения полей",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(tbx0.Text, out double x0))
                    throw new FormatException("Поле X0 должно быть числом!");
                if (!double.TryParse(tbxk.Text, out double xk))
                    throw new FormatException("Поле Xk должно быть числом!");
                if (!double.TryParse(tbdx.Text, out double dx))
                    throw new FormatException("Поле dx должно быть числом!");
                if (!double.TryParse(tbb.Text, out double b))
                    throw new FormatException("Поле b должно быть числом!");

                if (dx <= 0)
                    throw new Exception("Шаг должен быть положительным!");
                if (x0 > xk)
                    throw new Exception("Начало отрезка не может быть больше конца!");

                StringBuilder output = new StringBuilder();
                output.AppendLine("Результаты табулирования функции:");
                MyModel.Series.Clear();
                var lineSeries = new LineSeries
                {
                    Title = $"y = 9(x³ + {b:F2}³)·tg(x)",
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 2,
                    Color = OxyColors.Blue
                };

                int count = 0;
                double minY = double.MaxValue;
                double maxY = double.MinValue;

                for (double x = x0; x <= xk + dx / 2; x += dx)
                {
                    count++;
                    if (Math.Abs(Math.Cos(x)) < 1e-10)
                    {
                        output.AppendLine($"x = {x:F4}: \tЗначение не определено (cos(x)=0)");
                        continue;
                    }

                    double y = 9 * (Math.Pow(x, 3) + Math.Pow(b, 3)) * Math.Tan(x);

                    if (double.IsInfinity(y) || double.IsNaN(y))
                    {
                        output.AppendLine($"x = {x:F4}: \tЗначение выходит за пределы допустимых");
                    }
                    else
                    {
                        output.AppendLine($"x = {x:F4}: \ty = {y:F6}");
                        lineSeries.Points.Add(new DataPoint(x, y));

                        if (y < minY) minY = y;
                        if (y > maxY) maxY = y;
                    }

                    if (count > 1000)
                    {
                        output.AppendLine("Слишком много точек!");
                        break;
                    }
                }
                ResultTextBox.Text = output.ToString();
                if (lineSeries.Points.Count > 0)
                {
                    MyModel.Series.Add(lineSeries);
                    MyModel.Axes[0].Minimum = x0;
                    MyModel.Axes[0].Maximum = xk;

                    if (!double.IsInfinity(minY) && !double.IsInfinity(maxY))
                    {
                        MyModel.Axes[1].Minimum = minY - (maxY - minY) * 0.1;
                        MyModel.Axes[1].Maximum = maxY + (maxY - minY) * 0.1;
                    }
                }
                else
                {
                    MessageBox.Show("Отсутствуют точки для построения графика!", "Точек для графика нет",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                MyModel.InvalidatePlot(true);

            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Ошибка формата: {ex.Message}\nИспользуйте запятую для дробной части!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ОШИБКА: {ex.Message}", "Упс. Всё сломалось :(",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            tbx0.Clear();
            tbxk.Clear();
            tbdx.Clear();
            tbb.Clear();
            ResultTextBox.Clear();
            MyModel.Series.Clear();
            MyModel.InvalidatePlot(true);
        }
    }
}
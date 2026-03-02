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
            MyModel = new PlotModel();
            MyModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "X"
            });
            MyModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Y"
            });

            DataContext = this;
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbx0.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните поле X0 (начало)", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    tbx0.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(tbxk.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните поле Xk (конец)", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    tbxk.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(tbdx.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните поле dx (шаг)", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    tbdx.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(tbb.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните поле b (параметр)", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    tbb.Focus();
                    return;
                }

                if (!double.TryParse(tbx0.Text, out double x0))
                    throw new FormatException("Поле X0 должно быть заполнено числом!");
                if (!double.TryParse(tbxk.Text, out double xk))
                    throw new FormatException("Поле Xk должно быть заполнено числом!");
                if (!double.TryParse(tbdx.Text, out double dx))
                    throw new FormatException("Поле dx должно быть заполнено числом!");
                if (!double.TryParse(tbb.Text, out double b))
                    throw new FormatException("Поле b должно быть заполнено числом!");

                if (dx <= 0)
                    throw new Exception("Шаг должен быть положительным!");
                if (x0 > xk)
                    throw new Exception("Начало отрезка не может быть больше конца!");

                StringBuilder output = new StringBuilder();

                MyModel.Series.Clear();
                var lineSeries = new LineSeries
                {
                    Title = $"y = 9(x³ + {b:F2}³)·tg(x)",
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 3,
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
                        output.AppendLine($"x = {x:F3}: \tНе определено (cos=0)");
                        continue;
                    }

                    double y = 9 * (Math.Pow(x, 3) + Math.Pow(b, 3)) * Math.Tan(x);

                    if (double.IsInfinity(y) || double.IsNaN(y))
                    {
                        output.AppendLine($"x = {x:F3}: \tВыход за пределы");
                    }
                    else
                    {
                        output.AppendLine($"x = {x:F3}: \ty = {y:F5}");
                        lineSeries.Points.Add(new DataPoint(x, y));

                        if (y < minY) minY = y;
                        if (y > maxY) maxY = y;
                    }
                }

                ResultTextBox.Text = output.ToString();

                if (lineSeries.Points.Count > 0)
                {
                    MyModel.Series.Add(lineSeries);
                    MyModel.Axes[0].Minimum = x0;
                    MyModel.Axes[0].Maximum = xk;

                    if (!double.IsInfinity(minY) && !double.IsInfinity(maxY) && minY != maxY)
                    {
                        double padding = (maxY - minY) * 0.1;
                        MyModel.Axes[1].Minimum = minY - padding;
                        MyModel.Axes[1].Maximum = maxY + padding;
                    }
                }
                else
                {
                    MessageBox.Show("Нет точек для построения графика!", "Предупреждение",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                MyModel.InvalidatePlot(true);
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"{ex.Message}\nИспользуйте запятую для дробной части!",
                    "Ошибка формата", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "The winner is you!",
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
            MyModel.Axes[0].Minimum = double.NaN;
            MyModel.Axes[0].Maximum = double.NaN;
            MyModel.Axes[1].Minimum = double.NaN;
            MyModel.Axes[1].Maximum = double.NaN;
            MyModel.InvalidatePlot(true);
        }
    }
}
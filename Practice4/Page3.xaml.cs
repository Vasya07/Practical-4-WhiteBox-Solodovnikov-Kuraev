using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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

        /// <summary>
        /// Вычисляет значение функции для табуляции
        /// </summary>
        public double CalculateFunction(double x, double b)
        {
            if (Math.Abs(Math.Cos(x)) < 1e-10)
                throw new DivideByZeroException("cos(x) = 0, функция не определена");

            return 9 * (Math.Pow(x, 3) + Math.Pow(b, 3)) * Math.Tan(x);
        }

        /// <summary>
        /// Выполняет табуляцию функции на отрезке [x0, xk] с шагом dx
        /// </summary>
        public TabulationResult TabulateFunction(double x0, double xk, double dx, double b)
        {
            if (dx <= 0)
                throw new ArgumentException("Шаг должен быть положительным!");
            if (x0 > xk)
                throw new ArgumentException("Начало отрезка не может быть больше конца!");

            var result = new TabulationResult();
            StringBuilder output = new StringBuilder();

            for (double x = x0; x <= xk + dx / 2; x += dx)
            {
                try
                {
                    double y = CalculateFunction(x, b);
                    result.Points.Add(new TabulationPoint(x, y));
                    output.AppendLine($"x = {x:F3}: \ty = {y:F5}");
                }
                catch (DivideByZeroException)
                {
                    output.AppendLine($"x = {x:F3}: \tНе определено (cos=0)");
                }
                catch (OverflowException)
                {
                    output.AppendLine($"x = {x:F3}: \tВыход за пределы");
                }
            }

            result.TextOutput = output.ToString();
            return result;
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!double.TryParse(tbx0.Text, out double x0))
                    throw new FormatException("Поле X0 должно быть заполнено числом!");
                if (!double.TryParse(tbxk.Text, out double xk))
                    throw new FormatException("Поле Xk должно быть заполнено числом!");
                if (!double.TryParse(tbdx.Text, out double dx))
                    throw new FormatException("Поле dx должно быть заполнено числом!");
                if (!double.TryParse(tbb.Text, out double b))
                    throw new FormatException("Поле b должно быть заполнено числом!");

                var tabResult = TabulateFunction(x0, xk, dx, b);
                ResultTextBox.Text = tabResult.TextOutput;

                MyModel.Series.Clear();
                var lineSeries = new LineSeries
                {
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 3,
                    Color = OxyColors.Blue
                };

                foreach (var point in tabResult.Points)
                {
                    lineSeries.Points.Add(new DataPoint(point.X, point.Y));
                }

                if (lineSeries.Points.Count > 0)
                {
                    MyModel.Series.Add(lineSeries);
                    MyModel.Axes[0].Minimum = x0;
                    MyModel.Axes[0].Maximum = xk;
                    MyModel.InvalidatePlot(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

    public class TabulationPoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        public TabulationPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public class TabulationResult
    {
        public List<TabulationPoint> Points { get; set; } = new List<TabulationPoint>();
        public string TextOutput { get; set; } = "";
    }
}
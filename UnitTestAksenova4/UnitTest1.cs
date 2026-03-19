using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Практическая_работа_4_Солодовников_Кураев;

namespace UnitTestAksenova4
{
    [TestClass]
    public class UnitTest1
    {
        private Page1 page1;
        private Page2 page2;
        private Page3 page3;

        [TestInitialize]
        public void Setup()
        {
            page1 = new Page1();
            page2 = new Page2();
            page3 = new Page3();
        }

        [TestMethod]
        public void TestMethod1()
        {
            int res = 2 * 2;
            Assert.AreEqual(res, 4);
            Assert.AreNotEqual(res, 5);
            Assert.IsFalse(res > 4);
            Assert.IsTrue(res < 5);
        }

        [TestMethod]
        public void Page1_Calculate_ValidInput_ReturnsCorrectResult()
        {
            double x = 2;
            double y = 3;
            double z = 1;
            double expected = 122.715;
            double actual = page1.CalculateFunction(x, y, z);
            Assert.AreEqual(expected, actual, 0.001, "Неверное значение функции");
        }

        [TestMethod]
        public void Page1_Calculate_ZeroY_ThrowsDivideByZero()
        {
            double x = 2;
            double y = 0;
            double z = 1;
            Assert.ThrowsException<DivideByZeroException>(() => 
                page1.CalculateFunction(x, y, z), "Должно быть исключение при y=0");
        }

        [TestMethod]
        public void Page1_Calculate_NegativeY_WorksCorrectly()
        {
            double x = 2;
            double y = -2;
            double z = 1;
            double actual = page1.CalculateFunction(x, y, z);
            Assert.IsFalse(double.IsNaN(actual), "Результат не должен быть NaN");
            Assert.IsFalse(double.IsInfinity(actual), "Результат не должен быть бесконечностью");
        }

        [TestMethod]
        public void Page2_CalculateFX_Sinh_ReturnsCorrectValue()
        {
            double x = 1;
            double expected = Math.Sinh(1);
            double actual = page2.CalculateFX(x, Page2.FunctionType.Sinh);
            Assert.AreEqual(expected, actual, 0.001, "Неверное значение sh(x)");
        }

        [TestMethod]
        public void Page2_CalculateFX_Square_ReturnsCorrectValue()
        {
            double x = 3;
            double expected = 9;
            double actual = page2.CalculateFX(x, Page2.FunctionType.Square);
            Assert.AreEqual(expected, actual, 0.001, "Неверное значение x^2");
        }

        [TestMethod]
        public void Page2_CalculateFX_Exp_ReturnsCorrectValue()
        {
            double x = 2;
            double expected = Math.Exp(2);
            double actual = page2.CalculateFX(x, Page2.FunctionType.Exp);
            Assert.AreEqual(expected, actual, 0.001, "Неверное значение e^x");
        }

        [TestMethod]
        public void Page2_CalculateFunction_XequalsZero_ReturnsCorrectResult()
        {
            double x = 0;
            double y = 2;
            var funcType = Page2.FunctionType.Square;
            double expected = Math.Pow(0 * 0 + 2, 3);
            double actual = page2.CalculateFunction(x, y, funcType);
            Assert.AreEqual(expected, actual, 0.001, "Неверное значение при x=0");
        }

        [TestMethod]
        public void Page2_CalculateFunction_YequalsZero_ReturnsZero()
        {
            double x = 2;
            double y = 0;
            var funcType = Page2.FunctionType.Square;
            double actual = page2.CalculateFunction(x, y, funcType);
            Assert.AreEqual(0, actual, 0.001, "При y=0 результат должен быть 0");
        }

        [TestMethod]
        public void Page3_CalculateFunction_ValidInput_ReturnsCorrectResult()
        {
            double x = 1;
            double b = 2;
            double expected = 9 * (1 + 8) * Math.Tan(1);
            double actual = page3.CalculateFunction(x, b);
            Assert.AreEqual(expected, actual, 0.001, "Неверное значение функции");
        }

        [TestMethod]
        public void Page3_CalculateFunction_CosEqualsZero_ThrowsDivideByZero()
        {
            double x = Math.PI / 2;
            double b = 1;
            Assert.ThrowsException<DivideByZeroException>(() => 
                page3.CalculateFunction(x, b), "Должно быть исключение при cos(x)=0");
        }

        [TestMethod]
        public void Page3_TabulateFunction_ValidInput_ReturnsPoints()
        {
            double x0 = 0;
            double xk = 2;
            double dx = 0.5;
            double b = 1;
            var result = page3.TabulateFunction(x0, xk, dx, b);
            Assert.IsTrue(result.Points.Count > 0, "Должны быть точки");
            Assert.IsFalse(string.IsNullOrEmpty(result.TextOutput), "Должен быть текстовый вывод");
        }

        [TestMethod]
        public void Page3_TabulateFunction_InvalidStep_ThrowsException()
        {
            double x0 = 0;
            double xk = 2;
            double dx = -0.5;
            double b = 1;
            Assert.ThrowsException<ArgumentException>(() => 
                page3.TabulateFunction(x0, xk, dx, b), "Отрицательный шаг недопустим");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCore.TestDemo
{
    public class CalculatorOp
    {
        #region Demo 2
        public int AddInts(int a, int b)
        {
            return a + b;
        }
        public double AddDoubles(double a, double b)
        {
            return a + b;
        }
        public int Divide(int value, int by)
        {
            if (value > 100)
            {
                throw new ArgumentOutOfRangeException("value"); // bug for demo purposes
            }
            return value / by;
        }
        #endregion

        #region Demo 1
        public int Add(int a, int b)
        {
            return a + b;
        }
        public int Multiply(int num1, int num2)
        {
            int result = num1 * num2;
            return result;
        }
        #endregion
    }
}
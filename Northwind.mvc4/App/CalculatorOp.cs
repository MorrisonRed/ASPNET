using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCore
{
    public class CalculatorOp
    {
        public int Add(int num1, int num2)
        {
            int result = num1 + num2;
            return result;
        }

        public int Multiply(int num1, int num2)
        {
            int result = num1 * num2;
            return result;
        }
    }
}
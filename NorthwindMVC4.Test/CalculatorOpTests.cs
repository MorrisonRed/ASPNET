using System;
using NUnit.Framework;
using AppCore.TestDemo;

namespace Calculator.Test
{
    [TestFixture]
    public class CalculatorOpTests
    {
        #region Demo 1
        [Test]
        public void NUnit_ShouldAddTwoNumbers()
        {
            var sut = new CalculatorOp();
            var result = sut.Add(1, 2);
            Assert.That(result, Is.EqualTo(3)); 
        }
        [Test]
        public void NUnit_ShouldMultiplyTwoNumber()
        {
            var sut = new CalculatorOp();
            var result = sut.Multiply(2, 10);
            Assert.That(result, Is.EqualTo(20));
        }
        #endregion

        #region Demo 2
        [Test]
        public void NUnit_ShouldAddInts()
        {
            var sut = new CalculatorOp();
            var result = sut.AddInts(1, 2);
            Assert.That(result, Is.EqualTo(3));
        }
        [Test]
        public void NUnit_ShouldAddDoubles()
        {
            var sut = new CalculatorOp();
            var result = sut.AddDoubles(1.1, 2.2);
            Assert.That(result, Is.EqualTo(3.3));
        }
        [Test]
        public void NUnit_ShouldAddDoubles_WithTolerance()
        {
            var sut = new CalculatorOp();
            var result = sut.AddDoubles(1.1, 2.2);
            Assert.That(result, Is.EqualTo(3.3).Within(0.01));
        }
        [Test]
        public void NUnit_ShouldAddDoubles_WithPercentTolerance()
        {
            var sut = new CalculatorOp();
            var result = sut.AddDoubles(50, 50);
            Assert.That(result, Is.EqualTo(101).Within(1).Percent);
        }
        [Test]
        public void NUnit_ShouldAddDoubles_WithBadTolerance()
        {
            var sut = new CalculatorOp();
            var result = sut.AddDoubles(1.1, 2.2);
            Assert.That(result, Is.EqualTo(30).Within(100));
        }
        #endregion

        #region Demo 5
        [Test]
        public void NUnit_ShouldErrorWhenDividedByZero()
        {
            var sut = new CalculatorOp();
            Assert.That(() => sut.Divide(200, 0), Throws.Exception);
        }
        [Test]
        public void NUnit_ShouldErrorWhenDividedByZero_ExplicitExceptionType()
        {
            var sut = new CalculatorOp();
            Assert.That(() => sut.Divide(99, 0), 
                Throws.TypeOf<DivideByZeroException>());
        }
        [Test]
        public void NUnit_ShouldErrorWhenNumberTooBig()
        {
            var sut = new CalculatorOp();
            Assert.That(() => sut.Divide(200, 2),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }
        public void NUnit_ShouldErrorWhenNumberTooBig_MoreExplicit()
        {
            var sut = new CalculatorOp();
            Assert.That(() => sut.Divide(200, 2),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                .With.Matches<ArgumentOutOfRangeException>(x => x.ParamName == "value"));
        }
        #endregion

    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace NorthwindMVC4.MSTest
{
    [TestClass]
    public class IntroToMSTest
    {
        [TestMethod]
        public void ShouldAddReturnNineWhenPassFiveAndFour()
        {
            //Arrange
            AppCore.CalculatorOp sut = new AppCore.CalculatorOp();

            //Act
            int result = sut.Add(5, 4);

            //Assertion
            Assert.AreEqual(9, result);
        }

        [TestMethod]
        public void ShouldMultiplyReturnTwentyWhenPassFiveAndFour()
        {
            //Arrange
            AppCore.CalculatorOp sut = new AppCore.CalculatorOp();

            //Act
            int result = sut.Multiply(5, 4);

            //Assertion
            Assert.AreEqual(20, result);
        }
    }
}

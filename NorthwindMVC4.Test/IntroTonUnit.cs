using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NorthwindMVC4.Test
{
    [TestFixture(Description = "This is a Comment added", Category = "Introduction")]
    public class IntroTonUnit
    {
        AppCore.CalculatorOp sut;


        [Test]
        public void PostiveTest()
        {
            int x = 7;
            int y = 7;
            Assert.AreEqual(x, y);
        }

        [Test]
        public void NegativeTest()
        {
            if (true)
                Assert.Fail("Oh no this failed!");
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void ExpectedExceptionTest()
        {
            throw new NotSupportedException();
        }

        [Test, Ignore]
        public void NotImplementedException()
        {
            throw new NotImplementedException();
        }

        [TestFixtureSetUp]
        public void TestSetup() {
            sut = new AppCore.CalculatorOp();
        }

        [TestFixtureTearDown]
        public void TestEnd()
        {
            sut = null;
        }


        [Test]
        public void ShouldAddReturnNineWhenPassFiveAndFour()
        {
            //Act
            int result = sut.Add(5, 4);

            //Assertion
            Assert.AreEqual(9, result);
        }

        [Test]
        public void ShouldMultiplyReturnTwentyWhenPassFiveAndFour()
        {
            //Act
            int result = sut.Multiply(5, 4);

            //Assertion
            Assert.AreEqual(20, result);
        }
    }
}

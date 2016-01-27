using System;
using NUnit.Framework;
using AppCore.TestDemo;

namespace NorthwindMVC4.Test
{
    [TestFixture]
    public class SpecialDatesStoreTests
    {
        [Test]
        public void NUnit_ShouldHaveCorrectMilleniumDate()
        {
            var sut = new SpecialDateStore();
            var result = sut.DateOf(SpecialDates.NewMilennium);
            Assert.That(result, Is.EqualTo(new DateTime(2000, 1, 1, 0, 0, 0)));
        }

        [Test]
        public void NUnit_ShouldHaveCorrectMilliniumDate_WithTolerance()
        {
            var sut = new SpecialDateStore();
            var result = sut.DateOf(SpecialDates.NewMilennium);
            //Assert.That(result, Is.EqualTo(new DateTime(2000, 1, 1, 0, 0, 0, 1)));

            Assert.That(result,
                Is.EqualTo(new DateTime(2000, 1, 1, 0, 0, 0, 1))
                .Within(TimeSpan.FromMilliseconds(1)));

            //Assert.That(result,
            //    Is.EqualTo(new DateTime(2000, 1, 1, 0, 0, 0, 1))
            //    .Within(1).Milliseconds);
        }
    }
}

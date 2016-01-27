using NUnit.Framework;
using AppCore.TestDemo;

namespace NorthwindMVC4.Test
{
    [TestFixture]
    class NameJoinerTests
    {

        [Test]
        public void NUnit_ShouldJoinNames()
        {
            var sut = new NameJoiner();
            var fullName = sut.Join("Sarah", "Smith");
            Assert.That(fullName, Is.EqualTo("Sarah Smith"));
        }

        [Test]
        public void NUnit_ShouldJoinNames_CaseInsensitive()
        {
            var sut = new NameJoiner();
            var fullName = sut.Join("sarah", "smith");
            Assert.That(fullName, Is.EqualTo("SARAH SMITH").IgnoreCase);
        }

        [Test]
        public void NUnit_ShouldJoinNames_NotEqual()
        {
            var sut = new NameJoiner();
            var fullName = sut.Join("Sarah", "Smith");
            Assert.That(fullName, Is.Not.EqualTo("Gentry Smith"));
        }
    }
}

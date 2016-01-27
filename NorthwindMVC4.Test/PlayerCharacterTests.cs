using NUnit.Framework;
using AppCore.TestDemo;

namespace NorthwindMVC4.Test
{
    [TestFixture]
    public class PlayerCharacterTests
    {
        [Test]
        public void NUnit_ShouldIncreaseHealthAfterSleeping()
        {
            var sut = new PlayerCharacter() { Health = 100 };
            sut.Sleep();
            Assert.That(sut.Health, Is.GreaterThan(100));
        }

        [Test]
        public void NUnit_ShouldIncreaseHealthInExpectedRangeAfterSleeping()
        {
            var sut = new PlayerCharacter() { Health = 100 };
            sut.Sleep();
            Assert.That(sut.Health, Is.InRange(101, 200));
        }

        [Test]
        public void NUnit_ShouldHaveDefaultRandomGeneratedName()
        {
            var sut = new PlayerCharacter();
            Assert.That(sut.Name, Is.Not.Empty);
        }

        [Test]
        public void NUnit_ShouldNotHaveANickName()
        {
            var sut = new PlayerCharacter();
            Assert.That(sut.NickName, Is.Null);
        }

        [Test]
        public void NUnit_ShouldBeNewbie()
        {
            var sut = new PlayerCharacter();
            Assert.That(sut.IsNoob, Is.True);
        }

        [Test]
        public void NUnit_ShouldHaveNoEmptyDefaultWeapons()
        {
            var sut = new PlayerCharacter();
            Assert.That(sut.Weapons, Is.All.Not.Empty);
        }

        [Test]
        public void NUnit_ShouldHaveALongBow()
        {
            var sut = new PlayerCharacter();
            Assert.That(sut.Weapons, Contains.Item("Long Bow"));
        }

        [Test]
        public void NUnit_ShouldHaveAtLeastOneKindOfSword()
        {
            var sut = new PlayerCharacter();
            Assert.That(sut.Weapons, Has.Some.ContainsSubstring("Sword"));
        }

        [Test]
        public void NUnit_ShouldHaveTwoBows()
        {
            var sut = new PlayerCharacter();
            Assert.That(sut.Weapons, Has.Exactly(2).EndsWith("Bow"));
        }

        [Test]
        public void NUnit_ShouldNotHaveMoreThanOneTypeOfAGivenWeapon()
        {
            var sut = new PlayerCharacter();
            Assert.That(sut.Weapons, Is.Unique);
        }

        [Test]
        public void NUnit_ShouldNotHaveStaffOfWonder()
        {
            var sut = new PlayerCharacter();
            Assert.That(sut.Weapons, Has.None.EqualTo("Staff Of Wonder"));
        }

        [Test]
        public void NUnit_ShouldHaveAllExpectedWeapons()
        {
            var sut = new PlayerCharacter();
            var expectedWeapons = new[]
            {
                "Short Sword",
                "Short Bow",
                "Long Bow",
            };
            Assert.That(sut.Weapons, Is.EquivalentTo(expectedWeapons));
        }

        [Test]
        public void NUnit_ShouldHaveDefaultWeaponsInAlphabeticalOrder()
        {
            var sut = new PlayerCharacter();
            Assert.That(sut.Weapons, Is.Ordered);
        }

        [Test]
        public void NUnit_ReferenceEqualityDemo()
        {
            var player1 = new PlayerCharacter();
            var player2 = new PlayerCharacter();
            //Assert.That(player1, Is.SameAs(player2));

            var somePlayer = player1;
            Assert.That(player1, Is.SameAs(somePlayer));

            //Assert.That(player1, Is.Not.SameAs(player2));
        }
    }
}

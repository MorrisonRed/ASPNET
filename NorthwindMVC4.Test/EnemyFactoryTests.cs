using NUnit.Framework;
using AppCore.TestDemo;

namespace NorthwindMVC4.Test
{
    [TestFixture]
    public class EnemyFactoryTests
    {
        [Test]
        public void NUnit_ShouldCreateNormalEnemy()
        {
            var sut = new EnemyFactory();
            object enemy = sut.Create(false);
            Assert.That(enemy, Is.TypeOf<NormalEnemy>());
        }

        [Test]
        public void NUnit_ShouldCreateBossEnemy()
        {
            var sut = new EnemyFactory();
            object enemy = sut.Create(true);
            Assert.That(enemy, Is.TypeOf<BossEnemy>());
        }

        [Test]
        public void NUnit_ShouldBeOfBaseType()
        {
            var sut = new EnemyFactory();
            object enemy = sut.Create(true);
            Assert.That(enemy, Is.InstanceOf<Enemy>());
        }

        [Test]
        public void NUnit_ShouldHaveExtraPower()
        {
            var sut = new EnemyFactory();
            object enemy = sut.Create(true);
            Assert.That(enemy, Has.Property("ExtraPower"));

            //Assert.That(enemy, Has.Property("SomeOtherProperty"));
        }
    }
}

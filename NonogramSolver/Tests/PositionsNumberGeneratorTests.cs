using System.Numerics;
using NUnit.Framework;
using NonogramSolver.Core;

namespace NonogramSolver.Tests
{
    [TestFixture]
    class PositionsNumberGeneratorTests
    {
        private PositionsNumberGenerator target;

        [SetUp]
        public void Init()
        {
            target = new PositionsNumberGenerator();
        }

        [Test]
        public void FirstPowerTest()
        {
            Assert.AreEqual((BigInteger)2, target.PositionNumber(1, 1, 2));
            Assert.AreEqual((BigInteger)1, target.PositionNumber(1, 1, 1));
            Assert.AreEqual((BigInteger)5, target.PositionNumber(1, 2, 6));
        }

        [Test]
        public void SecondPowerTest()
        {
            Assert.AreEqual((BigInteger)1, target.PositionNumber(2, 4, 5));
            Assert.AreEqual((BigInteger)6, target.PositionNumber(2, 7, 10));
        }

        [Test]
        public void ThirdPowerTest()
        {
            Assert.AreEqual((BigInteger)35, target.PositionNumber(3, 3, 9));
            Assert.AreEqual((BigInteger)56, target.PositionNumber(3, 3, 10));
        }

        [Test]
        public void FourthPowerTest()
        {
            Assert.AreEqual((BigInteger)15, target.PositionNumber(4, 5, 10));
            Assert.AreEqual((BigInteger)5, target.PositionNumber(4, 5, 9));
        }
    }
}

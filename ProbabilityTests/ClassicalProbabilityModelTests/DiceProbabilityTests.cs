using NUnit.Framework;
using ProbabilityConsolePrjct.ProbabilityModels;
using System;
using System.Linq;

namespace ProbabilityTests.ClassicalProbabilityModelTests
{
    [TestFixture]
    public class DiceProbabilityTests
    {
        [Test]
        public void DieProbability_SixSides_EqualProbability()
        {
            // Arrange
            var dieFaces = Enumerable.Range(1, 6);
            var dieModel = new ClassicalProbabilityModel<int>(dieFaces);

            // Act & Assert
            foreach (var face in dieFaces)
            {
                Assert.AreEqual(1.0 / 6, dieModel.GetProbability(face), 0.001);
            }
        }

        [Test]
        public void AddFace_ChangesProbability()
        {
            // Arrange
            var die = new ClassicalProbabilityModel<int>(new[] { 1, 2, 3, 4, 5, 6 });
            var initialProb = die.GetProbability(1);

            // Act
            die.AddOutcome(7);
            var newProb = die.GetProbability(1);

            // Assert
            Assert.AreEqual(1.0 / 6, initialProb, 0.001);
            Assert.AreEqual(1.0 / 7, newProb, 0.001);
        }

        [Test]
        public void RemoveFace_ChangesProbability()
        {
            // Arrange
            var die = new ClassicalProbabilityModel<int>(new[] { 1, 2, 3, 4, 5, 6 });

            // Act
            die.RemoveOutcome(3);
            var prob = die.GetProbability(1);

            // Assert
            Assert.AreEqual(0.2, prob, 0.001);
            Assert.AreEqual(0.0, die.GetProbability(3));
        }

        [Test]
        public void NonExistentFace_ProbabilityZero()
        {
            // Arrange
            var die = new ClassicalProbabilityModel<int>(new[] { 1, 2, 3, 4, 5, 6 });

            // Act & Assert
            Assert.AreEqual(0.0, die.GetProbability(7));
            Assert.AreEqual(0.0, die.GetProbability(0));
        }
    }
}
using NUnit.Framework;
using ProbabilityConsolePrjct.ProbabilityModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProbabilityTests.ClassicalProbabilityModelTests
{
    [TestFixture]
    public class ClassicalEventTests
    {
        // Класс для тестирования карт
        public class Card
        {
            public string Suit { get; }
            public string Rank { get; }

            public Card(string suit, string rank)
            {
                Suit = suit;
                Rank = rank;
            }

            public override bool Equals(object obj) =>
                obj is Card other && Suit == other.Suit && Rank == other.Rank;

            public override int GetHashCode() =>
                HashCode.Combine(Suit, Rank);
        }

        private List<Card> GenerateDeck()
        {
            var suits = new[] { "Hearts", "Diamonds", "Clubs", "Spades" };
            var ranks = new[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

            return (from suit in suits
                    from rank in ranks
                    select new Card(suit, rank)).ToList();
        }

        [Test]
        public void EventsCount_Die_Returns64()
        {
            // Arrange
            var die = new ClassicalProbabilityModel<int>(Enumerable.Range(1, 6));
            var eventA = new ClassicalEvent<int>(die, new[] { 1, 2 });

            // Act & Assert
            Assert.That(eventA.EventsCount(), Is.EqualTo(64));
        }

        [Test]
        public void Complement_EvenDieEvent_Correct()
        {
            // Arrange
            var die = new ClassicalProbabilityModel<int>(Enumerable.Range(1, 6));
            var evenEvent = new ClassicalEvent<int>(die, new[] { 2, 4, 6 });

            // Act
            var oddEvent = !evenEvent;

            // Assert
            Assert.That(oddEvent.Probability, Is.EqualTo(0.5));
            CollectionAssert.AreEquivalent(new[] { 1, 3, 5 }, oddEvent.FavorableOutcomes);
        }

        [Test]
        public void Union_DieEvents_CorrectProbability()
        {
            // Arrange
            var die = new ClassicalProbabilityModel<int>(Enumerable.Range(1, 6));
            var evenEvent = new ClassicalEvent<int>(die, new[] { 2, 4, 6 });
            var greaterThan3 = new ClassicalEvent<int>(die, new[] { 4, 5, 6 });

            // Act
            var unionEvent = evenEvent | greaterThan3;

            // Assert
            Assert.That(unionEvent.Probability, Is.EqualTo(4.0 / 6).Within(0.001));
            CollectionAssert.AreEquivalent(new[] { 2, 4, 5, 6 }, unionEvent.FavorableOutcomes);
        }

        [Test]
        public void Intersection_CardEvents_Correct()
        {
            // Arrange
            var deck = GenerateDeck();
            var deckModel = new ClassicalProbabilityModel<Card>(deck);

            var heartsEvent = new ClassicalEvent<Card>(
                deckModel,
                deck.Where(c => c.Suit == "Hearts")
            );

            var acesEvent = new ClassicalEvent<Card>(
                deckModel,
                deck.Where(c => c.Rank == "A")
            );

            // Act
            var heartAceEvent = heartsEvent & acesEvent;

            // Assert
            Assert.That(heartAceEvent.Probability, Is.EqualTo(1.0 / 52).Within(0.001));
            var expectedCard = new Card("Hearts", "A");
            Assert.That(heartAceEvent.FavorableOutcomes.Single().Equals(expectedCard));
        }

        [Test]
        public void ConditionalProbability_HeartGivenAce_Correct()
        {
            // Arrange
            var deck = GenerateDeck();
            var deckModel = new ClassicalProbabilityModel<Card>(deck);

            var heartsEvent = new ClassicalEvent<Card>(
                deckModel,
                deck.Where(c => c.Suit == "Hearts")
            );

            var acesEvent = new ClassicalEvent<Card>(
                deckModel,
                deck.Where(c => c.Rank == "A")
            );

            // Act
            double pHeartGivenAce = heartsEvent.ConditionalProbability(acesEvent);

            // Assert
            // P(Hearts|Ace) = 1/4 (4 масти у туза)
            Assert.That(pHeartGivenAce, Is.EqualTo(0.25).Within(0.001));
        }

        [Test]
        public void AreMutuallyExclusive_DieEvents_True()
        {
            // Arrange
            var die = new ClassicalProbabilityModel<int>(Enumerable.Range(1, 6));
            var lowEvent = new ClassicalEvent<int>(die, new[] { 1, 2 });
            var highEvent = new ClassicalEvent<int>(die, new[] { 5, 6 });

            // Act & Assert
            Assert.That(ClassicalEvent<int>.AreMutuallyExclusive(lowEvent, highEvent));
        }

        [Test]
        public void IsIndependent_DieEvents_Correct()
        {
            // Arrange
            var die = new ClassicalProbabilityModel<int>(Enumerable.Range(1, 6));
            var evenEvent = new ClassicalEvent<int>(die, new[] { 2, 4, 6 });     // P=0.5
            var multipleOf3 = new ClassicalEvent<int>(die, new[] { 3, 6 });       // P=1/3

            // Act
            bool independent = evenEvent.IsIndependent(multipleOf3);

            // Assert
            // P(even ∩ multipleOf3) = P({6}) = 1/6
            // P(even)*P(multipleOf3) = (1/2)*(1/3)=1/6 → независимы
            Assert.IsTrue(independent);
        }

        [Test]
        public void Difference_DieEvents_Correct()
        {
            // Arrange
            var die = new ClassicalProbabilityModel<int>(Enumerable.Range(1, 6));
            var eventA = new ClassicalEvent<int>(die, new[] { 1, 2, 3, 4 });
            var eventB = new ClassicalEvent<int>(die, new[] { 3, 4, 5, 6 });

            // Act
            var difference = eventA.Difference(eventB);

            // Assert
            CollectionAssert.AreEquivalent(new[] { 1, 2 }, difference.FavorableOutcomes);
            Assert.That(difference.Probability, Is.EqualTo(1.0 / 3).Within(0.001));
        }

        [Test]
        public void ConditionalProbability_ZeroDenominator_ReturnsZero()
        {
            // Arrange
            var die = new ClassicalProbabilityModel<int>(Enumerable.Range(1, 6));
            var eventA = new ClassicalEvent<int>(die, new[] { 1 });
            var impossibleEvent = new ClassicalEvent<int>(die, Enumerable.Empty<int>());

            // Act
            double result = eventA.ConditionalProbability(impossibleEvent);

            // Assert
            Assert.That(result, Is.EqualTo(0.0));
        }

        [Test]
        public void OperatorOverloads_DieEvents_Consistent()
        {
            // Arrange
            var die = new ClassicalProbabilityModel<int>(Enumerable.Range(1, 6));
            var eventA = new ClassicalEvent<int>(die, new[] { 1, 2, 3 });
            var eventB = new ClassicalEvent<int>(die, new[] { 3, 4, 5 });

            // Act
            var union = eventA | eventB;
            var intersection = eventA & eventB;
            var complement = !eventA;

            // Assert
            CollectionAssert.AreEquivalent(new[] { 1, 2, 3, 4, 5 }, union.FavorableOutcomes);
            CollectionAssert.AreEquivalent(new[] { 3 }, intersection.FavorableOutcomes);
            CollectionAssert.AreEquivalent(new[] { 4, 5, 6 }, complement.FavorableOutcomes);
        }

        [Test]
        public void DifferentModels_ThrowsException()
        {
            // Arrange
            var die6 = new ClassicalProbabilityModel<int>(Enumerable.Range(1, 6));
            var die8 = new ClassicalProbabilityModel<int>(Enumerable.Range(1, 8));

            var event6 = new ClassicalEvent<int>(die6, new[] { 1 });
            var event8 = new ClassicalEvent<int>(die8, new[] { 1 });

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                event6.Union(event8));

            Assert.Throws<InvalidOperationException>(() =>
                event6.IsIndependent(event8));
        }
    }
}
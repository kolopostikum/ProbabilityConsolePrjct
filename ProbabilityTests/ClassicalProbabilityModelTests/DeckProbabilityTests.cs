using NUnit.Framework;
using ProbabilityConsolePrjct;
using ProbabilityConsolePrjct.ProbabilityModels;
using System.Collections.Generic;
using System.Linq;

namespace ProbabilityTests
{
    [TestFixture]
    public class DeckProbabilityTests
    {
        // Класс карты для тестирования
        public class Card
        {
            public string Suit { get; }
            public string Rank { get; }

            public Card(string suit, string rank)
            {
                Suit = suit;
                Rank = rank;
            }

            // Для корректной работы HashSet
            public override bool Equals(object obj) => obj is Card other &&
                Suit == other.Suit && Rank == other.Rank;

            public override int GetHashCode() =>
                (Suit, Rank).GetHashCode();
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
        public void FullDeckProbability_AllCardsEqual()
        {
            // Arrange
            var deck = GenerateDeck();
            var deckModel = new ClassicalProbabilityModel<Card>(deck);

            // Act & Assert
            foreach (var card in deck)
            {
                Assert.AreEqual(1.0 / 52, deckModel.GetProbability(card), 0.001);
            }
        }

        [Test]
        public void RemoveCard_ChangesProbability()
        {
            // Arrange
            var deck = GenerateDeck();
            var deckModel = new ClassicalProbabilityModel<Card>(deck);
            var aceOfSpades = new Card("Spades", "A");

            // Act
            deckModel.RemoveOutcome(aceOfSpades);

            // Assert
            Assert.AreEqual(0.0, deckModel.GetProbability(aceOfSpades));
            Assert.AreEqual(1.0 / 51, deckModel.GetProbability(deck.First()), 0.001);
        }

        [Test]
        public void AddDuplicateCard_ProbabilityUnchanged()
        {
            // Arrange
            var deck = GenerateDeck();
            var deckModel = new ClassicalProbabilityModel<Card>(deck);
            var aceOfHearts = new Card("Hearts", "A");

            // Act
            var added = deckModel.AddOutcome(aceOfHearts);

            // Assert
            Assert.IsFalse(added);
            Assert.AreEqual(1.0 / 52, deckModel.GetProbability(aceOfHearts), 0.001);
        }

        [Test]
        public void SpecificCardProbability_CorrectValue()
        {
            // Arrange
            var deck = GenerateDeck();
            var deckModel = new ClassicalProbabilityModel<Card>(deck);
            var kingOfClubs = new Card("Clubs", "K");

            // Act & Assert
            Assert.AreEqual(1.0 / 52, deckModel.GetProbability(kingOfClubs), 0.001);
        }

        [Test]
        public void EmptyDeck_ProbabilityZero()
        {
            // Arrange
            var deckModel = new ClassicalProbabilityModel<Card>(new List<Card>());
            var anyCard = new Card("Hearts", "2");

            // Act & Assert
            Assert.AreEqual(0.0, deckModel.GetProbability(anyCard));
        }
    }
}
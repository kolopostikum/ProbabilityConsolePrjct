using ProbabilityConsolePrjct.ProbabilityModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProbabilityConsolePrjct.Tasks
{
    public class ConditionalProbabilityTask : ProbabilityTasks
    {
        public void RunAllTasks()
        {
            Task1();
            Task2();
            Task3();
            Task4();
        }

        private void Task1()
        {
            // Create sample space: numbers 1 to 10
            var outcomes = Enumerable.Range(1, 10);
            var model = new ClassicalProbabilityModel<int>(outcomes);

            // Define events
            var A_outcomes = new List<int> { 2, 4, 6, 8, 10 }; // even numbers
            var eventA = new ClassicalEvent<int>(model, A_outcomes);

            var B_outcomes = new List<int> { 5, 10 }; // multiples of 5
            var eventB = new ClassicalEvent<int>(model, B_outcomes);

            // Calculate probabilities
            double pAgivenB = eventA.ConditionalProbability(eventB);
            double pBgivenA = eventB.ConditionalProbability(eventA);

            Console.WriteLine("Task 1:");
            Console.WriteLine($"P(A|B) = {FormatProbability(pAgivenB)}");
            Console.WriteLine($"P(B|A) = {FormatProbability(pBgivenA)}\n");
        }

        private void Task2()
        {
            // Create sample space: all coin combinations
            var outcomes = new List<string> { "HH", "HT", "TH", "TT" };
            var model = new ClassicalProbabilityModel<string>(outcomes);

            // Define events
            var eventA = new ClassicalEvent<string>(model, new List<string> { "HH" }); // both heads
            var eventB = new ClassicalEvent<string>(model, new List<string> { "HH", "HT", "TH" }); // at least one head

            // Calculate required probabilities
            double pAandB = eventA.Intersection(eventB).Probability;
            double pB = eventB.Probability;
            double pAgivenB = eventA.ConditionalProbability(eventB);

            Console.WriteLine("Task 2:");
            Console.WriteLine($"P(A∩B) = {FormatProbability(pAandB)}");
            Console.WriteLine($"P(B) = {FormatProbability(pB)}");
            Console.WriteLine($"P(A|B) = {FormatProbability(pAgivenB)}\n");
        }

        private void Task3()
        {
            // Create sample space: all dice combinations
            var outcomes = new List<(int, int)>();
            for (int i = 1; i <= 6; i++)
                for (int j = 1; j <= 6; j++)
                    outcomes.Add((i, j));

            var model = new ClassicalProbabilityModel<(int, int)>(outcomes);

            // Define condition: both dice even
            var condition = outcomes.Where(o => o.Item1 % 2 == 0 && o.Item2 % 2 == 0).ToList();
            var eventCondition = new ClassicalEvent<(int, int)>(model, condition);

            // Define target: sum equals 8
            var target = condition.Where(o => o.Item1 + o.Item2 == 8).ToList();
            var eventTarget = new ClassicalEvent<(int, int)>(model, target);

            // Calculate conditional probability
            double probability = eventTarget.ConditionalProbability(eventCondition);

            Console.WriteLine("Task 3:");
            Console.WriteLine($"Conditional probability = {FormatProbability(probability)}\n");
        }

        private void Task4()
        {
            // Create sample space with 6 outcomes
            var outcomes = new List<string>
            {
                "Card1_Side1", "Card1_Side2",
                "Card2_Side1", "Card2_Side2",
                "Card3_Side1", "Card3_Side2"
            };

            var model = new ClassicalProbabilityModel<string>(outcomes);

            // Part 1: Probability of black bottom
            var blackBottom = outcomes.Where(o =>
                o.StartsWith("Card2") ||  // Both sides black
                (o.StartsWith("Card3") && o.EndsWith("Side1")) // Card3 white-up/black-down
            ).ToList();

            var eventBlackBottom = new ClassicalEvent<string>(model, blackBottom);
            double pBlackBottom = eventBlackBottom.Probability;

            // Part 2: Conditional probability given white top
            var whiteTop = outcomes.Where(o =>
                o.StartsWith("Card1") ||  // Both sides white
                (o.StartsWith("Card3") && o.EndsWith("Side1")))
                .ToList();

            var eventWhiteTop = new ClassicalEvent<string>(model, whiteTop);

            // Intersection: black bottom AND white top
            var blackBottomWhiteTop = blackBottom.Intersect(whiteTop).ToList();
            var eventBoth = new ClassicalEvent<string>(model, blackBottomWhiteTop);

            double pConditional = eventBoth.Probability / eventWhiteTop.Probability;

            Console.WriteLine("Task 4:");
            Console.WriteLine($"Part 1: P(black bottom) = {FormatProbability(pBlackBottom)}");
            Console.WriteLine($"Part 2: P(black bottom | white top) = {FormatProbability(pConditional)}");
        }
    }
}
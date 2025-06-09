using ProbabilityConsolePrjct.ProbabilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbabilityConsolePrjct.Tasks
{
    public class AdditionalProbabilityTasks: ProbabilityTasks
    {
        public void RunAllTasks()
        {
            Task1();
            Task2();
            Task3();
        }

        private void Task1()
        {
            // Sample space: numbers 1 to 12
            var outcomes = Enumerable.Range(1, 12);
            var model = new ClassicalProbabilityModel<int>(outcomes);

            // Define events
            var A_outcomes = new List<int> { 3, 6, 9, 12 }; // multiples of 3
            var B_outcomes = new List<int> { 2, 4, 6, 8, 10, 12 }; // even numbers

            var eventA = new ClassicalEvent<int>(model, A_outcomes);
            var eventB = new ClassicalEvent<int>(model, B_outcomes);

            // Calculate required probabilities
            double pA = eventA.Probability;
            double pB = eventB.Probability;
            double pAandB = eventA.Intersection(eventB).Probability;
            double pA_times_pB = pA * pB;
            bool areIndependent = Math.Abs(pAandB - pA_times_pB) < 1e-9;

            Console.WriteLine("Task 1:");
            Console.WriteLine($"P(A∩B) = {FormatProbability(pAandB)}");
            Console.WriteLine($"P(A)·P(B) = {FormatProbability(pA_times_pB)}");
            Console.WriteLine($"Are A and B independent? {(areIndependent ? "Yes" : "No")}\n");
        }

        private void Task2()
        {
            // Sample space: numbers 1 to 12
            var outcomes = Enumerable.Range(1, 12);
            var model = new ClassicalProbabilityModel<int>(outcomes);

            // Define events
            var A_outcomes = new List<int> { 3, 6, 9, 12 }; // multiples of 3
            var C_outcomes = new List<int> { 1, 2, 3, 4 }; // numbers ≤ 4

            var eventA = new ClassicalEvent<int>(model, A_outcomes);
            var eventC = new ClassicalEvent<int>(model, C_outcomes);

            // Calculate required probabilities
            double pA = eventA.Probability;
            double pC = eventC.Probability;
            double pAandC = eventA.Intersection(eventC).Probability;
            double pA_times_pC = pA * pC;
            bool areIndependent = Math.Abs(pAandC - pA_times_pC) < 1e-9;

            Console.WriteLine("Task 2:");
            Console.WriteLine($"P(A∩C) = {FormatProbability(pAandC)}");
            Console.WriteLine($"P(A)·P(C) = {FormatProbability(pA_times_pC)}");
            Console.WriteLine($"Are A and C independent? {(areIndependent ? "Yes" : "No")}\n");
        }

        private void Task3()
        {
            /* 
            Вероятность попадания = P(нет осечки) * P(попадание|нет осечки)
            P(нет осечки) = 1 - 0.3 = 0.7
            P(попадание|нет осечки) = 0.2
            Итоговая вероятность = 0.7 * 0.2 = 0.14
            */
            double probability = 0.7 * 0.2;

            Console.WriteLine("Task 3:");
            Console.WriteLine($"Probability of hitting the target = {FormatProbability(probability)}");
        }
    }
}

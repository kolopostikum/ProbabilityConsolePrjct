using ProbabilityConsolePrjct.ProbabilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbabilityConsolePrjct.Tasks
{
    public class CoinTasks : ProbabilityTasks
    {
        public void RunAllTasks()
        {
            // Создаем пространство исходов для двух монет
            var outcomes = new List<string> { "HH", "HT", "TH", "TT" };
            var model = new ClassicalProbabilityModel<string>(outcomes);

            // Определяем события
            var eventA = new ClassicalEvent<string>(model, new List<string> { "HH", "HT" }); // Первая монета - орёл
            var eventB = new ClassicalEvent<string>(model, new List<string> { "HH", "TH" }); // Вторая монета - орёл
            var eventC = new ClassicalEvent<string>(model, new List<string> { "HT", "TH" }); // Ровно один орёл

            // Вычисляем вероятности пересечений
            double pAandB = eventA.Intersection(eventB).Probability; // A ∩ B
            double pAandC = eventA.Intersection(eventC).Probability; // A ∩ C
            double pBandC = eventB.Intersection(eventC).Probability; // B ∩ C
            double pAandBandC = eventA.Intersection(eventB).Intersection(eventC).Probability; // A ∩ B ∩ C

            // Проверяем независимость пар и тройки событий
            bool independentAB = eventA.IsIndependent(eventB);
            bool independentAC = eventA.IsIndependent(eventC);
            bool independentBC = eventB.IsIndependent(eventC);
            bool independentABC = ClassicalEvent<string>.CheckMutuallyIndependent(new[] { eventA, eventB, eventC });

            Console.WriteLine("Coin Tossing Task:");
            Console.WriteLine($"P(A∩B) = {FormatProbability(pAandB)}");
            Console.WriteLine($"P(A∩C) = {FormatProbability(pAandC)}");
            Console.WriteLine($"P(B∩C) = {FormatProbability(pBandC)}");
            Console.WriteLine($"P(A∩B∩C) = {FormatProbability(pAandBandC)}\n");

            Console.WriteLine("Independent pairs:");
            Console.WriteLine($"A and B: {independentAB}");
            Console.WriteLine($"A and C: {independentAC}");
            Console.WriteLine($"B and C: {independentBC}");
            Console.WriteLine($"A, B and C together: {independentABC}");
        }
    }
}

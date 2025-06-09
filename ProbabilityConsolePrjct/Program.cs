using ProbabilityConsolePrjct.Tasks;
using System;

namespace ProbabilityConsolePrjct
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Conditional probability tasks:\n");
            var conditionaTasks = new ConditionalProbabilityTask();
            conditionaTasks.RunAllTasks();

            Console.WriteLine("\n");
            Console.WriteLine("Additional probability tasks:\n");
            var additionalTasks = new AdditionalProbabilityTasks();
            additionalTasks.RunAllTasks();

            Console.WriteLine("\n");
            Console.WriteLine("Commission probability tasks:\n");
            var commissionTasks = new CommissionTasks();
            commissionTasks.RunAllTasks();

            Console.WriteLine("\n");
            Console.WriteLine("Coin probability tasks:\n");
            var coinTasks = new CoinTasks();
            coinTasks.RunAllTasks();

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbabilityConsolePrjct.Tasks
{
    public class CommissionTasks: ProbabilityTasks
    {
        public void RunAllTasks()
        {
            Task1();
            Task2();
            Task3();
            Task4();
        }

        // Задача 1: Третий голосует как председатель
        private void Task1()
        {
            double pChairman = 0.7;  // Вероятность правильного решения председателя
            double pExpert = 0.9;     // Вероятность правильного решения эксперта

            /* 
            Третий всегда голосует как председатель, поэтому:
            - Если председатель прав, третий тоже прав (комиссия принимает правильное решение)
            - Если председатель ошибается, третий тоже ошибается (комиссия принимает неправильное решение)
            */
            double pCorrect = pChairman;

            Console.WriteLine("Task 1: Commission with third member following chairman");
            Console.WriteLine($"Probability of correct decision: {FormatProbability(pCorrect)}\n");
        }

        // Задача 2: Все трое принимают правильное решение (третий подбрасывает монетку)
        private void Task2()
        {
            double pChairman = 0.7;
            double pExpert = 0.9;
            double pThird = 0.5;  // Вероятность правильного решения третьего

            // Все решения независимы
            double pAllCorrect = pChairman * pExpert * pThird;

            Console.WriteLine("Task 2: All three make correct decisions (third flips coin)");
            Console.WriteLine($"Probability: {FormatProbability(pAllCorrect)}\n");
        }

        // Задача 3: Ровно двое принимают правильное решение (третий подбрасывает монетку)
        private void Task3()
        {
            double pChairman = 0.7;
            double pExpert = 0.9;
            double pThird = 0.5;

            // Три случая:
            // 1. Председатель и эксперт правильные, третий ошибается
            double case1 = pChairman * pExpert * (1 - pThird);

            // 2. Председатель и третий правильные, эксперт ошибается
            double case2 = pChairman * (1 - pExpert) * pThird;

            // 3. Эксперт и третий правильные, председатель ошибается
            double case3 = (1 - pChairman) * pExpert * pThird;

            double pExactlyTwoCorrect = case1 + case2 + case3;

            Console.WriteLine("Task 3: Exactly two make correct decisions (third flips coin)");
            Console.WriteLine($"Probability: {FormatProbability(pExactlyTwoCorrect)}\n");
        }

        // Задача 4: Комиссия принимает правильное решение (большинством голосов, третий подбрасывает монетку)
        private void Task4()
        {
            double pChairman = 0.7;
            double pExpert = 0.9;
            double pThird = 0.5;

            // Комиссия принимает правильное решение в случаях:
            // 1. Все трое правильные
            double allCorrect = pChairman * pExpert * pThird;

            // 2. Ровно двое правильные (уже вычислено в Task3)
            double exactlyTwoCorrect =
                pChairman * pExpert * (1 - pThird) +
                pChairman * (1 - pExpert) * pThird +
                (1 - pChairman) * pExpert * pThird;

            double pCorrectDecision = allCorrect + exactlyTwoCorrect;

            Console.WriteLine("Task 4: Commission makes correct decision (third flips coin)");
            Console.WriteLine($"Probability: {FormatProbability(pCorrectDecision)}");
        }

    }
}

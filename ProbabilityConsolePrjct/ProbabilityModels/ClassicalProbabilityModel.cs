using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbabilityConsolePrjct.ProbabilityModels
{
    /// <summary>
    /// Классическая модель, которая представляет собой набор исходов.
    /// Аксиомы: 
    /// 1) Множество исходов исчерпывает все возможные исходы.
    /// 2) Все исходы имеют равную вероятность.
    /// 3) Никакие два исхода не могут быть одновременно.
    /// </summary>
    /// <typeparam name="T">Тип исходов</typeparam>
    public class ClassicalProbabilityModel<T> : IProbabilityModel<T>
    {
        /// <summary>
        /// Множество исходов
        /// </summary>
        private HashSet<T> outcomes = new();
        public ClassicalProbabilityModel(IEnumerable<T> outcomes)
        {
            this.outcomes = outcomes.ToHashSet();
        }
        /// <summary>
        /// Возвращает множество исходов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetOutcomes() => this.outcomes;

        /// <summary>
        /// Добавляет исход
        /// </summary>
        /// <param name="outcome">собственно исход</param>
        /// <returns>получилось ли добавить</returns>
        public bool AddOutcome(T outcome) => outcomes.Add(outcome); 

        /// <summary>
        /// Возвращает мощность множества исходов
        /// </summary>
        /// <returns>количество исходов</returns>
        public int CountOutcomes() => outcomes.Count;

        /// <summary>
        /// Удаляет исход
        /// </summary>
        /// <param name="outcome">собственно исход</param>
        /// <returns>получилось ли удалить</returns>
        public bool RemoveOutcome(T outcome) { return outcomes.Remove(outcome); }

        /// <summary>
        /// Возвращает вероятность события
        /// </summary>
        /// <param name="outcome">собственно исход</param>
        /// <returns>вероятность</returns>
        public double GetProbability(T outcome)
        {
            if (this.outcomes.Count == 0) return 0.0;
            if (!outcomes.Contains(outcome)) return 0.0;
            return 1.0 / outcomes.Count;// вероятность у одного исхода всегда равна 1/мощность множества исходов
        }

        /// <summary>
        /// Возвращает вероятность события
        /// </summary>
        /// <param name="outcomes">множество положительных событий</param>
        /// <returns>вероятность положительного исхода</returns>
        public double GetProbability(IEnumerable<T> outcomes)
        {
            if (!outcomes.Any()) return 0.0;// вероятность у пустого множества исходов всегда равна 0
            var favorable = outcomes.Count(o => this.outcomes.Contains(o));

            if (this.outcomes.Count == 0) return 0.0;
            return (double)favorable / this.outcomes.Count;// вероятность у n исходов всегда равна n/мощность множества исходов
        }
    }
}

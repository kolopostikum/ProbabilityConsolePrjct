using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public class ClassicalEvent<T>
    {
        private readonly HashSet<T> _outcomes;
        private readonly ClassicalProbabilityModel<T> _probabilityModel;

        /// <summary>
        /// Создает событие на основе модели вероятности
        /// </summary>
        public ClassicalEvent(
            ClassicalProbabilityModel<T> probabilityModel,
            IEnumerable<T> favorableOutcomes)
        {
            _probabilityModel = probabilityModel ??
                throw new ArgumentNullException(nameof(probabilityModel));

            var allOutcomes = probabilityModel.GetOutcomes().ToHashSet();
            _outcomes = favorableOutcomes?
                .Where(o => allOutcomes.Contains(o))
                .ToHashSet() ?? new HashSet<T>();
        }

        /// <summary>
        /// Свойство: Если у эксперимента есть n элементарных исходов,
        /// то количество соответствующих ему событий равно 2n.
        /// </summary>
        public int EventsCount()
        {
            return (int)Math.Pow(2.0, _probabilityModel.CountOutcomes());
        }

        /// <summary>
        /// Исходы, благоприятствующие событию
        /// </summary>
        public IEnumerable<T> FavorableOutcomes => _outcomes;

        /// <summary>
        /// Вероятность события
        /// </summary>
        public double Probability =>
            _probabilityModel.GetProbability(_outcomes);

        /// <summary>
        /// Проверяет, благоприятствует ли исход событию
        /// </summary>
        public bool IsFavorable(T outcome) => _outcomes.Contains(outcome);

        /// <summary>
        /// Дополнение события (противоположное событие)
        /// </summary>
        public ClassicalEvent<T> Complement()
        {
            var allOutcomes = _probabilityModel.GetOutcomes();
            var complement = allOutcomes.Except(_outcomes);
            return new ClassicalEvent<T>(_probabilityModel, complement);
        }

        /// <summary>
        /// Объединение с другим событием (A ∪ B)
        /// </summary>
        public ClassicalEvent<T> Union(ClassicalEvent<T> other)
        {
            ValidateSameModel(other);
            return new ClassicalEvent<T>(
                _probabilityModel,
                _outcomes.Union(other._outcomes)
            );
        }

        /// <summary>
        /// Пересечение с другим событием (A ∩ B)
        /// </summary>
        public ClassicalEvent<T> Intersection(ClassicalEvent<T> other)
        {
            ValidateSameModel(other);
            return new ClassicalEvent<T>(
                _probabilityModel,
                _outcomes.Intersect(other._outcomes)
            );
        }

        /// <summary>
        /// Разность с другим событием (A \ B)
        /// </summary>
        public ClassicalEvent<T> Difference(ClassicalEvent<T> other)
        {
            ValidateSameModel(other);
            return new ClassicalEvent<T>(
                _probabilityModel,
                _outcomes.Except(other._outcomes)
            );
        }

        /// <summary>
        /// Условная вероятность P(A|B)
        /// </summary>
        public double ConditionalProbability(ClassicalEvent<T> condition)
        {
            ValidateSameModel(condition);
            var intersection = Intersection(condition);
            if (intersection.Probability == 0.0) return 0.0;
            // P(A ∩ B) / P(B)
            return intersection.Probability / condition.Probability;
        }

        /// <summary>
        /// Проверяет независимость событий: P(A∩B) = P(A)*P(B)
        /// </summary>
        public bool IsIndependent(ClassicalEvent<T> other)
        {
            ValidateSameModel(other);
            var intersectionProb = Intersection(other).Probability;
            return Math.Abs(intersectionProb - Probability * other.Probability) < 1e-9;
        }

        private void ValidateSameModel(ClassicalEvent<T> other)
        {
            if (!ReferenceEquals(_probabilityModel, other._probabilityModel))
                throw new InvalidOperationException("События принадлежат разным вероятностным моделям");
        }

        /// <summary>
        /// Строковое представление события
        /// </summary>
        public override string ToString() =>
            $"Событие (|A|={_outcomes.Count}, P={Probability:P})";

        public static ClassicalEvent<T> operator !(ClassicalEvent<T> e) => e.Complement();
        public static ClassicalEvent<T> operator |(ClassicalEvent<T> a, ClassicalEvent<T> b) => a.Union(b);
        public static ClassicalEvent<T> operator &(ClassicalEvent<T> a, ClassicalEvent<T> b) => a.Intersection(b);

        public static bool AreMutuallyExclusive(ClassicalEvent<T> a, ClassicalEvent<T> b)
        {
            return !a.Intersection(b).FavorableOutcomes.Any();
        }
    }
}

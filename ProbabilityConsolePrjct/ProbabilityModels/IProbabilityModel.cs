using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbabilityConsolePrjct.ProbabilityModels
{
    /// <summary>
    /// Интерфейс модели вероятностей
    /// </summary>
    /// <typeparam name="T">Тип события</typeparam>
    internal interface IProbabilityModel<T>
    {
        /// <summary>
        /// Вероятность события
        /// </summary>
        /// <param name="outcome">Событие</param>
        /// <returns></returns>
        public double GetProbability(T outcome);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbabilityConsolePrjct.Tasks
{
    public class ProbabilityTasks
    {
        public string FormatProbability(double p)
        {
            // Try to convert to simplified fraction
            for (int denom = 1; denom <= 20; denom++)
            {
                double numerator = p * denom;
                double rounded = Math.Round(numerator);
                if (Math.Abs(numerator - rounded) < 0.0001)
                {
                    int num = (int)rounded;
                    int den = denom;
                    SimplifyFraction(ref num, ref den);
                    return $"{num}/{den}";
                }
            }
            return p.ToString("F3");
        }

        private void SimplifyFraction(ref int numerator, ref int denominator)
        {
            int gcd = GCD(numerator, denominator);
            numerator /= gcd;
            denominator /= gcd;
        }

        private int GCD(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}

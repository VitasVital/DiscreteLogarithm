using DiscreteLogarithm.MathFunctionsForCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

namespace DiscreteLogarithm.ExponentialAlgorithms
{
    // Алгоритм https://ilovecalc.com/calcs/maths/baby-step-giant-step/1382/
    public class Shenks
    {
        MathFunctions mathFunctions;
        public Shenks()
        {
            mathFunctions = new MathFunctions();
        }

        public void CheckingTheInputValues(
            string input_g,
            string input_A,
            string input_p,
            Label inputLabel,
            ref bool theValuesAreCorrect,
            out BigInteger g,
            out BigInteger A,
            out BigInteger p)
        {
            inputLabel.Text = "";
            if (!BigInteger.TryParse(input_g, out g))
            {
                theValuesAreCorrect = false;
                inputLabel.Text = "Ошибка ввода числа g";
            };
            if (!BigInteger.TryParse(input_A, out A))
            {
                theValuesAreCorrect = false;
                inputLabel.Text += "\nОшибка ввода числа A";
            };
            if (!BigInteger.TryParse(input_p, out p))
            {
                theValuesAreCorrect = false;
                inputLabel.Text += "\nОшибка ввода числа p";
            };
        }

        public void CalculateShenks(BigInteger g, BigInteger A, BigInteger p, Label inputLabel)
        {
            BigInteger m, k;
            Step1(p, out m, out k);

            List<BigInteger> g_km_degree = new List<BigInteger>();
            List<BigInteger> Ag_m_degree = new List<BigInteger>();

            Step2(g_km_degree, Ag_m_degree, g, A, p, m, k);

            int i, j;
            Step3(g_km_degree, Ag_m_degree, out i, out j);
            BigInteger result = BigInteger.Multiply(i, m) - j;

            inputLabel.Text = "Результат = " + result.ToString();
        }

        private BigInteger Sqrt(BigInteger number)
        {
            BigInteger n = 0, p = 0;
            if (number == BigInteger.Zero)
            {
                return BigInteger.Zero;
            }
            var high = number >> 1;
            var low = BigInteger.Zero;

            while (high > low + 1)
            {
                n = (high + low) >> 1;
                p = n * n;
                if (number < p)
                {
                    high = n;
                }
                else
                {
                    if (number > p)
                    {
                        low = n;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return number == p ? n : low;
        }

        private void Step1(BigInteger p, out BigInteger m, out BigInteger k)
        {
            m = k = Sqrt(p) + 1;
        }

        private void Step2(List<BigInteger> g_km_degree, List<BigInteger> Ag_m_degree, BigInteger g, BigInteger A, BigInteger p, BigInteger m, BigInteger k)
        {
            for (BigInteger k_i = 1; k_i <= k; k_i++)
            {
                g_km_degree.Add(mathFunctions.ExponentiationModulo(g, k_i * m, p));
            }
            for (int m_i = 0; m_i <= m - 1; m_i++)
            {
                Ag_m_degree.Add(mathFunctions.ExponentiationModulo(A * BigInteger.Pow(g, m_i), 1, p)); 
            }
        }

        private void Step3(List<BigInteger> g_km_degree, List<BigInteger> Ag_m_degree, out int ind_i, out int ind_j)
        {
            for(int i = 0; i < g_km_degree.Count; i++)
            {
                for (int j = 0; j < Ag_m_degree.Count; j++)
                {
                    if (g_km_degree[i] == Ag_m_degree[j])
                    {
                        ind_i = i + 1; 
                        ind_j = j; 
                        return;
                    }
                }
            }
            ind_i = 0;
            ind_j = 0;
        }
    }
}

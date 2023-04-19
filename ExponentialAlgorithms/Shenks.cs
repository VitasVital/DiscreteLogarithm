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
            string input_a,
            string input_b,
            string input_p,
            Label inputLabel,
            ref bool theValuesAreCorrect,
            out BigInteger a,
            out BigInteger b,
            out BigInteger p)
        {
            inputLabel.Text = "";
            if (!BigInteger.TryParse(input_a, out a))
            {
                theValuesAreCorrect = false;
                inputLabel.Text = "Ошибка ввода числа а";
            };
            if (!BigInteger.TryParse(input_b, out b))
            {
                theValuesAreCorrect = false;
                inputLabel.Text += "\nОшибка ввода числа b";
            };
            if (!BigInteger.TryParse(input_p, out p))
            {
                theValuesAreCorrect = false;
                inputLabel.Text += "\nОшибка ввода числа p";
            };
        }

        public void CalculateShenks(BigInteger a, BigInteger b, BigInteger p, Label inputLabel)
        {
            BigInteger m, k;
            Step1(p, out m, out k);

            List<BigInteger> a_km_degree = new List<BigInteger>();
            List<BigInteger> ba_m_degree = new List<BigInteger>();

            Step2(a_km_degree, ba_m_degree, a, b, p, m, k);

            int i, j;
            Step3(a_km_degree, ba_m_degree, out i, out j);
            BigInteger result = BigInteger.Multiply(i, m) - j;

            inputLabel.Text = "Результат = " + result.ToString();
        }

        private void Step1(BigInteger p, out BigInteger m, out BigInteger k)
        {
            m = k = (BigInteger)Math.Sqrt((double)p) + 1;
        }

        private void Step2(List<BigInteger> a_km_degree, List<BigInteger> ba_m_degree, BigInteger a, BigInteger b, BigInteger p, BigInteger m, BigInteger k)
        {
            for (BigInteger k_i = 1; k_i <= k; k_i++)
            {
                a_km_degree.Add(BigInteger.ModPow(a, BigInteger.Multiply(k_i, m), p));
            }
            for (int m_i = 0; m_i <= m - 1; m_i++)
            {
                ba_m_degree.Add(BigInteger.Multiply(b, BigInteger.Pow(a, m_i)) % p);
            }
        }

        private void Step3(List<BigInteger> a_km_degree, List<BigInteger> ba_m_degree, out int ind_i, out int ind_j)
        {
            for(int i = 0; i < a_km_degree.Count; i++)
            {
                for (int j = 0; j < ba_m_degree.Count; j++)
                {
                    if (a_km_degree[i] == ba_m_degree[j])
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

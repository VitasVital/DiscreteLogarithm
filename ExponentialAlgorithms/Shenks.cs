﻿using DiscreteLogarithm.MathFunctionsForCalculation;
using System.Numerics;
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
            if (!BigInteger.TryParse(input_g, out g) || g <= 0)
            {
                theValuesAreCorrect = false;
                inputLabel.Text = "Ошибка g";
            };
            if (!BigInteger.TryParse(input_A, out A) || A <= 0)
            {
                theValuesAreCorrect = false;
                inputLabel.Text += "\nОшибка A";
            };
            if (!BigInteger.TryParse(input_p, out p) || p <= 0)
            {
                theValuesAreCorrect = false;
                inputLabel.Text += "\nОшибка p";
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

            inputLabel.Text = "Результат: \na = " + result.ToString();
        }

        private void Step1(BigInteger p, out BigInteger m, out BigInteger k)
        {
            m = k = mathFunctions.Sqrt(p) + 1;
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

using DiscreteLogarithm.MathFunctionsForCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using Label = System.Windows.Forms.Label;

namespace DiscreteLogarithm.ExponentialAlgorithms
{
    public class ListGroupedValues
    {
        public ListGroupedValues(BigInteger Key, int degree_number) 
        {
            this.Key = Key;
            this.degree_number = degree_number;
            this.x_values = new List<int> { 0 };
        }

        public BigInteger Key { get; set; }
        public int degree_number { get; set; }
        public List<int> x_values { get; set; }
    }

    public class PoligHellman
    {
        RoPollard roPollard;
        MathFunctions mathFunctions;
        public PoligHellman()
        {
            roPollard = new RoPollard();
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

        public void CalculatePoligHellman(BigInteger g, BigInteger A, BigInteger p, Label inputLabel)
        {
            BigInteger fi_p = p - 1;
            List<BigInteger> p_dividers = mathFunctions.Factorization(fi_p);
            List<ListGroupedValues> fi_p_dividers_grouped = p_dividers.GroupBy(x => x).Select(group => new ListGroupedValues(group.Key, group.Count())).ToList();

            List<List<BigInteger>> step1_result = new List<List<BigInteger>>();
            for (int i = 0; i < fi_p_dividers_grouped.Count; i++)
            {
                step1_result.Add(Step1(g, fi_p_dividers_grouped[i].degree_number, fi_p, p));
            }

            Step2(fi_p_dividers_grouped, g, A, p);

            //BigInteger factorizated_num1 = roPollard.ro_Pollard(p - 1);
            //BigInteger factorizated_num2 = (p - 1) / factorizated_num1;

            //IList<BigInteger> calculated_g_A_1 = Step1(factorizated_num1, g, A, p);
            //IList<BigInteger> calculated_g_A_2 = Step1(factorizated_num2, g, A, p);

            //BigInteger x_1 = Step2(calculated_g_A_1, p);
            //BigInteger x_2 = Step2(calculated_g_A_2, p);

            //calculated_g_A_1.Add(x_1);
            //calculated_g_A_2.Add(x_2);

            //BigInteger x_main = Step3(calculated_g_A_1, calculated_g_A_2, g, A, p);

            //inputLabel.Text = string.Format("Результат = {0}", x_main);
        }

        public List<BigInteger> Step1(BigInteger g, BigInteger q_i, BigInteger fi_p, BigInteger p)
        {
            List<BigInteger> step1_result = new List<BigInteger>();
            for (BigInteger j = 0; j <= q_i - 1; j++)
            {
                step1_result.Add(BigInteger.ModPow(g, j * fi_p / q_i, p));
            }
            return step1_result;
        }

        public BigInteger Step2(List<ListGroupedValues> fi_p_dividers_grouped, BigInteger g, BigInteger A, BigInteger p)
        {
            for (int i = 0; i < fi_p_dividers_grouped.Count; i++)
            {
                Step2_find_x(fi_p_dividers_grouped[i], g, A, p);
            }
            return 1;
        }

        public void Step2_find_x(ListGroupedValues fi_p_dividers_grouped_i, BigInteger g, BigInteger A, BigInteger p)
        {
            for (int i = 0; i < fi_p_dividers_grouped_i.degree_number; i++)
            {
                int x_ind = 0;
                BigInteger g_x_q = BigInteger.Pow(g, -fi_p_dividers_grouped_i.x_values[0]);
                BigInteger A_g_x;
                while (true)
                {
                    for (int j = 1; j < fi_p_dividers_grouped_i.x_values.Count; j++)
                    {
                        g_x_q *= -BigInteger.Pow(g, (int)(-fi_p_dividers_grouped_i.x_values[j] * BigInteger.Pow(fi_p_dividers_grouped_i.degree_number, j)));
                    }
                    A_g_x = BigInteger.ModPow(A * BigInteger.Pow(g, (int)g_x_q), (p - 1) / BigInteger.Pow(fi_p_dividers_grouped_i.degree_number, i + 1), p);
                    if(BigInteger.ModPow(g, x_ind * (p - 1) / fi_p_dividers_grouped_i.degree_number, p) == A_g_x)
                    {
                        fi_p_dividers_grouped_i.x_values[i] = x_ind;
                    }
                    x_ind++;
                }
            }
        }

        public BigInteger Step3(IList<BigInteger> calculated_g_A_1, IList<BigInteger> calculated_g_A_2, BigInteger g, BigInteger A, BigInteger p)
        {
            //BigInteger i = 0;
            //while (true)
            //{
            //    BigInteger x_1 = mathFunctions.ExponentiationModulo(i, 1, calculated_a_b_2[2]);
            //    BigInteger x_2 = mathFunctions.ExponentiationModulo(i, 1, calculated_a_b_1[2]);
            //    if (x_1 != calculated_a_b_1[3] || x_2 != calculated_a_b_2[3])
            //    {
            //        i++;
            //        continue;
            //    }
            //    BigInteger x_main = mathFunctions.ExponentiationModulo(a, i, p);
            //    if (x_main == b)
            //    {
            //        return i;
            //    }
            //    i++;
            //}
            BigInteger i = 0;
            BigInteger x;
            BigInteger x_2;
            BigInteger x_main;
            while (true)
            {
                x = calculated_g_A_2[2] * i + calculated_g_A_1[3];
                x_2 = mathFunctions.ExponentiationModulo(x, 1, calculated_g_A_1[2]);
                i++;
                if (x_2 != calculated_g_A_2[3])
                {
                    continue;
                }
                x_main = mathFunctions.ExponentiationModulo(g, x, p);
                if (x_main == A)
                {
                    return x;
                }
            }
        }
    }
}

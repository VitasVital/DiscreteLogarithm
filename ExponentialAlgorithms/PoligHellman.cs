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
        public ListGroupedValues(BigInteger Key, int degree_number, BigInteger key_degree) 
        {
            this.Key = Key;
            this.degree_number = degree_number;
            this.key_degree = key_degree;
        }

        public BigInteger Key { get; set; }
        public int degree_number { get; set; }
        public BigInteger key_degree { get; set; }
    }

    public class PoligHellman
    {
        MathFunctions mathFunctions;
        public PoligHellman()
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
                inputLabel.Text = "Ошибка ввода числа g";
            };
            if (!BigInteger.TryParse(input_A, out A) || A <= 0)
            {
                theValuesAreCorrect = false;
                inputLabel.Text += "\nОшибка ввода числа A";
            };
            if (!BigInteger.TryParse(input_p, out p) || p <= 0)
            {
                theValuesAreCorrect = false;
                inputLabel.Text += "\nОшибка ввода числа p";
            };
        }

        public void CalculatePoligHellman(BigInteger g, BigInteger A, BigInteger p, Label inputLabel)
        {
            BigInteger fi_p = p - 1;
            List<BigInteger> p_dividers = mathFunctions.Factorization(fi_p);
            List<ListGroupedValues> fi_p_dividers_grouped = p_dividers.GroupBy(x => x).Select(group => new ListGroupedValues(group.Key, group.Count(), BigInteger.Pow(group.Key, group.Count()))).ToList();

            List<List<BigInteger>> step1_result = Step1(fi_p_dividers_grouped, g, fi_p, p);

            List<List<BigInteger>> step2_result = Step2(fi_p_dividers_grouped, step1_result, g, A, p);

            BigInteger step3_result = Step3(fi_p_dividers_grouped, step2_result);

            inputLabel.Text = string.Format("Результат = {0}", step3_result);
        }

        private List<List<BigInteger>> Step1(List<ListGroupedValues> fi_p_dividers_grouped, BigInteger g, BigInteger fi_p, BigInteger p)
        {
            List<List<BigInteger>> step1_result = new List<List<BigInteger>>();
            for (int i = 0; i <  fi_p_dividers_grouped.Count; i++)
            {
                List<BigInteger> step1_result_i = new List<BigInteger>();
                for (int j = 0; j < fi_p_dividers_grouped[i].Key; j++)
                {
                    step1_result_i.Add(mathFunctions.ExponentiationModulo(g, j * fi_p / fi_p_dividers_grouped[i].Key, p));
                }
                step1_result.Add(step1_result_i);
            }
            return step1_result;
        }

        private List<List<BigInteger>> Step2(List<ListGroupedValues> fi_p_dividers_grouped, List<List<BigInteger>> step1_result, BigInteger g, BigInteger A, BigInteger p)
        {
            BigInteger Agmodp;
            BigInteger p_1_q_degree;
            List<List<BigInteger>> x_list = new List<List<BigInteger>>();
            for (int i = 0; i <  fi_p_dividers_grouped.Count; i++)
            {
                List<BigInteger> x_list_i = new List<BigInteger>() { 0 };
                for (int j = 0; j < fi_p_dividers_grouped[i].degree_number; j++)
                {
                    p_1_q_degree = (p - 1) / BigInteger.Pow(fi_p_dividers_grouped[i].Key, j + 1);
                    Agmodp = mathFunctions.ExponentiationModulo(A / BigInteger.Pow(g, CalculateDegreeStep2(fi_p_dividers_grouped[i].Key, x_list_i)), p_1_q_degree, p);
                    Find_x_j_Step2(Agmodp, step1_result[i], x_list_i);
                }
                x_list_i.RemoveAt(0);
                x_list.Add(x_list_i);
            }
            return x_list;
        }

        private int CalculateDegreeStep2(BigInteger q_i, List<BigInteger> x_list_i)
        {
            BigInteger result = 0;
            for (int j = 1; j < x_list_i.Count; j++)
            {
                result += x_list_i[j] * BigInteger.Pow(q_i, j - 1);
            }
            return (int)result;
        }

        private void Find_x_j_Step2(BigInteger Agmodp, List<BigInteger> step1_result_i, List<BigInteger> x_list_i)
        {
            for (int i = 0; i < step1_result_i.Count; i++)
            {
                if (Agmodp == step1_result_i[i])
                {
                    x_list_i.Add(i);
                    break;
                }
            }
        }

        private BigInteger Step3(List<ListGroupedValues> fi_p_dividers_grouped, List<List<BigInteger>> step2_result)
        {
            List<BigInteger> x_q = new List<BigInteger>();
            for (int i = 0; i < fi_p_dividers_grouped.Count; i++)
            {
                BigInteger x_q_i = 0;
                for (int x_i = 0; x_i < step2_result[i].Count; x_i++)
                {
                    x_q_i += step2_result[i][x_i] * BigInteger.Pow(fi_p_dividers_grouped[i].Key, x_i);
                }
                x_q_i %= fi_p_dividers_grouped[i].key_degree;
                x_q.Add(x_q_i);
            }

            bool x_result_true = true;
            BigInteger x_result = 0;
            BigInteger max_key_degree = 0;
            for (int i = 0; i < fi_p_dividers_grouped.Count; i++)
            {
                if (max_key_degree < fi_p_dividers_grouped[i].key_degree)
                {
                    x_result = x_q[i];
                    max_key_degree = fi_p_dividers_grouped[i].key_degree;
                }
            }
            while (true)
            {
                for (int i = 0; i < fi_p_dividers_grouped.Count; i++)
                {
                    if (x_result % fi_p_dividers_grouped[i].key_degree != x_q[i])
                    {
                        x_result_true = false;
                        break;
                    }
                }
                if (x_result_true == false)
                {
                    x_result += max_key_degree;
                    x_result_true = true;
                    continue;
                }
                return x_result;
            }
        }
    }
}

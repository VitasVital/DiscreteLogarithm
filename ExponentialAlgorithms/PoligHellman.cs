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

            List<List<BigInteger>> step1_result = Step1(fi_p_dividers_grouped, g, fi_p, p);

            Step2(fi_p_dividers_grouped, g, A, p);

            //inputLabel.Text = string.Format("Результат = {0}", x_main);
        }

        public List<List<BigInteger>> Step1(List<ListGroupedValues> fi_p_dividers_grouped, BigInteger g, BigInteger fi_p, BigInteger p)
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

        public BigInteger Step2(List<ListGroupedValues> fi_p_dividers_grouped, BigInteger g, BigInteger A, BigInteger p)
        {
            return 1;
        }

        public BigInteger Step3(IList<BigInteger> calculated_g_A_1, IList<BigInteger> calculated_g_A_2, BigInteger g, BigInteger A, BigInteger p)
        {
            return 1;
        }
    }
}

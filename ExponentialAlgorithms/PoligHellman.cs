using DiscreteLogarithm.MathFunctionsForCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Label = System.Windows.Forms.Label;

namespace DiscreteLogarithm.ExponentialAlgorithms
{
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
            BigInteger factorizated_num1 = roPollard.ro_Pollard(p - 1);
            BigInteger factorizated_num2 = (p - 1) / factorizated_num1;

            IList<BigInteger> calculated_g_A_1 = Step1(factorizated_num1, g, A, p);
            IList<BigInteger> calculated_g_A_2 = Step1(factorizated_num2, g, A, p);

            BigInteger x_1 = Step2(calculated_g_A_1, p);
            BigInteger x_2 = Step2(calculated_g_A_2, p);

            calculated_g_A_1.Add(x_1);
            calculated_g_A_2.Add(x_2);

            BigInteger x_main = Step3(calculated_g_A_1, calculated_g_A_2, g, A, p);

            inputLabel.Text = string.Format("Результат = {0}", x_main);
        }

        public IList<BigInteger> Step1(BigInteger factorizated_num, BigInteger g, BigInteger A, BigInteger p)
        {
            BigInteger g_simplified = mathFunctions.ExponentiationModulo(g, factorizated_num, p);
            BigInteger A_simplified = mathFunctions.ExponentiationModulo(A, factorizated_num, p);

            return new List<BigInteger> { g_simplified, A_simplified, factorizated_num };
        }

        public BigInteger Step2(IList<BigInteger> calculated_g_A, BigInteger p)
        {
            BigInteger i = 2;
            while(true)
            {
                if (mathFunctions.ExponentiationModulo(calculated_g_A[0], i, p) == calculated_g_A[1])
                {
                    return i;
                }
                i++;
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

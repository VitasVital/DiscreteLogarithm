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

        public void CalculatePoligHellman(BigInteger a, BigInteger b, BigInteger p, Label inputLabel)
        {
            BigInteger factorizated_num1 = roPollard.ro_Pollard(p - 1);
            BigInteger factorizated_num2 = (p - 1) / factorizated_num1;

            IList<BigInteger> calculated_a_b_1 = Step1(factorizated_num1, a, b, p);
            IList<BigInteger> calculated_a_b_2 = Step1(factorizated_num2, a, b, p);

            calculated_a_b_1.Add(factorizated_num1);
            calculated_a_b_2.Add(factorizated_num2);

            BigInteger x_1 = Step2(calculated_a_b_1, p);
            BigInteger x_2 = Step2(calculated_a_b_2, p);

            calculated_a_b_1.Add(x_1);
            calculated_a_b_2.Add(x_2);

            BigInteger x_main = Step3(calculated_a_b_1, calculated_a_b_2, a, b, p);

            inputLabel.Text = string.Format("Результат = {0}", x_main);
        }

        public IList<BigInteger> Step1(BigInteger factorizated_num, BigInteger a, BigInteger b, BigInteger p)
        {
            BigInteger a_simplified = mathFunctions.ExponentiationModulo(a, factorizated_num, p);
            BigInteger b_simplified = mathFunctions.ExponentiationModulo(b, factorizated_num, p);

            return new List<BigInteger> { a_simplified, b_simplified };
        }

        public BigInteger Step2(IList<BigInteger> calculated_a_b, BigInteger p)
        {
            BigInteger i = 2;
            while(true)
            {
                if (mathFunctions.ExponentiationModulo(calculated_a_b[0], i, p) == calculated_a_b[1])
                {
                    return i;
                }
                i++;
            }
        }

        public BigInteger Step3(IList<BigInteger> calculated_a_b_1, IList<BigInteger> calculated_a_b_2, BigInteger a, BigInteger b, BigInteger p)
        {
            BigInteger i = 1;
            while (true)
            {
                BigInteger x_1 = mathFunctions.ExponentiationModulo(i, 1, calculated_a_b_2[2]);
                BigInteger x_2 = mathFunctions.ExponentiationModulo(i, 1, calculated_a_b_1[2]);
                BigInteger x_main = mathFunctions.ExponentiationModulo(a, i, p);
                if (x_1 == calculated_a_b_1[3] && x_2 == calculated_a_b_2[3] && x_main == b)
                {
                    return i;
                }
                i++;
            }
        }
    }
}

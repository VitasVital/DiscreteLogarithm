using DiscreteLogarithm.MathFunctionsForCalculation;
using DiscreteLogarithm.SubExponentialAlgorithms;
using ExtendedNumerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteLogarithmCore.SubExponentialAlgorithms
{
    public class COS
    {
        BigInteger g;
        BigInteger p;
        BigInteger A;
        BigInteger H;
        BigInteger J;
        BigInteger L;
        BigRational expNumber;
        MathFunctions mathFunctions;
        List<BigInteger> q;

        public COS()
        {
            expNumber = new BigRational(2, 718, 1000); // 2.718
            mathFunctions = new MathFunctions();
            q = new List<BigInteger>() { 2 };
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

        public void CalculateCOS(BigInteger input_g, BigInteger input_A, BigInteger input_p, Label inputLabel)
        {
            //g = 21;
            //p = 127;
            //A = 34;

            g = input_g;
            p = input_p;
            A = input_A;
            Step1();
            //Step2();
            //Step3();
            //BigInteger result = Step4();


            inputLabel.Text = string.Format("Результат = {0}", 0);
        }

        private void Step1()
        {
            H = BigRational.Sqrt(p).WholePart + 1;
            J = H * H - p;
            BigInteger degree = BigRational.Sqrt(BigInteger.Log2(p) * BigInteger.Log2(BigInteger.Log2(p))).WholePart;
            L = (BigInteger)BigRational.Pow(expNumber, degree).FractionalPart;

            for (BigInteger i = 3; i < L; i += 2)
            {
                if (mathFunctions.TestMillerRabin(i) == "Вероятно простое")
                {
                    q.Add(i);
                }
            }
        }
    }
}

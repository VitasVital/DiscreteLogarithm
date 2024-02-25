using DiscreteLogarithm.ExponentialAlgorithms;
using DiscreteLogarithm.MathFunctionsForCalculation;
using ExtendedNumerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Label = System.Windows.Forms.Label;

namespace DiscreteLogarithm.SubExponentialAlgorithms
{
    public class ListGroupedValuesIndex
    {
        public ListGroupedValuesIndex(int index, List<ListGroupedValues> listGroupedValues)
        {
            this.index = index;
            this.listGroupedValues = listGroupedValues;
        }

        public int index { get; set; }
        public List<ListGroupedValues> listGroupedValues { get; set; }
    }

    public class Adleman
    {
        MathFunctions mathFunctions;
        BigRational expNumber;
        FactorBase primeFactorBase { get; set; }
        BigInteger B;
        List<ListGroupedValuesIndex> exponentiationModuloDividersGroupedList;
        public Adleman()
        {
            mathFunctions = new MathFunctions();
            expNumber = new BigRational(2, 718, 1000); // 2.718
            primeFactorBase = new FactorBase();
            B = 0;
            exponentiationModuloDividersGroupedList = new List<ListGroupedValuesIndex>();
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

        public void CalculateAdleman(BigInteger g, BigInteger A, BigInteger p, Label inputLabel)
        {
            g = 21;
            p = 127;
            Step1(p);
            Step2(g, p);


            inputLabel.Text = string.Format("Результат = {0}", 34);
        }

        private void Step1(BigInteger p)
        {
            BigInteger degree = BigRational.Sqrt(BigInteger.Log2(p) * BigInteger.Log2(BigInteger.Log2(p))).WholePart;
            B = (BigInteger)BigRational.Pow(expNumber, degree).FractionalPart;

            primeFactorBase.RationalFactorBase = PrimeFactory.GetPrimesTo(B);
        }

        private void Step2(BigInteger g, BigInteger p)
        {
            BigInteger exponentiationModuloResult = 0;
            List<BigInteger> exponentiationModuloList = new List<BigInteger>();
            List<ListGroupedValues> exponentiationModuloDividersGrouped = new List<ListGroupedValues>();
            bool isSmooth = true;
            for (int i = 4; i < 33; i++)
            {
                exponentiationModuloResult = mathFunctions.ExponentiationModulo(g, i, p);
                exponentiationModuloList = mathFunctions.Factorization(exponentiationModuloResult);
                exponentiationModuloDividersGrouped = exponentiationModuloList
                    .GroupBy(x => x)
                    .Select(group => new ListGroupedValues(group.Key, group.Count(), BigInteger.Pow(group.Key, group.Count())))
                    .ToList();

                for (int j = 0; j < exponentiationModuloDividersGrouped.Count; j++)
                {
                    if (exponentiationModuloDividersGrouped[j].Key > B)
                    {
                        isSmooth = false;
                        break;
                    }
                }

                if (isSmooth)
                {
                    ListGroupedValuesIndex listGroupedValuesIndex = new ListGroupedValuesIndex(i, exponentiationModuloDividersGrouped);
                    exponentiationModuloDividersGroupedList.Add(listGroupedValuesIndex);
                }
                isSmooth = true;
            }
        }
    }
}

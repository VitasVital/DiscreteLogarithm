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
        List<BigInteger> log_g_NUM;
        List<List<BigInteger>> SLAU;
        List<List<BigInteger>> log_g_NUM_result;
        BigInteger[,] slauArray;
        BigInteger g;
        BigInteger p;
        BigInteger A;
        public Adleman()
        {
            mathFunctions = new MathFunctions();
            expNumber = new BigRational(2, 718, 1000); // 2.718
            primeFactorBase = new FactorBase();
            B = 0;
            exponentiationModuloDividersGroupedList = new List<ListGroupedValuesIndex>();
            log_g_NUM = new List<BigInteger>();
            log_g_NUM_result = new List<List<BigInteger>>();
            SLAU = new List<List<BigInteger>>();
            slauArray = new BigInteger[3, 3];
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

        public void CalculateAdleman(BigInteger input_g, BigInteger input_A, BigInteger input_p, Label inputLabel)
        {
            g = 21;
            p = 127;
            Step1();
            Step2();
            Step3();


            inputLabel.Text = string.Format("Результат = {0}", 34);
        }

        private void Step1()
        {
            BigInteger degree = BigRational.Sqrt(BigInteger.Log2(p) * BigInteger.Log2(BigInteger.Log2(p))).WholePart;
            B = (BigInteger)BigRational.Pow(expNumber, degree).FractionalPart;

            primeFactorBase.RationalFactorBase = PrimeFactory.GetPrimesTo(B);
        }

        private void Step2()
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

        private void Step3()
        {
            CreateSLAU();

            CalculateSLAU();
        }

        private void CreateSLAU()
        {
            for (int i = 0; i < exponentiationModuloDividersGroupedList.Count; i++)
            {
                for (int j = 0; j < exponentiationModuloDividersGroupedList[i].listGroupedValues.Count; j++)
                {
                    log_g_NUM.Add(exponentiationModuloDividersGroupedList[i].listGroupedValues[j].Key);
                }
            }
            log_g_NUM.Sort();
            log_g_NUM = log_g_NUM.Distinct().ToList();

            // создание СЛАУ
            int rowSLAUindex;
            for (int i = 0; i < exponentiationModuloDividersGroupedList.Count; i++)
            {
                List<BigInteger> rowSLAU = new List<BigInteger>();
                for (int j = 0; j < log_g_NUM.Count; j++)
                {
                    rowSLAU.Add(0);
                }
                rowSLAU.Add(exponentiationModuloDividersGroupedList[i].index);

                for (int j = 0; j < exponentiationModuloDividersGroupedList[i].listGroupedValues.Count; j++)
                {
                    rowSLAUindex = log_g_NUM.IndexOf(exponentiationModuloDividersGroupedList[i].listGroupedValues[j].Key);
                    rowSLAU[rowSLAUindex] = exponentiationModuloDividersGroupedList[i].listGroupedValues[j].degree_number;
                }

                SLAU.Add(rowSLAU);
            }

            for (int i = 0; i < log_g_NUM.Count; i++)
            {
                Console.Write(string.Format("{0} ", log_g_NUM[i]));
            }
            Console.WriteLine();

            for (int i = 0; i < SLAU.Count; i++)
            {
                for (int j = 0; j < SLAU[0].Count; j++)
                {
                    Console.Write(string.Format("{0} ", SLAU[i][j]));
                }
                Console.WriteLine();
            }
        }

        private void CalculateSLAU()
        {
            for (int i = 0; i < SLAU.Count - 1; i++)
            {
                if (NonZeroValuesCount(SLAU[i]) > 2)
                {
                    continue;
                }
                for (int j = i + 1; j < SLAU.Count; j++)
                {
                    if (NonZeroValuesCount(SLAU[j]) > 2)
                    {
                        continue;
                    }

                    if (SlauMatrixCreated(SLAU[i], SLAU[j]))
                    {
                        CalculateCreatedSlauMatrix();
                    }
                }
            }
        }

        private int NonZeroValuesCount(List<BigInteger> slauRow)
        {
            int nonZeroValuesCount = 0;
            for (int i = 0; i < slauRow.Count - 1; i++)
            {
                if (slauRow[i] != 0)
                {
                    nonZeroValuesCount++;
                }
            }
            return nonZeroValuesCount;
        }

        private bool SlauMatrixCreated(List<BigInteger> slauRow_i, List<BigInteger> slauRow_j)
        {
            int slauArrayIndex_i_0 = -1;
            int slauArrayIndex_i_1 = -1;
            int slauArrayIndex_j_0 = -1;
            int slauArrayIndex_j_1 = -1;
            int slauRow_i_NonZeroValuesCount = NonZeroValuesCount(slauRow_i);
            int slauRow_j_NonZeroValuesCount = NonZeroValuesCount(slauRow_j);

            for (int q = 0; q < slauRow_i.Count - 1; q++)
            {
                if (slauRow_i[q] > 0)
                {
                    if (slauArrayIndex_i_0 == -1)
                    {
                        slauArrayIndex_i_0 = q;
                    }
                    else
                    {
                        slauArrayIndex_i_1 = q;
                    }
                }

                if (slauRow_j[q] > 0)
                {
                    if (slauArrayIndex_j_0 == -1)
                    {
                        slauArrayIndex_j_0 = q;
                    }
                    else
                    {
                        slauArrayIndex_j_1 = q;
                    }
                }
            }

            bool result = false;
            if (slauArrayIndex_i_0 == slauArrayIndex_j_0 
                && slauArrayIndex_i_1 == slauArrayIndex_j_1 
                && slauArrayIndex_i_0 != -1 && slauArrayIndex_i_1 != -1)
            {
                result = true;
            }
            else if (slauArrayIndex_i_0 == slauArrayIndex_j_0
                && slauRow_i_NonZeroValuesCount == 2 
                && slauRow_j_NonZeroValuesCount == 1)
            {
                slauArrayIndex_j_1 = slauArrayIndex_i_1;
                result = true;
            }
            else if (slauArrayIndex_i_1 == slauArrayIndex_j_1
                && slauRow_i_NonZeroValuesCount == 2
                && slauRow_j_NonZeroValuesCount == 1)
            {
                slauArrayIndex_j_0 = slauArrayIndex_i_0;
                result = true;
            }
            else if (slauArrayIndex_i_0 == slauArrayIndex_j_0
                && slauRow_i_NonZeroValuesCount == 1
                && slauRow_j_NonZeroValuesCount == 2)
            {
                slauArrayIndex_i_1 = slauArrayIndex_j_1;
                result = true;
            }
            else if (slauArrayIndex_i_1 == slauArrayIndex_j_1
                && slauRow_i_NonZeroValuesCount == 1
                && slauRow_j_NonZeroValuesCount == 2)
            {
                slauArrayIndex_i_0 = slauArrayIndex_j_0;
                result = true;
            }

            if (result)
            {
                slauArray[0, 0] = slauArrayIndex_i_0;
                slauArray[0, 1] = slauArrayIndex_i_1;

                slauArray[1, 0] = slauRow_i[slauArrayIndex_i_0];
                slauArray[1, 1] = slauRow_i[slauArrayIndex_i_1];
                slauArray[1, 2] = slauRow_i[slauRow_i.Count - 1];

                slauArray[2, 0] = slauRow_j[slauArrayIndex_j_0];
                slauArray[2, 1] = slauRow_j[slauArrayIndex_j_1];
                slauArray[2, 2] = slauRow_j[slauRow_j.Count - 1];

                BigInteger swapNumber;
                if (slauRow_j_NonZeroValuesCount == 1 && slauArray[2, 1] == 0 
                    || slauRow_i_NonZeroValuesCount == 1 && slauArray[1, 0] == 0)
                {
                    for (int  i = 0; i < 3; i++)
                    {
                        swapNumber = slauArray[1, i];
                        slauArray[1, i] = slauArray[2, i];
                        slauArray[2, i] = swapNumber;
                    }
                }

                PrintSlauArray();
            }

            return result;
        }

        private bool CalculateCreatedSlauMatrix()
        {
            BigInteger invertibleNumberModulo;

            BigInteger[] multipliersModulo_x_y;

            BigInteger p_1 = p - 1;

            if (slauArray[1, 1] != 0)
            {
                multipliersModulo_x_y = mathFunctions.FindMultipliersModulo_x_y(slauArray[1, 1], slauArray[2, 1], p_1, 0);

                if (multipliersModulo_x_y[0] == 0 && multipliersModulo_x_y[1] == 0)
                {
                    return false;
                }

                slauArray[1, 0] = mathFunctions.ExponentiationModulo(slauArray[1, 0] * multipliersModulo_x_y[0] + slauArray[2, 0] * multipliersModulo_x_y[1], 1, p_1);
                slauArray[1, 1] = 0;
                slauArray[1, 2] = mathFunctions.ExponentiationModulo(slauArray[1, 2] * multipliersModulo_x_y[0] + slauArray[2, 2] * multipliersModulo_x_y[1], 1, p_1);
            }

            if (slauArray[1, 0] != 1)
            {
                invertibleNumberModulo = mathFunctions.FindInvertibleNumberModulo(slauArray[1, 0], p_1);

                if (invertibleNumberModulo == -1)
                {
                    return false;
                }

                slauArray[1, 0] = 1;
                slauArray[1, 2] = mathFunctions.ExponentiationModulo(slauArray[1, 2] * invertibleNumberModulo, 1, p_1);
            }

            if (slauArray[2, 0] != 0)
            {
                multipliersModulo_x_y = mathFunctions.FindMultipliersModulo_x_y(slauArray[1, 0], slauArray[2, 0], p_1, 0);

                if (multipliersModulo_x_y[0] == 0 && multipliersModulo_x_y[1] == 0)
                {
                    return false;
                }

                slauArray[2, 0] = 0;
                slauArray[2, 1] = mathFunctions.ExponentiationModulo(slauArray[1, 1] * multipliersModulo_x_y[0] + slauArray[2, 1] * multipliersModulo_x_y[1], 1, p_1);
                slauArray[2, 2] = mathFunctions.ExponentiationModulo(slauArray[1, 2] * multipliersModulo_x_y[0] + slauArray[2, 2] * multipliersModulo_x_y[1], 1, p_1);
            }

            if (slauArray[2, 1] != 0)
            {
                invertibleNumberModulo = mathFunctions.FindInvertibleNumberModulo(slauArray[2, 1], p_1);

                if (invertibleNumberModulo == -1)
                {
                    return false;
                }

                slauArray[2, 1] = 1;
                slauArray[2, 2] = mathFunctions.ExponentiationModulo(slauArray[2, 2] * invertibleNumberModulo, 1, p_1);
            }

            PrintSlauArray("Преобразованная СЛАУ");

            return true;
        }

        private void PrintSlauArray(string inputText = "")
        {
            Console.WriteLine(inputText);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(string.Format("{0} ", slauArray[i, j]));
                }
                Console.WriteLine();
            }
        }
    }
}

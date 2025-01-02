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
    public class COS
    {
        MathFunctions mathFunctions;
        BigRational expNumber;
        FactorBase primeFactorBase { get; set; }
        BigInteger B;
        List<ListGroupedValuesIndex> exponentiationModuloDividersGroupedList;
        List<BigInteger> log_g_NUM;
        List<List<BigInteger>> SLAU;
        List<Log_g_NUM_result> log_g_NUM_result;
        List<BigInteger[,]> slauArrayResults;
        BigInteger g;
        BigInteger p;
        BigInteger A;
        bool numbersSwaped;
        public COS()
        {
            mathFunctions = new MathFunctions();
            expNumber = new BigRational(2, 718, 1000); // 2.718
            primeFactorBase = new FactorBase();
            B = 0;
            exponentiationModuloDividersGroupedList = new List<ListGroupedValuesIndex>();
            log_g_NUM = new List<BigInteger>();
            log_g_NUM_result = new List<Log_g_NUM_result>();
            SLAU = new List<List<BigInteger>>();
            slauArrayResults = new List<BigInteger[,]>();
            numbersSwaped = false;
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

        public void CalculateCOS(BigInteger input_g, BigInteger input_A, BigInteger input_p, Label inputLabel)
        {
            //g = 21;
            //p = 127;
            //A = 34;

            g = input_g;
            p = input_p;
            A = input_A;
            Step1();
            Step2();
            Step3();
            BigInteger result = Step4();


            inputLabel.Text = string.Format("Результат = {0}", result);
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
            for (int i = 4; i < B; i++)
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
            PrintSLAU();
        }

        private BigInteger Step4()
        {
            List<BigInteger> exponentiationModuloList = new List<BigInteger>();
            List<ListGroupedValues> exponentiationModuloDividersGrouped = new List<ListGroupedValues>();
            BigInteger x;
            bool isContains;
            for (int i = 2; i < 100; i++)
            {
                x = 0;
                exponentiationModuloList = mathFunctions.Factorization(mathFunctions.ExponentiationModulo(A * BigInteger.Pow(g, i), 1, p));
                exponentiationModuloDividersGrouped = exponentiationModuloList
                    .GroupBy(x => x)
                    .Select(group => new ListGroupedValues(group.Key, group.Count(), BigInteger.Pow(group.Key, group.Count())))
                    .ToList();

                isContains = false;
                foreach (var exponentiationModuloDivider in exponentiationModuloDividersGrouped)
                {
                    foreach (var log_g_NUM_element in log_g_NUM_result)
                    {
                        if (exponentiationModuloDivider.Key == log_g_NUM[(int)log_g_NUM_element.num])
                        {
                            isContains = true;
                            x += exponentiationModuloDivider.degree_number * log_g_NUM_element.result;
                            break;
                        }
                    }
                    if (isContains == false)
                    {
                        break;
                    }
                }
                if (isContains == true)
                {
                    x -= i;
                    if (x != 0 && mathFunctions.ExponentiationModulo(g, x, p) == A)
                    {
                        return x;
                    }
                }
            }
            return 0;
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

            PrintSLAU();
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

                    BigInteger[,] slauArray = new BigInteger[3, 3];
                    if (SlauMatrixCreated(slauArray, SLAU[i], SLAU[j]))
                    {
                        if (CalculateCreatedSlauMatrix(slauArray, i, j))
                        {
                            slauArrayResults.Add(slauArray);
                        }
                        numbersSwaped = false;
                    }
                }
            }

            CreateLog_g_NUM_result();
        }

        private void CreateLog_g_NUM_result()
        {
            bool isСontainsList_0_0 = false;
            bool isСontainsList_0_1 = false;
            for (int i = 0; i < slauArrayResults.Count; i++)
            {
                for (int j = 0; j < log_g_NUM_result.Count; j++)
                {
                    if (log_g_NUM_result[j].num == slauArrayResults[i][0, 0] && log_g_NUM_result[j].result == slauArrayResults[i][1, 2])
                    {
                        isСontainsList_0_0 = true;
                    }
                    if (log_g_NUM_result[j].num == slauArrayResults[i][0, 1] && log_g_NUM_result[j].result == slauArrayResults[i][2, 2])
                    {
                        isСontainsList_0_1 = true;
                    }
                }
                if (isСontainsList_0_0 == false)
                {
                    log_g_NUM_result.Add(new Log_g_NUM_result(slauArrayResults[i][0, 0], slauArrayResults[i][1, 2]));
                }
                if (isСontainsList_0_1 == false)
                {
                    log_g_NUM_result.Add(new Log_g_NUM_result(slauArrayResults[i][0, 1], slauArrayResults[i][2, 2]));
                }
                isСontainsList_0_0 = false;
                isСontainsList_0_1 = false;
            }
            log_g_NUM_result = log_g_NUM_result.OrderBy(x => x.num).ToList();
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

        private bool SlauMatrixCreated(BigInteger[,] slauArray, List<BigInteger> slauRow_i, List<BigInteger> slauRow_j)
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
                    for (int i = 0; i < 3; i++)
                    {
                        swapNumber = slauArray[1, i];
                        slauArray[1, i] = slauArray[2, i];
                        slauArray[2, i] = swapNumber;
                    }
                    numbersSwaped = true;
                }

                PrintSlauArray(slauArray);
            }

            return result;
        }

        private bool CalculateCreatedSlauMatrix(BigInteger[,] slauArray, int i, int j)
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

            if (numbersSwaped)
            {
                int swapNumber;
                swapNumber = i;
                i = j;
                j = swapNumber;
            }

            SLAU[i][(int)slauArray[0, 0]] = slauArray[1, 0];
            SLAU[i][(int)slauArray[0, 1]] = slauArray[1, 1];
            SLAU[i][SLAU[i].Count - 1] = slauArray[1, 2];

            SLAU[j][(int)slauArray[0, 0]] = slauArray[2, 0];
            SLAU[j][(int)slauArray[0, 1]] = slauArray[2, 1];
            SLAU[j][SLAU[j].Count - 1] = slauArray[2, 2];

            PrintSlauArray(slauArray, "Преобразованная СЛАУ");

            return true;
        }

        private void PrintSlauArray(BigInteger[,] slauArray, string inputText = "")
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

        private void PrintSLAU()
        {
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
    }
}

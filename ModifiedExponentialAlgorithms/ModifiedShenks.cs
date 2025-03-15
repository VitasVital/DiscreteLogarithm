using DiscreteLogarithm.MathFunctionsForCalculation;
using System.Numerics;
using Label = System.Windows.Forms.Label;

namespace DiscreteLogarithm.ModifiedExponentialAlgorithms
{
    // Алгоритм https://ilovecalc.com/calcs/maths/baby-step-giant-step/1382/
    public class ModifiedShenks
    {
        MathFunctions mathFunctions;
        public ModifiedShenks()
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

        async public Task CalculateModifiedShenksAsync(BigInteger g, BigInteger A, BigInteger p, Label inputLabel)
        {
            BigInteger m, k;
            Step1(p, out m, out k);

            List<BigInteger> g_km_degree = new List<BigInteger>();
            List<BigInteger> Ag_m_degree = new List<BigInteger>();

            await Step2Async(g_km_degree, Ag_m_degree, g, A, p, m, k);

            BigInteger i, j;
            (i, j) = await Step3Async(g_km_degree, Ag_m_degree);
            BigInteger result = i * m - j;

            inputLabel.Text = "Результат: \na = " + result.ToString();
        }

        private void Step1(BigInteger p, out BigInteger m, out BigInteger k)
        {
            m = k = mathFunctions.Sqrt(p) + 1;
        }

        async private Task Step2Async(List<BigInteger> g_km_degree, List<BigInteger> Ag_m_degree, BigInteger g, BigInteger A, BigInteger p, BigInteger m, BigInteger k)
        {
            Task task_Step2_g_km_degree = Step2_g_km_degreeAsync(g_km_degree, g, A, p, m, k);
            Task task_Step2_Ag_m_degree = Step2_Ag_m_degreeAsync(Ag_m_degree, g, A, p, m, k);
            await Task.WhenAll(task_Step2_g_km_degree, task_Step2_Ag_m_degree);
        }

        async private Task Step2_g_km_degreeAsync(List<BigInteger> g_km_degree, BigInteger g, BigInteger A, BigInteger p, BigInteger m, BigInteger k)
        {
            for (BigInteger k_i = 1; k_i <= k; k_i++)
            {
                g_km_degree.Add(mathFunctions.ExponentiationModulo(g, k_i * m, p));
            }
        }

        async private Task Step2_Ag_m_degreeAsync(List<BigInteger> Ag_m_degree, BigInteger g, BigInteger A, BigInteger p, BigInteger m, BigInteger k)
        {
            for (int m_i = 0; m_i <= m - 1; m_i++)
            {
                Ag_m_degree.Add(mathFunctions.ExponentiationModulo(A * BigInteger.Pow(g, m_i), 1, p));
            }
        }

        async private Task<(BigInteger, BigInteger)> Step3Async(List<BigInteger> g_km_degree, List<BigInteger> Ag_m_degree)
        {
            var task_Step3Async_0 = Step3Async_0(g_km_degree, Ag_m_degree);
            var task_Step3Async_1 = Step3Async_1(g_km_degree, Ag_m_degree);
            (BigInteger, BigInteger) result = await Task.WhenAny(task_Step3Async_0, task_Step3Async_1).Result;
            return result; // распараллелил алгоритм на 3 шаге, чтобы с двух сторон был поиск результата
        }

        async private Task<(BigInteger, BigInteger)> Step3Async_0(List<BigInteger> g_km_degree, List<BigInteger> Ag_m_degree)
        {
            for (int i = 0; i < g_km_degree.Count / 2; i += 1)
            {
                for (int j = 0; j < Ag_m_degree.Count; j++)
                {
                    if (g_km_degree[i] == Ag_m_degree[j])
                    {
                        return (i + 1, j);
                    }
                }
            }
            await Task.Delay(100000);
            return (0, 0);
        }

        async private Task<(BigInteger, BigInteger)> Step3Async_1(List<BigInteger> g_km_degree, List<BigInteger> Ag_m_degree)
        {
            for (int i = g_km_degree.Count; i > g_km_degree.Count / 2; i--)
            {
                for (int j = 0; j < Ag_m_degree.Count; j++)
                {
                    if (g_km_degree[i] == Ag_m_degree[j])
                    {
                        return (i + 1, j);
                    }
                }
            }
            await Task.Delay(100000);
            return (0, 0);
        }
    }
}

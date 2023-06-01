using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Label = System.Windows.Forms.Label;

namespace DiscreteLogarithm.MathFunctionsForCalculation
{
    public class MathFunctions
    {
        public MathFunctions()
        {
        }
        //public BigInteger ExponentiationModulo(BigInteger a, BigInteger b, BigInteger n)
        //{
        //    if (b == 0) return 1;
        //    BigInteger z = ExponentiationModulo(a, b / 2, n);
        //    return (b % 2 == 0) ? (z * z) % n : (a * z * z) % n;
        //}

        public void CheckingTheInputValues(
            string input_a,
            string input_x,
            string input_p,
            Label inputLabel,
            ref bool theValuesAreCorrect,
            out BigInteger a,
            out BigInteger x,
            out BigInteger p)
        {
            inputLabel.Text = "";
            if (!BigInteger.TryParse(input_a, out a))
            {
                theValuesAreCorrect = false;
                inputLabel.Text = "Ошибка ввода числа а";
            };
            if (!BigInteger.TryParse(input_x, out x))
            {
                theValuesAreCorrect = false;
                inputLabel.Text += "\nОшибка ввода числа x";
            };
            if (!BigInteger.TryParse(input_p, out p))
            {
                theValuesAreCorrect = false;
                inputLabel.Text += "\nОшибка ввода числа p";
            };
        }

        private string ConvertToBinaty(BigInteger number)
        {
            string binary_letter = "";
            if (number == 0)
            {
                return "0";
            }
            while (number >= 1)
            {
                binary_letter += Convert.ToString(number % 2);
                number /= 2;
            }
            return binary_letter;
        }

        public BigInteger ExponentiationModulo(BigInteger a, BigInteger alpha, BigInteger n)
        {
            //перевод alpha в двоичный вид
            string binary_alpha = ConvertToBinaty(alpha);

            List<BigInteger> number = new List<BigInteger>() { a };
            for (int i = 1; i < binary_alpha.Length; i++)
            {
                number.Add((number[i - 1] * number[i - 1]) % n);
            }

            BigInteger result = 1;
            for (int i = 0; i < binary_alpha.Length; i++)
            {
                if (binary_alpha[i] == '1')
                {
                    result *= number[i];
                }
            }
            result %= n;

            return result;
        }

        public void ExponentiationModuloWin(BigInteger a, BigInteger alpha, BigInteger n, Label inputLabel)
        {
            BigInteger result = ExponentiationModulo(a, alpha, n);

            inputLabel.Text = string.Format("Результат = {0}", result);
        }

        public BigInteger Generate_a()
        {
            // число a 16 бит
            int byteCount = 16 / 8;
            var rng = new RNGCryptoServiceProvider();
            byte[] bytes;
            BigInteger a;
            while (true)
            {
                bytes = new byte[byteCount];
                rng.GetBytes(bytes);
                a = new BigInteger(bytes);
                if (a > 1)
                {
                    return a;
                }
            }
        }

        public BigInteger Generate_p()
        {
            // число p 64 бит
            int byteCount = 64 / 8;
            var rng = new RNGCryptoServiceProvider();
            byte[] bytes;
            BigInteger p;
            while (true)
            {
                bytes = new byte[byteCount];
                rng.GetBytes(bytes);
                p = new BigInteger(bytes);

                if (p < 3)
                {
                    continue;
                }

                if (TestMillerRabin(p) == "Вероятно простое")
                {
                    return p;
                }
            }
        }

        public BigInteger Generate_g(BigInteger p)
        {
            // число g 64 бит
            int byteCount = 64 / 8;
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] bytes;
            BigInteger g;

            BigInteger fi_p = p - 1;
            IList<BigInteger> p_dividers = new List<BigInteger>();
            for (BigInteger i = 2; i < fi_p; i++)
            {
                if (fi_p % i == 0)
                {
                    p_dividers.Add(i);
                }
                if (p_dividers.Count > 5)
                {
                    break;
                }
            }

            bool true_p;
            while (true)
            {
                true_p = true;
                bytes = new byte[byteCount];
                do
                {
                    rng.GetBytes(bytes);
                    g = new BigInteger(bytes);
                }
                while (g < 2 || g >= p - 2);

                for (int i = 0; i < p_dividers.Count; i++)
                {
                    if (ExponentiationModulo(g, fi_p, p_dividers[i]) != 1)
                    {
                        true_p = false;
                        break;
                    };
                }
                if(true_p)
                {
                    return g;
                }
            }
        }

        private string TestMillerRabin(BigInteger n)
        {
            if (n == 2 || n == 3)
            {
                return "Вероятно простое";
            }
            if (n % 2 == 0)
            {
                return "Составное";
            }

            double k = BigInteger.Log(n);
            // представим n − 1 в виде (2^s)·t, где t нечётно, это можно сделать последовательным делением n - 1 на 2
            BigInteger t = n - 1;
            int s = 0;
            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }

            for (int i = 0; i < k; i++)
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                byte[] _a = new byte[n.ToByteArray().LongLength];

                BigInteger a;

                do
                {
                    rng.GetBytes(_a);
                    a = new BigInteger(_a);
                }
                while (a < 2 || a >= n - 2);

                BigInteger x = ExponentiationModulo(a, t, n);

                if (x == 1 || x == n - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = ExponentiationModulo(x, 2, n);

                    if (x == 1)
                        return "Составное";

                    if (x == n - 1)
                        break;
                }

                if (x != n - 1)
                    return "Составное";
            }

            return "Вероятно простое";
        }
    }
}

using DiscreteLogarithm.ExponentialAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Label = System.Windows.Forms.Label;

namespace DiscreteLogarithm.MathFunctionsForCalculation
{
    public class MathFunctions
    {
        RoPollard roPollard;
        public MathFunctions()
        {
            roPollard = new RoPollard();
        }

        public void CheckingTheInputValues(
            string input_g,
            string input_a,
            string input_p,
            Label inputLabel,
            ref bool theValuesAreCorrect,
            out BigInteger g,
            out BigInteger a,
            out BigInteger p)
        {
            inputLabel.Text = "";
            if (!BigInteger.TryParse(input_g, out g))
            {
                theValuesAreCorrect = false;
                inputLabel.Text = "Ошибка ввода числа g";
            };
            if (!BigInteger.TryParse(input_a, out a))
            {
                theValuesAreCorrect = false;
                inputLabel.Text += "\nОшибка ввода числа a";
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

        public BigInteger ExponentiationModulo(BigInteger g, BigInteger a, BigInteger n)
        {
            //перевод alpha в двоичный вид
            string binary_a = ConvertToBinaty(a);

            List<BigInteger> number = new List<BigInteger>() { g };
            for (int i = 1; i < binary_a.Length; i++)
            {
                number.Add((number[i - 1] * number[i - 1]) % n);
            }

            BigInteger result = 1;
            for (int i = 0; i < binary_a.Length; i++)
            {
                if (binary_a[i] == '1')
                {
                    result *= number[i];
                }
            }
            result %= n;

            return result;
        }

        public void ExponentiationModuloWin(BigInteger g, BigInteger a, BigInteger n, Label inputLabel)
        {
            BigInteger result = ExponentiationModulo(g, a, n);

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

        public List<BigInteger> Factorization(BigInteger fi_p)
        {
            List<BigInteger> p_dividers = new List<BigInteger>();
            BigInteger p_factorized_new;
            BigInteger q_factorized_new;
            BigInteger fi_p_help = fi_p;

            while (true)
            {
                if (TestMillerRabin(fi_p_help) == "Вероятно простое")
                {
                    p_dividers.Add(fi_p_help);
                    return p_dividers;
                }

                p_factorized_new = roPollard.ro_Pollard(fi_p_help);
                q_factorized_new = fi_p_help / p_factorized_new;

                string p_factorized_new_miller_rabin = p_factorized_new > 1 ? TestMillerRabin(p_factorized_new) : "Меньше 2";
                string q_factorized_new_miller_rabin = q_factorized_new > 1 ? TestMillerRabin(q_factorized_new) : "Меньше 2";

                if (p_factorized_new != 1 && q_factorized_new != 1 && p_factorized_new_miller_rabin == "Вероятно простое")
                {
                    p_dividers.Add(p_factorized_new);
                    fi_p /= p_factorized_new;
                    fi_p_help = fi_p;
                } 
                else if (p_factorized_new != 1 && q_factorized_new != 1 && q_factorized_new_miller_rabin == "Вероятно простое")
                {
                    p_dividers.Add(q_factorized_new);
                    fi_p /= q_factorized_new;
                    fi_p_help = fi_p;
                } 
                else if (p_factorized_new > q_factorized_new)
                {
                    fi_p_help /= p_factorized_new;
                }
                else
                {
                    fi_p_help /= q_factorized_new;
                }

            }
        }

        public void FindAllDivisors(List<BigInteger> p_dividers, BigInteger p_factorized)
        {
            if (TestMillerRabin(p_factorized) == "Вероятно простое")
            {
                return;
            }

            BigInteger p_factorized_new;
            BigInteger q_factorized_new;

            while (true)
            {
                p_factorized_new = roPollard.ro_Pollard(p_factorized);
                q_factorized_new = p_factorized / p_factorized_new;
                if (p_factorized_new != 1 && q_factorized_new != 1)
                {
                    break;
                }
            }

            p_dividers.Add(p_factorized_new);
            p_dividers.Add(q_factorized_new);

            if (p_factorized_new == 1 || q_factorized_new == 1)
            {
                return;
            }

            FindAllDivisors(p_dividers, p_factorized_new);
            FindAllDivisors(p_dividers, q_factorized_new);
        }

        public BigInteger Generate_g(BigInteger p)
        {
            BigInteger fi_p = p - 1;
            List<BigInteger> p_dividers = new List<BigInteger>();
            FindAllDivisors(p_dividers, fi_p);
            p_dividers = p_dividers.Distinct().ToList();

            // число g 64 бит
            int byteCount = 64 / 8;
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] bytes;
            BigInteger g;

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

                //for (int i = 0; i < p_dividers.Count; i++) // идёт долгий поиск. Ниразу пока не нашел
                //{
                //    if (ExponentiationModulo(g, fi_p, p_dividers[i]) != 1)
                //    {
                //        true_p = false;
                //        break;
                //    };
                //}

                if (TestMillerRabin(g) != "Вероятно простое" || ExponentiationModulo(g, fi_p, p_dividers[0]) != 1) // проверка на первом делителе из списка
                {
                    true_p = false;
                };

                if (true_p)
                {
                    return g;
                }
            }
        }

        private string TestMillerRabin(BigInteger n)
        {
            if (n == 1 || n == 2 || n == 3)
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

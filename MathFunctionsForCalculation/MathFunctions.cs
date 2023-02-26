using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteLogarithm.MathFunctionsForCalculation
{
    public class MathFunctions
    {
        public MathFunctions()
        {
        }

        public string ConvertToBinaty(BigInteger number)
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

        // возведение в степень по модулю
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
            return result %= n;
        }
    }
}

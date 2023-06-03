using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using Label = System.Windows.Forms.Label;
using System.Security.Cryptography;

namespace DiscreteLogarithm.ExponentialAlgorithms
{
    public class RoPollard
    {
        public void CheckingTheInputValues(
            string input_N,
            Label inputLabel,
            ref bool theValuesAreCorrect,
            out BigInteger a)
        {
            inputLabel.Text = "";
            if (!BigInteger.TryParse(input_N, out a) || a < 5)
            {
                theValuesAreCorrect = false;
                inputLabel.Text = "Ошибка ввода числа N";
            };
        }

        public BigInteger ro_Pollard(BigInteger n)
        {
            Random random = new Random();
            byte[] data = new byte[n.ToByteArray().Length];
            random.NextBytes(data);
            BigInteger x = new BigInteger(data);
            x = x < 0 ? -x - 2 : x - 2;

            BigInteger y = 1;
            BigInteger i = 0;
            BigInteger stage = 2;

            while (BigInteger.GreatestCommonDivisor(n, BigInteger.Abs(x - y)) == 1)
            {
                if (i == stage)
                {
                    y = x;
                    stage = stage * 2;
                }
                x = (x * x - 1) % n;
                i = i + 1;
            }
            return BigInteger.GreatestCommonDivisor(n, BigInteger.Abs(x - y));
        }

        public void CalculateRoPollard(BigInteger N, Label inputLabel)
        {
            BigInteger p = ro_Pollard(N);
            BigInteger q = N / p;

            inputLabel.Text = string.Format("Результат P: {0} \nРезультат Q: {1}", p, q);
        }
    }
}

using System.Numerics;
using Label = System.Windows.Forms.Label;

namespace DiscreteLogarithm.ModifiedExponentialAlgorithms
{
    public class ModifiedRoPollard
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
                inputLabel.Text = "Ошибка N";
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

            inputLabel.Text = string.Format("P = {0} \nQ = {1}", p, q);
        }
    }
}

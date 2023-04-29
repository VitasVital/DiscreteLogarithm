using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        public BigInteger ExponentiationModulo(BigInteger a, BigInteger b, BigInteger n)
        {
            if (b == 0) return 1;
            BigInteger z = ExponentiationModulo(a, b / 2, n);
            return (b % 2 == 0) ? (z * z) % n : (a * z * z) % n;
        }
    }
}

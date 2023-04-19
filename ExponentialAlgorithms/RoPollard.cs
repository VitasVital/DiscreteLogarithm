﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Label = System.Windows.Forms.Label;

namespace DiscreteLogarithm.ExponentialAlgorithms
{
    public class RoPollard
    {
        BigInteger x;
        BigInteger y;

        public void CheckingTheInputValues(
            string input_N,
            string input_e,
            Label inputLabel,
            ref bool theValuesAreCorrect,
            out BigInteger a,
            out BigInteger b)
        {
            inputLabel.Text = "";
            if (!BigInteger.TryParse(input_N, out a))
            {
                theValuesAreCorrect = false;
                inputLabel.Text = "Ошибка ввода числа N";
            };
            if (!BigInteger.TryParse(input_e, out b))
            {
                theValuesAreCorrect = false;
                inputLabel.Text += "\nОшибка ввода числа e";
            };
        }

        private BigInteger ro_Pollard(BigInteger n)
        {
            BigInteger x = 4;
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

        public void NOD(BigInteger a, BigInteger b)
        {
            List<BigInteger[]> list = new List<BigInteger[]>();
            list.Add(new BigInteger[6]);
            list[0][0] = a;
            list[0][1] = b;

            list[0][2] = a % b;
            list[0][3] = a / b;

            int ind = 0;
            while (list[ind][2] != 0)
            {
                ind += 1;

                list.Add(new BigInteger[6]);

                list[ind][0] = list[ind - 1][1];
                list[ind][1] = list[ind - 1][2];

                list[ind][2] = list[ind][0] % list[ind][1];
                list[ind][3] = list[ind][0] / list[ind][1];
            }

            list[ind][4] = 0;
            list[ind][5] = 1;
            while (ind != 0)
            {
                ind -= 1;

                list[ind][4] = list[ind + 1][5];
                list[ind][5] = list[ind + 1][4] - list[ind + 1][5] * list[ind][3];
            }

            x = list[ind][4];
            y = list[ind][5];
        }

        public void CalculateRoPollard(BigInteger N, BigInteger e, Label inputLabel)
        {
            BigInteger p = ro_Pollard(N);
            BigInteger q = N / p;
            BigInteger fi_n = (p - 1) * (q - 1);

            NOD(fi_n, e);

            BigInteger d = y;

            while (d < 0)
            {
                d = fi_n + d;
            }

            inputLabel.Text = "Результат P = " + p.ToString() + "\nРезультат Q = " + q.ToString();
        }
    }
}

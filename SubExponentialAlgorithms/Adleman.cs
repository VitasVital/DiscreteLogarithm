using DiscreteLogarithm.ExponentialAlgorithms;
using DiscreteLogarithm.MathFunctionsForCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Label = System.Windows.Forms.Label;

namespace DiscreteLogarithm.SubExponentialAlgorithms
{
    public class Adleman
    {
        MathFunctions mathFunctions;
        public Adleman()
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

        public void CalculateAdleman(BigInteger g, BigInteger A, BigInteger p, Label inputLabel)
        {
            inputLabel.Text = string.Format("Результат = {0}", 34);
        }
    }
}

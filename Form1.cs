using DiscreteLogarithm.ExponentialAlgorithms;
using DiscreteLogarithm.MathFunctionsForCalculation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscreteLogarithm
{
    public partial class Form1 : Form
    {
        Shenks shenks;
        RoPollard roPollard;
        MathFunctions mathFunctions;
        public Form1()
        {
            InitializeComponent();
            shenks = new Shenks();
            mathFunctions = new MathFunctions();
            roPollard = new RoPollard();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BigInteger a;
            BigInteger b;
            BigInteger p;
            bool theValuesAreCorrect = true;

            shenks.CheckingTheInputValues(textBox1.Text, textBox2.Text, textBox3.Text, label5, ref theValuesAreCorrect, out a, out b, out p);
            if (!theValuesAreCorrect)
            {
                return;
            }

            shenks.CalculateShenks(a, b, p, label5);
        }

        private void button2_Click(object sender, EventArgs _e)
        {
            BigInteger N;
            BigInteger e;
            bool theValuesAreCorrect = true;

            roPollard.CheckingTheInputValues(textBox6.Text, textBox5.Text, label11, ref theValuesAreCorrect, out N, out e);
            if (!theValuesAreCorrect)
            {
                return;
            }

            roPollard.CalculateRoPollard(N, e, label11);
        }
    }
}

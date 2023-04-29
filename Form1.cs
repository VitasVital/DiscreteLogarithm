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
        PoligHellman poligHellman;
        public Form1()
        {
            InitializeComponent();
            shenks = new Shenks();
            mathFunctions = new MathFunctions();
            roPollard = new RoPollard();
            poligHellman = new PoligHellman();
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
            bool theValuesAreCorrect = true;

            roPollard.CheckingTheInputValues(textBox6.Text, label11, ref theValuesAreCorrect, out N);
            if (!theValuesAreCorrect)
            {
                return;
            }

            roPollard.CalculateRoPollard(N, label11);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BigInteger a;
            BigInteger b;
            BigInteger p;
            bool theValuesAreCorrect = true;

            poligHellman.CheckingTheInputValues(textBox8.Text, textBox7.Text, textBox4.Text, label8, ref theValuesAreCorrect, out a, out b, out p);
            if (!theValuesAreCorrect)
            {
                return;
            }

            poligHellman.CalculatePoligHellman(a, b, p, label8);
        }
    }
}

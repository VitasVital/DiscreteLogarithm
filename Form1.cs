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
            BigInteger g;
            BigInteger A;
            BigInteger p;
            bool theValuesAreCorrect = true;

            shenks.CheckingTheInputValues(textBox1.Text, textBox2.Text, textBox3.Text, label5, ref theValuesAreCorrect, out g, out A, out p);
            if (!theValuesAreCorrect)
            {
                return;
            }

            shenks.CalculateShenks(g, A, p, label5);
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

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            BigInteger g;
            BigInteger a;
            BigInteger p;
            bool theValuesAreCorrect = true;

            mathFunctions.CheckingTheInputValues(textBox11.Text, textBox10.Text, textBox9.Text, label8, ref theValuesAreCorrect, out g, out a, out p);
            if (!theValuesAreCorrect)
            {
                return;
            }

            mathFunctions.ExponentiationModuloWin(g, a, p, label20);
        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            BigInteger a = mathFunctions.Generate_a();
            BigInteger p = mathFunctions.Generate_p();
            BigInteger g = mathFunctions.Generate_g(p);
            BigInteger A = mathFunctions.ExponentiationModulo(g, a, p);
            textBox13.Text = a.ToString();
            textBox12.Text = p.ToString();
            textBox14.Text = g.ToString();
            textBox15.Text = A.ToString();
        }
    }
}

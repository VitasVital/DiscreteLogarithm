using DiscreteLogarithm.ExponentialAlgorithms;
using DiscreteLogarithm.MathFunctionsForCalculation;
using DiscreteLogarithm.SubExponentialAlgorithms;
using System.Numerics;

namespace DiscreteLogarithmCore
{
    public partial class Form1 : Form
    {
        Shenks shenks;
        RoPollard roPollard;
        MathFunctions mathFunctions;
        PoligHellman poligHellman;
        Adleman adleman;
        GNFS gNFS;
        COS cos;
        public Form1()
        {
            InitializeComponent();
            
            mathFunctions = new MathFunctions();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            BigInteger N;
            bool theValuesAreCorrect = true;

            gNFS = new GNFS();
            gNFS.CheckingTheInputValues(textBox1.Text, label28, ref theValuesAreCorrect, out N);
            if (!theValuesAreCorrect)
            {
                return;
            }

            gNFS.CalculateGNFS(N, label28);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BigInteger g;
            BigInteger A;
            BigInteger p;
            bool theValuesAreCorrect = true;

            shenks = new Shenks();
            shenks.CheckingTheInputValues(textBox2.Text, textBox3.Text, textBox4.Text, label15, ref theValuesAreCorrect, out g, out A, out p);
            if (!theValuesAreCorrect)
            {
                return;
            }

            shenks.CalculateShenks(g, A, p, label15);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BigInteger a;
            BigInteger b;
            BigInteger p;
            bool theValuesAreCorrect = true;

            poligHellman = new PoligHellman();
            poligHellman.CheckingTheInputValues(textBox7.Text, textBox6.Text, textBox5.Text, label16, ref theValuesAreCorrect, out a, out b, out p);
            if (!theValuesAreCorrect)
            {
                return;
            }

            poligHellman.CalculatePoligHellman(a, b, p, label16);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            BigInteger N;
            bool theValuesAreCorrect = true;

            roPollard = new RoPollard();
            roPollard.CheckingTheInputValues(textBox14.Text, label29, ref theValuesAreCorrect, out N);
            if (!theValuesAreCorrect)
            {
                return;
            }

            roPollard.CalculateRoPollard(N, label29);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            BigInteger a = mathFunctions.Generate_a();
            List<BigInteger> p_g = mathFunctions.Generate_p_g();
            BigInteger A = mathFunctions.ExponentiationModulo(p_g[1], a, p_g[0]);
            textBox16.Text = a.ToString();
            textBox15.Text = p_g[0].ToString();
            textBox17.Text = p_g[1].ToString();
            textBox18.Text = A.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            BigInteger g;
            BigInteger a;
            BigInteger p;
            bool theValuesAreCorrect = true;

            mathFunctions.CheckingTheInputValues(textBox21.Text, textBox20.Text, textBox19.Text, label35, ref theValuesAreCorrect, out g, out a, out p);
            if (!theValuesAreCorrect)
            {
                return;
            }

            mathFunctions.ExponentiationModuloWin(g, a, p, label35);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BigInteger a;
            BigInteger b;
            BigInteger p;
            bool theValuesAreCorrect = true;

            //adleman = new Adleman();
            //adleman.CheckingTheInputValues(textBox10.Text, textBox9.Text, textBox8.Text, label20, ref theValuesAreCorrect, out a, out b, out p);
            //if (!theValuesAreCorrect)
            //{
            //    return;
            //}

            //adleman.CalculateAdleman(a, b, p, label20);

            shenks = new Shenks();
            shenks.CheckingTheInputValues(textBox10.Text, textBox9.Text, textBox8.Text, label20, ref theValuesAreCorrect, out a, out b, out p);
            if (!theValuesAreCorrect)
            {
                return;
            }

            shenks.CalculateShenks(a, b, p, label20);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BigInteger a;
            BigInteger b;
            BigInteger p;
            bool theValuesAreCorrect = true;

            //cos = new COS();
            //cos.CheckingTheInputValues(textBox13.Text, textBox12.Text, textBox11.Text, label24, ref theValuesAreCorrect, out a, out b, out p);
            //if (!theValuesAreCorrect)
            //{
            //    return;
            //}

            //cos.CalculateCOS(a, b, p, label24);

            shenks = new Shenks();
            shenks.CheckingTheInputValues(textBox13.Text, textBox12.Text, textBox11.Text, label24, ref theValuesAreCorrect, out a, out b, out p);
            if (!theValuesAreCorrect)
            {
                return;
            }

            shenks.CalculateShenks(a, b, p, label24);
        }
    }
}

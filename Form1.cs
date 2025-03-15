using DiscreteLogarithm.ExponentialAlgorithms;
using DiscreteLogarithm.MathFunctionsForCalculation;
using DiscreteLogarithm.ModifiedExponentialAlgorithms;
using DiscreteLogarithm.ModifiedSubExponentialAlgorithms;
using DiscreteLogarithm.SubExponentialAlgorithms;
using System.Diagnostics;
using System.Numerics;

namespace DiscreteLogarithmCore
{
    public partial class Form1 : Form
    {
        MathFunctions mathFunctions;
        Shenks shenks;
        ModifiedShenks modifiedShenks;
        PoligHellman poligHellman;
        ModifiedPoligHellman modifiedPoligHellman;
        RoPollard roPollard;
        ModifiedRoPollard modifiedRoPollard;
        Adleman adleman;
        ModifiedAdleman modifiedAdleman;
        COS cos;
        ModifiedCOS modifiedCOS;
        GNFS gNFS;
        ModifiedGNFS modifiedGNFS;
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

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
			long before = GC.GetTotalMemory(false);
			try
            {
                gNFS.CalculateGNFS(N, label28);
            }
            catch (Exception ex)
            {
                label28.Text = "Error";
			}
			long after = GC.GetTotalMemory(false);
			int consumedInBytes = (int)(after - before);
			consumedInBytes = consumedInBytes > 0 ? consumedInBytes : -consumedInBytes;
			stopwatch.Stop();
            label28.Text += $"\nt = {stopwatch.ElapsedMilliseconds} мс\n{consumedInBytes} байт";
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

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
			long before = GC.GetTotalMemory(false);
			shenks.CalculateShenks(g, A, p, label15);
			long after = GC.GetTotalMemory(false);
			int consumedInBytes = (int)(after - before);
			consumedInBytes = consumedInBytes > 0 ? consumedInBytes : -consumedInBytes;
			stopwatch.Stop();
            label15.Text += $"\nt = {stopwatch.ElapsedMilliseconds} мс\n{consumedInBytes} байт";
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

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
			long before = GC.GetTotalMemory(false);
			poligHellman.CalculatePoligHellman(a, b, p, label16);
			long after = GC.GetTotalMemory(false);
			int consumedInBytes = (int)(after - before);
			consumedInBytes = consumedInBytes > 0 ? consumedInBytes : -consumedInBytes;
			stopwatch.Stop();
            label16.Text += $"\nt = {stopwatch.ElapsedMilliseconds} мс\n{consumedInBytes} байт";
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

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
			long before = GC.GetTotalMemory(false);
			roPollard.CalculateRoPollard(N, label29);
			long after = GC.GetTotalMemory(false);
			int consumedInBytes = (int)(after - before);
			consumedInBytes = consumedInBytes > 0 ? consumedInBytes : -consumedInBytes;
			stopwatch.Stop();
            label29.Text += $"\nt = {stopwatch.ElapsedMilliseconds} мс\n{consumedInBytes} байт";
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

            adleman = new Adleman();
            adleman.CheckingTheInputValues(textBox10.Text, textBox9.Text, textBox8.Text, label20, ref theValuesAreCorrect, out a, out b, out p);
            if (!theValuesAreCorrect)
            {
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
			long before = GC.GetTotalMemory(false);
			adleman.CalculateAdleman(a, b, p, label20);
			long after = GC.GetTotalMemory(false);
			int consumedInBytes = (int)(after - before);
            consumedInBytes = consumedInBytes > 0 ? consumedInBytes : -consumedInBytes;
			stopwatch.Stop();
            label20.Text += $"\nt = {stopwatch.ElapsedMilliseconds} мс\n{consumedInBytes} байт";

            //shenks = new Shenks();
            //shenks.CheckingTheInputValues(textBox10.Text, textBox9.Text, textBox8.Text, label20, ref theValuesAreCorrect, out a, out b, out p);
            //if (!theValuesAreCorrect)
            //{
            //    return;
            //}

            //shenks.CalculateShenks(a, b, p, label20);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BigInteger a;
            BigInteger b;
            BigInteger p;
            bool theValuesAreCorrect = true;

            cos = new COS();
            cos.CheckingTheInputValues(textBox13.Text, textBox12.Text, textBox11.Text, label24, ref theValuesAreCorrect, out a, out b, out p);
            if (!theValuesAreCorrect)
            {
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
			long before = GC.GetTotalMemory(false);
			cos.CalculateCOS(a, b, p, label24);
			long after = GC.GetTotalMemory(false);
			int consumedInBytes = (int)(after - before);
			consumedInBytes = consumedInBytes > 0 ? consumedInBytes : -consumedInBytes;
			stopwatch.Stop();
            label24.Text += $"\nt = {stopwatch.ElapsedMilliseconds} мс\n{consumedInBytes} байт";

            //shenks = new Shenks();
            //shenks.CheckingTheInputValues(textBox13.Text, textBox12.Text, textBox11.Text, label24, ref theValuesAreCorrect, out a, out b, out p);
            //if (!theValuesAreCorrect)
            //{
            //    return;
            //}

            //shenks.CalculateShenks(a, b, p, label24);
        }

        async private void button9_Click(object sender, EventArgs e)
        {
            BigInteger a;
            BigInteger b;
            BigInteger p;
            bool theValuesAreCorrect = true;

            modifiedShenks = new ModifiedShenks();
            modifiedShenks.CheckingTheInputValues(textBox2.Text, textBox3.Text, textBox4.Text, label40, ref theValuesAreCorrect, out a, out b, out p);
            if (!theValuesAreCorrect)
            {
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
			long before = GC.GetTotalMemory(false);
			await modifiedShenks.CalculateModifiedShenksAsync(a, b, p, label40);
			long after = GC.GetTotalMemory(false);
			int consumedInBytes = (int)(after - before);
			consumedInBytes = consumedInBytes > 0 ? consumedInBytes : -consumedInBytes;
			stopwatch.Stop();
            label40.Text += $"\nt = {stopwatch.ElapsedMilliseconds} мс\n{consumedInBytes} байт";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            BigInteger a;
            BigInteger b;
            BigInteger p;
            bool theValuesAreCorrect = true;

            modifiedPoligHellman = new ModifiedPoligHellman();
            modifiedPoligHellman.CheckingTheInputValues(textBox7.Text, textBox6.Text, textBox5.Text, label41, ref theValuesAreCorrect, out a, out b, out p);
            if (!theValuesAreCorrect)
            {
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
			long before = GC.GetTotalMemory(false);
			modifiedPoligHellman.CalculatePoligHellman(a, b, p, label41);
			long after = GC.GetTotalMemory(false);
			int consumedInBytes = (int)(after - before);
			consumedInBytes = consumedInBytes > 0 ? consumedInBytes : -consumedInBytes;
			stopwatch.Stop();
            label41.Text += $"\nt = {stopwatch.ElapsedMilliseconds} мс\n{consumedInBytes} байт";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            BigInteger N;
            bool theValuesAreCorrect = true;

            modifiedRoPollard = new ModifiedRoPollard();
            modifiedRoPollard.CheckingTheInputValues(textBox14.Text, label42, ref theValuesAreCorrect, out N);
            if (!theValuesAreCorrect)
            {
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
			long before = GC.GetTotalMemory(false);
			modifiedRoPollard.CalculateRoPollard(N, label42);
			long after = GC.GetTotalMemory(false);
			int consumedInBytes = (int)(after - before);
			consumedInBytes = consumedInBytes > 0 ? consumedInBytes : -consumedInBytes;
			stopwatch.Stop();
            label42.Text += $"\nt = {stopwatch.ElapsedMilliseconds} мс\n{consumedInBytes} байт";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            BigInteger a;
            BigInteger b;
            BigInteger p;
            bool theValuesAreCorrect = true;

            modifiedAdleman = new ModifiedAdleman();
            modifiedAdleman.CheckingTheInputValues(textBox10.Text, textBox9.Text, textBox8.Text, label43, ref theValuesAreCorrect, out a, out b, out p);
            if (!theValuesAreCorrect)
            {
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
			long before = GC.GetTotalMemory(false);
			modifiedAdleman.CalculateAdleman(a, b, p, label43);
			long after = GC.GetTotalMemory(false);
			int consumedInBytes = (int)(after - before);
			consumedInBytes = consumedInBytes > 0 ? consumedInBytes : -consumedInBytes;
			stopwatch.Stop();
            label43.Text += $"\nt = {stopwatch.ElapsedMilliseconds} мс\n{consumedInBytes} байт";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            BigInteger a;
            BigInteger b;
            BigInteger p;
            bool theValuesAreCorrect = true;

            modifiedCOS = new ModifiedCOS();
            modifiedCOS.CheckingTheInputValues(textBox13.Text, textBox12.Text, textBox11.Text, label44, ref theValuesAreCorrect, out a, out b, out p);
            if (!theValuesAreCorrect)
            {
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
			long before = GC.GetTotalMemory(false);
			modifiedCOS.CalculateCOS(a, b, p, label44);
			long after = GC.GetTotalMemory(false);
			int consumedInBytes = (int)(after - before);
			consumedInBytes = consumedInBytes > 0 ? consumedInBytes : -consumedInBytes;
			stopwatch.Stop();
            label44.Text += $"\nt = {stopwatch.ElapsedMilliseconds} мс\n{consumedInBytes} байт";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            BigInteger N;
            bool theValuesAreCorrect = true;

            modifiedGNFS = new ModifiedGNFS();
            modifiedGNFS.CheckingTheInputValues(textBox1.Text, label45, ref theValuesAreCorrect, out N);
            if (!theValuesAreCorrect)
            {
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
			long before = GC.GetTotalMemory(false);
			try
            {
                modifiedGNFS.CalculateGNFS(N, label45);
            }
            catch (Exception ex)
            {
                label45.Text = "Error";
			}
			long after = GC.GetTotalMemory(false);
			int consumedInBytes = (int)(after - before);
			consumedInBytes = consumedInBytes > 0 ? consumedInBytes : -consumedInBytes;
			stopwatch.Stop();
            label45.Text += $"\nt = {stopwatch.ElapsedMilliseconds} мс\n{consumedInBytes} байт";
        }
    }
}

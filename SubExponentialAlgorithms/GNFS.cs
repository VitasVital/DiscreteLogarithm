﻿using DiscreteLogarithm.ExponentialAlgorithms;
using DiscreteLogarithm.MathFunctionsForCalculation;
using ExtendedArithmetic;
using ExtendedNumerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Label = System.Windows.Forms.Label;

namespace DiscreteLogarithm.SubExponentialAlgorithms
{
    public class GNFS
    {
        MathFunctions mathFunctions;
        public int PolynomialDegree { get; internal set; }
        public BigInteger PolynomialBase { get; private set; }
        public Polynomial CurrentPolynomial { get; internal set; }

        public PolyRelationsSieveProgress CurrentRelationsProgress { get; set; }
        public FactorBase PrimeFactorBase { get; set; }
        public List<Polynomial> PolynomialCollection { get; set; }
        public FactorPairCollection QuadraticFactorPairCollection { get; set; }
        int relationQuantity { get; set; }
        int relationValueRange { get; set; }

        /// <summary>
		/// Array of (p, m % p)
		/// </summary>
		public FactorPairCollection RationalFactorPairCollection { get; set; }

        /// <summary>
		/// Array of (p, r) where ƒ(r) % p == 0
		/// </summary>
		public FactorPairCollection AlgebraicFactorPairCollection { get; set; }

        public GNFS()
        {
            //mathFunctions = new MathFunctions();
            //PolynomialBase = 31;
            //PolynomialDegree = 3;
            //relationQuantity = 65;
            //relationValueRange = 1000;
            //PrimeFactorBase = new FactorBase();
            //PolynomialCollection = new List<Polynomial>();
            //RationalFactorPairCollection = new FactorPairCollection();
            //AlgebraicFactorPairCollection = new FactorPairCollection();
            //QuadraticFactorPairCollection = new FactorPairCollection();
            //CaclulatePrimeFactorBaseBounds(50);
            //SetPrimeFactorBases();
            //NewFactorPairCollections();
            //CurrentRelationsProgress = new PolyRelationsSieveProgress(this, relationQuantity, relationValueRange);
            
        }

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

        public void CalculateGNFS(BigInteger N, Label inputLabel)
        {
            BigInteger p = 124;
            BigInteger q = N / p;
            Step1(N);
            Step2();
            Step3();

            inputLabel.Text = string.Format("Результат P: {0} \nРезультат Q: {1}", p, q);
        }

        private void Step1(BigInteger N) // Create Polynomial, Factor Bases && Roots
        {
            mathFunctions = new MathFunctions();
            PolynomialBase = 31;
            PolynomialDegree = 3;
            relationQuantity = 65;
            relationValueRange = 1000;
            PrimeFactorBase = new FactorBase();
            PolynomialCollection = new List<Polynomial>();
            RationalFactorPairCollection = new FactorPairCollection();
            AlgebraicFactorPairCollection = new FactorPairCollection();
            QuadraticFactorPairCollection = new FactorPairCollection();
            CurrentPolynomial = new Polynomial(N, PolynomialBase, PolynomialDegree);
            CaclulatePrimeFactorBaseBounds(50);
            SetPrimeFactorBases();
            NewFactorPairCollections();
            CurrentRelationsProgress = new PolyRelationsSieveProgress(this, relationQuantity, relationValueRange);

        }

        private void Step2() // Sieve Relations
        {
            this.CurrentRelationsProgress.GenerateRelations();
        }

        private void Step3() // Matrix
        {
            MatrixSolve.GaussianSolve(this);
        }

        public void CaclulatePrimeFactorBaseBounds(BigInteger bound)
        {
            PrimeFactorBase = new FactorBase();

            PrimeFactorBase.RationalFactorBaseMax = bound;
            PrimeFactorBase.AlgebraicFactorBaseMax = (PrimeFactorBase.RationalFactorBaseMax) * 3;

            PrimeFactorBase.QuadraticBaseCount = CalculateQuadraticBaseSize(PolynomialDegree);

            PrimeFactorBase.QuadraticFactorBaseMin = PrimeFactorBase.AlgebraicFactorBaseMax + 20;
            PrimeFactorBase.QuadraticFactorBaseMax = PrimeFactory.GetApproximateValueFromIndex((UInt64)(PrimeFactorBase.QuadraticFactorBaseMin + PrimeFactorBase.QuadraticBaseCount));

        }
        private static int CalculateQuadraticBaseSize(int polyDegree)
        {
            int result = -1;

            if (polyDegree <= 3)
            {
                result = 10;
            }
            else if (polyDegree == 4)
            {
                result = 20;
            }
            else if (polyDegree == 5 || polyDegree == 6)
            {
                result = 40;
            }
            else if (polyDegree == 7)
            {
                result = 80;
            }
            else if (polyDegree >= 8)
            {
                result = 100;
            }

            return result;
        }

        public void SetPrimeFactorBases()
        {
            PrimeFactory.IncreaseMaxValue(PrimeFactorBase.QuadraticFactorBaseMax);

            PrimeFactorBase.RationalFactorBase = PrimeFactory.GetPrimesTo(PrimeFactorBase.RationalFactorBaseMax);
            
            PrimeFactorBase.AlgebraicFactorBase = PrimeFactory.GetPrimesTo(PrimeFactorBase.AlgebraicFactorBaseMax);
            
            PrimeFactorBase.QuadraticFactorBase = PrimeFactory.GetPrimesFrom(PrimeFactorBase.QuadraticFactorBaseMin).Take(PrimeFactorBase.QuadraticBaseCount);
            
        }

        private void NewFactorPairCollections()
        {
            if (!RationalFactorPairCollection.Any())
            {
                RationalFactorPairCollection = FactorPairCollection.Factory.BuildRationalFactorPairCollection(this);
            }

            if (!AlgebraicFactorPairCollection.Any())
            {
                AlgebraicFactorPairCollection = FactorPairCollection.Factory.BuildAlgebraicFactorPairCollection(this);
            }

            if (!QuadraticFactorPairCollection.Any())
            {
                QuadraticFactorPairCollection = FactorPairCollection.Factory.BuildQuadraticFactorPairCollection(this);
            }
        }

    }

    public class PolyRelationsSieveProgress
    {
        MathFunctions mathFunctions;
        public BigInteger A { get; private set; }
        public BigInteger B { get; private set; }
        public int SmoothRelations_TargetQuantity { get; private set; }

        public BigInteger ValueRange { get; private set; }

        public List<List<Relation>> FreeRelations { get { return Relations.FreeRelations; } }
        public List<Relation> SmoothRelations { get { return Relations.SmoothRelations; } }
        public List<Relation> RoughRelations { get { return Relations.RoughRelations; } }

        public RelationContainer Relations { get; set; }

        public BigInteger MaxB { get; set; }
        public int SmoothRelationsCounter { get; set; }
        public int FreeRelationsCounter { get; set; }

        public int SmoothRelationsRequiredForMatrixStep
        {
            get
            {
                return PrimeFactory.GetIndexFromValue(_gnfs.PrimeFactorBase.RationalFactorBaseMax)
                      + PrimeFactory.GetIndexFromValue(_gnfs.PrimeFactorBase.AlgebraicFactorBaseMax)
                      + _gnfs.QuadraticFactorPairCollection.Count + 3;
            }
        }

        internal GNFS _gnfs;


        public PolyRelationsSieveProgress()
        {
            Relations = new RelationContainer();
            mathFunctions = new MathFunctions();
        }

        public PolyRelationsSieveProgress(GNFS gnfs, BigInteger valueRange)
            : this(gnfs, -1, valueRange)
        {
        }

        public PolyRelationsSieveProgress(GNFS gnfs, int smoothRelationsTargetQuantity, BigInteger valueRange)
        {
            _gnfs = gnfs;
            Relations = new RelationContainer();

            A = 0;
            B = 3;
            ValueRange = valueRange;

            if (smoothRelationsTargetQuantity == -1)
            {
                SmoothRelations_TargetQuantity = SmoothRelationsRequiredForMatrixStep;
            }
            else
            {
                SmoothRelations_TargetQuantity = Math.Max(smoothRelationsTargetQuantity, SmoothRelationsRequiredForMatrixStep);
            }

            if (MaxB == 0)
            {
                MaxB = (uint)gnfs.PrimeFactorBase.AlgebraicFactorBaseMax;
            }
        }

        public void GenerateRelations()
        {

            SmoothRelations_TargetQuantity = Math.Max(SmoothRelations_TargetQuantity, SmoothRelationsRequiredForMatrixStep); ;


            if (A >= ValueRange)
            {
                ValueRange += 200;
            }

            ValueRange = (ValueRange % 2 == 0) ? ValueRange + 1 : ValueRange;
            A = (A % 2 == 0) ? A + 1 : A;

            BigInteger startA = A;

            while (B >= MaxB)
            {
                MaxB += 100;
            }

            while (SmoothRelationsCounter < SmoothRelations_TargetQuantity)
            {

                if (B > MaxB)
                {
                    break;
                }

                foreach (BigInteger a in SieveRange.GetSieveRangeContinuation(A, ValueRange))
                {

                    A = a;
                    if (BigInteger.GreatestCommonDivisor(A, B) == 1)
                    {
                        Relation rel = new Relation(_gnfs, A, B);

                        rel.Sieve(_gnfs.CurrentRelationsProgress);

                        bool smooth = rel.IsSmooth;
                        if (smooth)
                        {
                            Serialization.Save.Relations.Smooth.Append(_gnfs, rel);

                            _gnfs.CurrentRelationsProgress.Relations.SmoothRelations.Add(rel);

                        }
                        else
                        {

                        }
                    }
                }

                B += 1;
                A = startA;

            }
        }

        public void IncreaseTargetQuantity()
        {
            IncreaseTargetQuantity(SmoothRelations_TargetQuantity - SmoothRelationsRequiredForMatrixStep);
        }

        public void IncreaseTargetQuantity(int ammount)
        {
            SmoothRelations_TargetQuantity += ammount;
        }

        public void PurgePrimeRoughRelations()
        {
            List<Relation> roughRelations = Relations.RoughRelations.ToList();

            IEnumerable<Relation> toRemoveAlg = roughRelations
                .Where(r => r.AlgebraicQuotient != 1 && mathFunctions.TestMillerRabin(r.AlgebraicQuotient) == "Вероятно простое");
            
            roughRelations = roughRelations.Except(toRemoveAlg).ToList();

            Relations.RoughRelations = roughRelations;

            IEnumerable<Relation> toRemoveRational = roughRelations
                .Where(r => r.RationalQuotient != 1 && mathFunctions.TestMillerRabin(r.AlgebraicQuotient) == "Вероятно простое");

            roughRelations = roughRelations.Except(toRemoveRational).ToList();

            Relations.RoughRelations = roughRelations;
        }

        public void AddFreeRelationSolution(List<Relation> freeRelationSolution)
        {
            Relations.FreeRelations.Add(freeRelationSolution);
        }

        public string FormatRelations(IEnumerable<Relation> relations)
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine($"Smooth relations:");
            result.AppendLine("\t_______________________________________________");
            result.AppendLine($"\t|   A   |  B | ALGEBRAIC_NORM | RATIONAL_NORM | \t\tRelations count: {Relations.SmoothRelations.Count} Target quantity: {SmoothRelations_TargetQuantity}");
            result.AppendLine("\t```````````````````````````````````````````````");
            foreach (Relation rel in relations.OrderByDescending(rel => rel.A * rel.B))
            {
                result.AppendLine(rel.ToString());
                result.AppendLine("Algebraic " + rel.AlgebraicFactorization.FormatStringAsFactorization());
                result.AppendLine("Rational  " + rel.RationalFactorization.FormatStringAsFactorization());
                result.AppendLine();
            }
            result.AppendLine();

            return result.ToString();
        }
    }

    public static class SieveRange
    {
        public static IEnumerable<BigInteger> GetSieveRange(BigInteger maximumRange)
        {
            return GetSieveRangeContinuation(1, maximumRange);
        }

        public static IEnumerable<BigInteger> GetSieveRangeContinuation(BigInteger currentValue, BigInteger maximumRange)
        {
            BigInteger max = maximumRange;
            BigInteger counter = BigInteger.Abs(currentValue);
            bool flipFlop = !(currentValue.Sign == -1);

            while (counter <= max)
            {
                if (flipFlop)
                {
                    yield return counter;
                    flipFlop = false;
                }
                else if (!flipFlop)
                {
                    yield return -counter;
                    counter++;
                    flipFlop = true;
                }
            }
        }
    }

    public class Relation : IEquatable<Relation>, IEqualityComparer<Relation>
    {
        public BigInteger A { get; protected set; }

        /// <summary>
        /// Root of f(x) in algebraic field
        /// </summary>
        public BigInteger B { get; protected set; }

        /// <summary> ƒ(b) ≡ 0 (mod a); Calculated as: ƒ(-a/b) * -b^deg </summary>
        public BigInteger AlgebraicNorm { get; protected set; }
        /// <summary>  a + bm </summary>
        public BigInteger RationalNorm { get; protected set; }

        internal BigInteger AlgebraicQuotient;
        internal BigInteger RationalQuotient;

        public CountDictionary AlgebraicFactorization { get; private set; }
        public CountDictionary RationalFactorization { get; private set; }

        public bool IsSmooth { get { return (IsRationalQuotientSmooth && IsAlgebraicQuotientSmooth); } }

        public bool IsRationalQuotientSmooth { get { return (RationalQuotient == 1 || RationalQuotient == 0); } }

        public bool IsAlgebraicQuotientSmooth { get { return (AlgebraicQuotient == 1 || AlgebraicQuotient == 0); } }

        public bool IsPersisted { get; set; }

        public Relation()
        {
            IsPersisted = false;
            RationalFactorization = new CountDictionary();
            AlgebraicFactorization = new CountDictionary();
        }

        public Relation(GNFS gnfs, BigInteger a, BigInteger b)
            : this()
        {
            A = a;
            B = b;

            AlgebraicNorm = Normal.Algebraic(A, B, gnfs.CurrentPolynomial); // b^deg * f( a/b )
            RationalNorm = Normal.Rational(A, B, gnfs.PolynomialBase); // a + bm

            AlgebraicQuotient = BigInteger.Abs(AlgebraicNorm);
            RationalQuotient = BigInteger.Abs(RationalNorm);

            if (AlgebraicNorm.Sign == -1)
            {
                AlgebraicFactorization.Add(BigInteger.MinusOne);
            }

            if (RationalNorm.Sign == -1)
            {
                RationalFactorization.Add(BigInteger.MinusOne);
            }
        }

        public BigInteger Apply(BigInteger x)
        {
            return BigInteger.Add(A, BigInteger.Multiply(B, x));
        }

        public void Sieve(PolyRelationsSieveProgress relationsSieve)
        {
            Sieve(relationsSieve._gnfs.PrimeFactorBase.RationalFactorBase, ref RationalQuotient, RationalFactorization);

            if (IsRationalQuotientSmooth) // No sense wasting time on factoring the AlgebraicQuotient if the relation is ultimately going to be rejected anyways.
            {
                Sieve(relationsSieve._gnfs.PrimeFactorBase.AlgebraicFactorBase, ref AlgebraicQuotient, AlgebraicFactorization);
            }
        }

        private static void Sieve(IEnumerable<BigInteger> primeFactors, ref BigInteger quotientValue, CountDictionary dictionary)
        {
            if (quotientValue.Sign == -1 || primeFactors.Any(f => f.Sign == -1))
            {
                throw new Exception("There shouldn't be any negative values either in the quotient or the factors");
            }

            foreach (BigInteger factor in primeFactors)
            {
                if (quotientValue == 1)
                {
                    return;
                }

                if ((factor * factor) > quotientValue)
                {
                    if (primeFactors.Contains(quotientValue))
                    {
                        dictionary.Add(quotientValue);
                        quotientValue = 1;
                    }
                    return;
                }

                while (quotientValue != 1 && quotientValue % factor == 0)
                {
                    dictionary.Add(factor);
                    quotientValue = BigInteger.Divide(quotientValue, factor);
                }
            }
        }

        #region IEquatable / IEqualityComparer

        public override bool Equals(object obj)
        {
            Relation other = obj as Relation;

            if (other == null)
            {
                return false;
            }
            else
            {
                return this.Equals(other);
            }
        }

        public bool Equals(Relation x, Relation y)
        {
            return x.Equals(y);
        }

        public bool Equals(Relation other)
        {
            return (this.A == other.A && this.B == other.B);
        }

        public int GetHashCode(Relation obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Tuple.Create(this.A, this.B).GetHashCode();
        }

        #endregion

    }

    public class FactorBase
    {
        public FactorBase()
        {
            RationalFactorBase = new List<BigInteger>();
            AlgebraicFactorBase = new List<BigInteger>();
            QuadraticFactorBase = new List<BigInteger>();
        }

        public BigInteger RationalFactorBaseMax { get; internal set; }
        public BigInteger AlgebraicFactorBaseMax { get; internal set; }
        public BigInteger QuadraticFactorBaseMin { get; internal set; }
        public BigInteger QuadraticFactorBaseMax { get; internal set; }
        public int QuadraticBaseCount { get; internal set; }
        public IEnumerable<BigInteger> RationalFactorBase { get; internal set; }
        public IEnumerable<BigInteger> AlgebraicFactorBase { get; internal set; }
        public IEnumerable<BigInteger> QuadraticFactorBase { get; internal set; }
    }

    public class RelationContainer
    {
        public List<Relation> SmoothRelations { get; internal set; }
        public List<Relation> RoughRelations { get; internal set; }
        public List<List<Relation>> FreeRelations { get; internal set; }

        public RelationContainer()
        {
            SmoothRelations = new List<Relation>();
            RoughRelations = new List<Relation>();
            FreeRelations = new List<List<Relation>>();
        }
    }

    public class CountDictionary : SortedDictionary<BigInteger, BigInteger>, ICloneable<Dictionary<BigInteger, BigInteger>>
    {
        public CountDictionary()
            : base(Comparer<BigInteger>.Create(BigInteger.Compare))
        {
        }

        public void Add(BigInteger key)
        {
            this.AddSafe(key, 1);
        }
        private void AddSafe(BigInteger key, BigInteger value)
        {
            if (!ContainsKey(key)) { this.Add(key, value); }
            else { this[key] += value; }
        }

        public void Combine(CountDictionary dictionary)
        {
            foreach (var kvp in dictionary)
            {
                AddSafe(kvp.Key, kvp.Value);
            }
        }

        public Dictionary<BigInteger, BigInteger> ToDictionary()
        {
            return this.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public Dictionary<BigInteger, BigInteger> Clone()
        {
            return this.ToDictionary();
        }

        #region String Formatting

        public override string ToString()
        {
            //Order();

            StringBuilder result = new StringBuilder();
            result.AppendLine("{");
            foreach (KeyValuePair<BigInteger, BigInteger> kvp in this)
            {
                result.Append('\t');
                result.Append(kvp.Key.ToString().PadLeft(5));
                result.Append(":\t");
                result.AppendLine(kvp.Value.ToString().PadLeft(5));
            }
            result.Append("}");

            return result.ToString();
        }

        public string FormatStringAsFactorization()
        {
            //Order();
            StringBuilder result = new StringBuilder();
            result.Append(
                " -> {\t" +
                string.Join(" * ", this.Select(kvp => $"{kvp.Key}^{kvp.Value}")) +
                "\t};"
                );
            return result.ToString();
        }

        #endregion
    }

    public static class Normal
    {
        /// <summary>
        ///  a + bm
        /// </summary>
        /// <param name="polynomialBase">Base m of f(m) = N</param>
        /// <returns></returns>
        public static BigInteger Rational(BigInteger a, BigInteger b, BigInteger polynomialBase)
        {
            return BigInteger.Add(a, BigInteger.Multiply(b, polynomialBase));
        }

        /// <summary>
        /// a - bm
        /// </summary>
        public static BigInteger RationalSubtract(BigInteger a, BigInteger b, BigInteger polynomialBase)
        {
            return BigInteger.Subtract(a, BigInteger.Multiply(b, polynomialBase));
        }

        /// <summary>
        /// ƒ(b) ≡ 0 (mod a)
        /// 
        /// Calculated as:
        /// ƒ(-a/b) * -b^deg
        /// </summary>
        /// <param name="a">Divisor in the equation ƒ(b) ≡ 0 (mod a)</param>
        /// <param name="b">A root of f(x)</param>
        /// <param name="poly">Base m of f(m) = N</param>
        /// <returns></returns>
        public static BigInteger Algebraic(BigInteger a, BigInteger b, Polynomial poly)
        {
            BigRational aD = (BigRational)a;
            BigRational bD = (BigRational)b;
            BigRational ab = BigRational.Negate(aD) / bD;

            BigRational left = PolynomialEvaluate_BigRational(poly, ab);
            BigInteger right = BigInteger.Pow(BigInteger.Negate(b), poly.Degree);

            BigRational product = right * left;

            Fraction fractionalPart = product.FractionalPart;

            BigInteger result = product.WholePart;
            return result;
        }

        private static BigRational PolynomialEvaluate_BigRational(Polynomial polynomial, BigRational indeterminateValue)
        {
            int num = polynomial.Degree;

            BigRational result = (BigRational)polynomial[num];
            while (--num >= 0)
            {
                result *= indeterminateValue;
                result += (BigRational)polynomial[num];
            }

            return result;
        }
    }

    public static class PrimeFactory
    {
        private static MathFunctions mathFunctions;
        private static BigInteger MaxValue = 10;

        private static int primesCount;
        private static BigInteger primesLast;
        private static List<BigInteger> primes = new List<BigInteger>() { 2, 3, 5, 7, 11, 13 };

        static PrimeFactory()
        {
            SetPrimes();
            mathFunctions = new MathFunctions();
        }

        private static void SetPrimes()
        {
            primes = FastPrimeSieve.GetRange(2, (Int32)MaxValue).ToList();
            primesCount = primes.Count;
            primesLast = primes.Last();
        }

        public static IEnumerable<BigInteger> GetPrimeEnumerator(int startIndex = 0, int stopIndex = -1)
        {
            int index = startIndex;
            int maxIndex = stopIndex > 0 ? stopIndex : primesCount - 1;
            while (index < maxIndex)
            {
                yield return primes[index];
                index++;
            }
            yield break;
        }

        public static void IncreaseMaxValue(BigInteger newMaxValue)
        {
            // Increase bound
            BigInteger temp = BigInteger.Max(newMaxValue + 1000, MaxValue + 100000 /*MaxValue*/);
            MaxValue = BigInteger.Min(temp, (Int32.MaxValue - 1));
            SetPrimes();
        }

        public static int GetIndexFromValue(BigInteger value)
        {
            if (value == -1)
            {
                return -1;
            }
            if (primesLast < value)
            {
                IncreaseMaxValue(value);
            }

            BigInteger primeValue = primes.First(p => p >= value);

            int index = primes.IndexOf(primeValue) + 1;
            return index;
        }

        public static BigInteger GetApproximateValueFromIndex(UInt64 n)
        {
            if (n < 6)
            {
                return primes[(int)n];
            }

            double fn = (double)n;
            double flogn = Math.Log(n);
            double flog2n = Math.Log(flogn);

            double upper;

            if (n >= 688383)    /* Dusart 2010 page 2 */
            {
                upper = fn * (flogn + flog2n - 1.0 + ((flog2n - 2.00) / flogn));
            }
            else if (n >= 178974)    /* Dusart 2010 page 7 */
            {
                upper = fn * (flogn + flog2n - 1.0 + ((flog2n - 1.95) / flogn));
            }
            else if (n >= 39017)    /* Dusart 1999 page 14 */
            {
                upper = fn * (flogn + flog2n - 0.9484);
            }
            else                    /* Modified from Robin 1983 for 6-39016 _only_ */
            {
                upper = fn * (flogn + 0.6000 * flog2n);
            }

            if (upper >= (double)UInt64.MaxValue)
            {
                throw new OverflowException($"{upper} > {UInt64.MaxValue}");
            }

            return new BigInteger((UInt64)Math.Ceiling(upper));
        }

        public static IEnumerable<BigInteger> GetPrimesFrom(BigInteger minValue)
        {
            return GetPrimeEnumerator(GetIndexFromValue(minValue));
        }

        public static IEnumerable<BigInteger> GetPrimesTo(BigInteger maxValue)
        {
            if (primesLast < maxValue)
            {
                IncreaseMaxValue(maxValue);
            }
            return GetPrimeEnumerator(0).TakeWhile(p => p < maxValue);
        }

        public static bool IsPrime(BigInteger value)
        {
            return primes.Contains(BigInteger.Abs(value));
        }

        public static BigInteger GetNextPrime(BigInteger fromValue)
        {
            BigInteger result = fromValue + 1;

            if (result.IsEven)
            {
                result += 1;
            }

            while (mathFunctions.TestMillerRabin(result) != "Вероятно простое")
            {
                result += 2;
            }

            return result;
        }
    }

    public class FactorPairCollection : List<FactorPair>
    {
        public FactorPairCollection()
            : base()
        {
        }

        public FactorPairCollection(IEnumerable<FactorPair> collection)
            : base(collection)
        {
        }

        public override string ToString()
        {
            return string.Join("\t", this.Select(factr => factr.ToString()));
        }

        public string ToString(int take)
        {
            return string.Join("\t", this.Take(take).Select(factr => factr.ToString()));
        }

        public static class Factory
        {
            // array of (p, m % p) up to bound
            // quantity = phi(bound)
            public static FactorPairCollection BuildRationalFactorPairCollection(GNFS gnfs)
            {
                IEnumerable<FactorPair> result = gnfs.PrimeFactorBase.RationalFactorBase.Select(p => new FactorPair(p, (gnfs.PolynomialBase % p))).Distinct();
                return new FactorPairCollection(result);
            }

            // array of (p, r) where ƒ(r) % p == 0
            // quantity = 2-3 times RFB.quantity
            public static FactorPairCollection BuildAlgebraicFactorPairCollection(GNFS gnfs)
            {
                return new FactorPairCollection(FindPolynomialRootsInRange(gnfs.CurrentPolynomial, gnfs.PrimeFactorBase.AlgebraicFactorBase, 0, gnfs.PrimeFactorBase.AlgebraicFactorBaseMax, 2000));
            }

            // array of (p, r) where ƒ(r) % p == 0
            // quantity =< 100
            // magnitude p > AFB.Last().p
            public static FactorPairCollection BuildQuadraticFactorPairCollection(GNFS gnfs)
            {
                return new FactorPairCollection(FindPolynomialRootsInRange(gnfs.CurrentPolynomial, gnfs.PrimeFactorBase.QuadraticFactorBase, 2, gnfs.PrimeFactorBase.QuadraticFactorBaseMax, gnfs.PrimeFactorBase.QuadraticBaseCount));
            }
        }

        public static List<FactorPair> FindPolynomialRootsInRange(Polynomial polynomial, IEnumerable<BigInteger> primes, BigInteger rangeFrom, BigInteger rangeTo, int totalFactorPairs)
        {
            List<FactorPair> result = new List<FactorPair>();

            BigInteger r = rangeFrom;
            IEnumerable<BigInteger> modList = primes.AsEnumerable();
            while (r < rangeTo && result.Count < totalFactorPairs)
            {
                // Finds p such that ƒ(r) ≡ 0 (mod p)
                List<BigInteger> roots = GetRootsMod(polynomial, r, modList);
                if (roots.Any())
                {
                    result.AddRange(roots.Select(p => new FactorPair(p, r)));
                }
                r++;
            }

            return result.OrderBy(tup => tup.P).ToList();
        }

        /// <summary>
        /// Given a list of primes, returns primes p such that ƒ(r) ≡ 0 (mod p)
        /// </summary>
        public static List<BigInteger> GetRootsMod(Polynomial polynomial, BigInteger baseM, IEnumerable<BigInteger> modList)
        {
            BigInteger polyResult = polynomial.Evaluate(baseM);
            IEnumerable<BigInteger> result = modList.Where(mod => (polyResult % mod) == 0);
            return result.ToList();
        }
    }

    public struct FactorPair : IEquatable<FactorPair>
    {
        public int P { get; private set; }
        public int R { get; private set; }

        public FactorPair(BigInteger p, BigInteger r)
        {
            P = (int)p;
            R = (int)r;
        }

        public FactorPair(int p, int r)
        {
            P = p;
            R = r;
        }

        public override int GetHashCode()
        {
            return CombineHashCodes(P, R);
        }

        private static int CombineHashCodes(int h1, int h2)
        {
            return (((h1 << 5) + h1) ^ h2);
        }

        public override bool Equals(object obj)
        {
            return (obj is FactorPair && this.Equals((FactorPair)obj));
        }

        public bool Equals(FactorPair other)
        {
            return (this == other);
        }

        public static bool operator !=(FactorPair left, FactorPair right)
        {
            return !(left == right);
        }

        public static bool operator ==(FactorPair left, FactorPair right)
        {
            return (left.P == right.P && left.R == right.R);
        }

        public override string ToString()
        {
            return $"({P},{R})";
        }
    }

    public class FastPrimeSieve : IEnumerable<BigInteger>
    {
        private static readonly uint PageSize; // L1 CPU cache size in bytes
        private static readonly uint BufferBits;
        private static readonly uint BufferBitsNext;

        static FastPrimeSieve()
        {
            uint cacheSize = 393216;
            List<uint> cacheSizes = CPUInfo.GetCacheSizes(CPUInfo.CacheLevel.Level1);
            if (cacheSizes.Any())
            {
                cacheSize = cacheSizes.First() * 1024;
            }

            PageSize = cacheSize; // L1 CPU cache size in bytes
            BufferBits = PageSize * 8; // in bits
            BufferBitsNext = BufferBits * 2;
        }

        public static IEnumerable<BigInteger> GetRange(BigInteger floor, BigInteger ceiling)
        {
            FastPrimeSieve primesPaged = new FastPrimeSieve();
            IEnumerator<BigInteger> enumerator = primesPaged.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current >= floor)
                {
                    break;
                }
            }

            do
            {
                if (enumerator.Current > ceiling)
                {
                    break;
                }
                yield return enumerator.Current;
            }
            while (enumerator.MoveNext());

            yield break;
        }

        public IEnumerator<BigInteger> GetEnumerator()
        {
            return Iterator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        private static IEnumerator<BigInteger> Iterator()
        {
            IEnumerator<BigInteger> basePrimes = null;
            List<uint> basePrimesArray = new List<uint>();
            uint[] cullBuffer = new uint[PageSize / 4]; // 4 byte words

            yield return 2;

            for (var low = (BigInteger)0; ; low += BufferBits)
            {
                for (var bottomItem = 0; ; ++bottomItem)
                {
                    if (bottomItem < 1)
                    {
                        if (bottomItem < 0)
                        {
                            bottomItem = 0;
                            yield return 2;
                        }

                        BigInteger next = 3 + low + low + BufferBitsNext;
                        if (low <= 0)
                        {
                            // cull very first page
                            for (int i = 0, sqr = 9, p = 3; sqr < next; i++, p += 2, sqr = p * p)
                            {
                                if ((cullBuffer[i >> 5] & (1 << (i & 31))) == 0)
                                {
                                    for (int j = (sqr - 3) >> 1; j < BufferBits; j += p)
                                    {
                                        cullBuffer[j >> 5] |= 1u << j;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Cull for the rest of the pages
                            Array.Clear(cullBuffer, 0, cullBuffer.Length);

                            if (basePrimesArray.Count == 0)
                            {
                                // Init second base primes stream
                                basePrimes = Iterator();
                                basePrimes.MoveNext();
                                basePrimes.MoveNext();
                                basePrimesArray.Add((uint)basePrimes.Current); // Add 3 to base primes array
                                basePrimes.MoveNext();
                            }

                            // Make sure basePrimesArray contains enough base primes...
                            for (BigInteger p = basePrimesArray[basePrimesArray.Count - 1], square = p * p; square < next;)
                            {
                                p = basePrimes.Current;
                                basePrimes.MoveNext();
                                square = p * p;
                                basePrimesArray.Add((uint)p);
                            }

                            for (int i = 0, limit = basePrimesArray.Count - 1; i < limit; i++)
                            {
                                var p = (BigInteger)basePrimesArray[i];
                                var start = (p * p - 3) >> 1;

                                // adjust start index based on page lower limit...
                                if (start >= low)
                                {
                                    start -= low;
                                }
                                else
                                {
                                    var r = (low - start) % p;
                                    start = (r != 0) ? p - r : 0;
                                }
                                for (var j = (uint)start; j < BufferBits; j += (uint)p)
                                {
                                    cullBuffer[j >> 5] |= 1u << ((int)j);
                                }
                            }
                        }
                    }

                    while (bottomItem < BufferBits && (cullBuffer[bottomItem >> 5] & (1 << (bottomItem & 31))) != 0)
                    {
                        ++bottomItem;
                    }

                    if (bottomItem < BufferBits)
                    {
                        var result = 3 + (((BigInteger)bottomItem + low) << 1);
                        yield return result;
                    }
                    else break; // outer loop for next page segment...
                }
            }
        }
    }

    public static class CPUInfo
    {
        public static List<uint> GetCacheSizes(CacheLevel level)
        {
            ManagementClass mc = new ManagementClass("Win32_CacheMemory");
            ManagementObjectCollection moc = mc.GetInstances();
            List<uint> cacheSizes = new List<uint>(moc.Count);

            cacheSizes.AddRange(moc
              .Cast<ManagementObject>()
              .Where(p => (ushort)(p.Properties["Level"].Value) == (ushort)level)
              .Select(p => (uint)(p.Properties["MaxCacheSize"].Value)));

            return cacheSizes;
        }

        public enum CacheLevel : ushort
        {
            Level1 = 3,
            Level2 = 4,
            Level3 = 5,
        }
    }

    public static partial class Serialization
    {
        public static class Save
        {
            public static void Object(object obj, string filename)
            {

            }

            public static void All(GNFS gnfs)
            {
                Save.Gnfs(gnfs);

                int counter = 1;
                foreach (Polynomial poly in gnfs.PolynomialCollection)
                {
                    string filename = $"Polynomial.{counter:00}";
                    counter++;
                }

                Save.FactorPair.Rational(gnfs);
                Save.FactorPair.Algebraic(gnfs);
                Save.FactorPair.Quadratic(gnfs);

                Save.Relations.Smooth.Append(gnfs);
                Save.Relations.Rough.Append(gnfs);
                Save.Relations.Free.AllSolutions(gnfs);
            }

            public static void Gnfs(GNFS gnfs)
            {

            }

            public static class FactorPair
            {
                public static void Rational(GNFS gnfs)
                {
                    if (gnfs.RationalFactorPairCollection.Any())
                    {

                    }
                }

                public static void Algebraic(GNFS gnfs)
                {
                    if (gnfs.AlgebraicFactorPairCollection.Any())
                    {

                    }
                }

                public static void Quadratic(GNFS gnfs)
                {
                    if (gnfs.QuadraticFactorPairCollection.Any())
                    {

                    }
                }
            }

            public static class Relations
            {
                public static class Smooth
                {
                    private static bool? _fileExists = null;
                    private static string _saveFilePath = null;

                    private static bool FileExists(GNFS gnfs)
                    {
                        return _fileExists.Value;
                    }

                    public static void Append(GNFS gnfs)
                    {
                        if (gnfs.CurrentRelationsProgress.Relations.SmoothRelations.Any())
                        {
                            List<Relation> toSave = gnfs.CurrentRelationsProgress.Relations.SmoothRelations.Where(rel => !rel.IsPersisted).ToList();
                            foreach (Relation rel in toSave)
                            {
                                Append(gnfs, rel);
                            }
                        }
                    }

                    public static void Append(GNFS gnfs, Relation relation)
                    {
                        if (relation != null && relation.IsSmooth && !relation.IsPersisted)
                        {
                            gnfs.CurrentRelationsProgress.SmoothRelationsCounter += 1;

                            relation.IsPersisted = true;
                        }
                    }
                }

                public static class Rough
                {
                    private static bool? _fileExists = null;
                    private static string _saveFilePath = null;

                    private static bool FileExists(GNFS gnfs)
                    {
                        return _fileExists.Value;
                    }

                    public static void Append(GNFS gnfs)
                    {
                        if (gnfs.CurrentRelationsProgress.Relations.RoughRelations.Any())
                        {
                            List<Relation> toSave = gnfs.CurrentRelationsProgress.Relations.RoughRelations.Where(rel => !rel.IsPersisted).ToList();
                            foreach (Relation rel in toSave)
                            {
                                Append(gnfs, rel);
                            }
                        }
                    }

                    public static void Append(GNFS gnfs, Relation roughRelation)
                    {
                        if (roughRelation != null && !roughRelation.IsSmooth && !roughRelation.IsPersisted)
                        {
                            roughRelation.IsPersisted = true;
                        }
                    }
                }

                public static class Free
                {
                    public static void AllSolutions(GNFS gnfs)
                    {
                        if (gnfs.CurrentRelationsProgress.Relations.FreeRelations.Any())
                        {
                            gnfs.CurrentRelationsProgress.FreeRelationsCounter = 1;
                            foreach (List<Relation> solution in gnfs.CurrentRelationsProgress.Relations.FreeRelations)
                            {
                                SingleSolution(gnfs, solution);
                            }
                        }
                    }

                    public static void SingleSolution(GNFS gnfs, List<Relation> solution)
                    {
                        if (solution.Any())
                        {
                            solution.ForEach(rel => rel.IsPersisted = true);
                            gnfs.CurrentRelationsProgress.FreeRelationsCounter += 1;
                        }
                    }
                }
            }
        }
    }

    public static class MatrixSolve
    {
        public static void GaussianSolve(GNFS gnfs)
        {
            Serialization.Save.Relations.Smooth.Append(gnfs); // Persist any relations not already persisted to disk

            // Because some operations clear this collection after persisting unsaved relations (to keep memory usage light)...
            // We completely reload the entire relations collection from disk.
            // This ensure that all the smooth relations are available for the matrix solving step.
            //Serialization.Load.Relations.Smooth(ref gnfs);


            List<Relation> smoothRelations = gnfs.CurrentRelationsProgress.SmoothRelations.ToList();

            int smoothCount = smoothRelations.Count;

            BigInteger requiredRelationsCount = gnfs.CurrentRelationsProgress.SmoothRelationsRequiredForMatrixStep;

            while (smoothRelations.Count >= requiredRelationsCount)
            {
                // Randomly select n relations from smoothRelations
                List<Relation> selectedRelations = new List<Relation>();
                while (
                        selectedRelations.Count < requiredRelationsCount
                        ||
                        selectedRelations.Count % 2 != 0 // Force number of relations to be even
                    )
                {
                    int randomIndex = StaticRandom.Next(0, smoothRelations.Count);
                    selectedRelations.Add(smoothRelations[randomIndex]);
                    smoothRelations.RemoveAt(randomIndex);
                }

                GaussianMatrix gaussianReduction = new GaussianMatrix(gnfs, selectedRelations);
                gaussianReduction.TransposeAppend();
                gaussianReduction.Elimination();

                int number = 1;
                int solutionCount = gaussianReduction.FreeVariables.Count(b => b) - 1;
                List<List<Relation>> solution = new List<List<Relation>>();
                while (number <= solutionCount)
                {
                    List<Relation> relations = gaussianReduction.GetSolutionSet(number);
                    number++;

                    BigInteger algebraic = relations.Select(rel => rel.AlgebraicNorm).Product();
                    BigInteger rational = relations.Select(rel => rel.RationalNorm).Product();

                    CountDictionary algCountDict = new CountDictionary();
                    foreach (var rel in relations)
                    {
                        algCountDict.Combine(rel.AlgebraicFactorization);
                    }

                    bool isAlgebraicSquare = algebraic.IsSquare();
                    bool isRationalSquare = rational.IsSquare();

                    if (isAlgebraicSquare && isRationalSquare)
                    {
                        solution.Add(relations);
                        gnfs.CurrentRelationsProgress.AddFreeRelationSolution(relations);
                    }
                }

            }
        }
    }

    public static class StaticRandom
    {
        private static readonly Random rand = new Random();
        static StaticRandom()
        {
            int counter = rand.Next(100, 200);
            while (counter-- > 0)
            {
                rand.Next();
            }
        }

        public static int Next()
        {
            return rand.Next();
        }

        public static int Next(int maxValue)
        {
            return rand.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return rand.Next(minValue, maxValue);
        }

        public static double NextDouble()
        {
            return rand.NextDouble();
        }

        public static void NextBytes(byte[] bytes)
        {
            rand.NextBytes(bytes);
        }

        /// <summary>
        ///  Picks a random number from a range, where such numbers will be chosen uniformly across the entire range.
        /// </summary>
        public static BigInteger NextBigInteger(BigInteger lower, BigInteger upper)
        {
            if (lower > upper) { throw new ArgumentOutOfRangeException("Upper must be greater than upper"); }

            BigInteger delta = upper - lower;
            byte[] deltaBytes = delta.ToByteArray();
            byte[] buffer = new byte[deltaBytes.Length];
            deltaBytes = null;

            BigInteger result;
            while (true)
            {
                NextBytes(buffer);

                result = new BigInteger(buffer) + lower;

                if (result >= lower && result <= upper)
                {
                    buffer = null;
                    return result;
                }
            }
        }
    }

    public class GaussianMatrix
    {
        public List<bool[]> Matrix { get { return M; } }
        public bool[] FreeVariables { get { return freeCols; } }

        public int RowCount { get { return M.Count; } }
        public int ColumnCount { get { return M.Any() ? M.First().Length : 0; } }

        private List<bool[]> M;
        private bool[] freeCols;
        private bool eliminationStep;

        private GNFS _gnfs;
        private List<Relation> relations;
        public Dictionary<int, Relation> ColumnIndexRelationDictionary;
        private List<Tuple<Relation, bool[]>> relationMatrixTuple;

        public GaussianMatrix(GNFS gnfs, List<Relation> rels)
        {
            _gnfs = gnfs;
            relationMatrixTuple = new List<Tuple<Relation, bool[]>>();
            eliminationStep = false;
            freeCols = new bool[0];
            M = new List<bool[]>();

            relations = rels;

            List<GaussianRow> relationsAsRows = new List<GaussianRow>();

            foreach (Relation rel in relations)
            {
                GaussianRow row = new GaussianRow(_gnfs, rel);

                relationsAsRows.Add(row);
            }

            //List<GaussianRow> orderedRows = relationsAsRows.OrderBy(row1 => row1.LastIndexOfAlgebraic).ThenBy(row2 => row2.LastIndexOfQuadratic).ToList();

            List<GaussianRow> selectedRows = relationsAsRows.Take(_gnfs.CurrentRelationsProgress.SmoothRelationsRequiredForMatrixStep).ToList();

            int maxIndexRat = selectedRows.Select(row => row.LastIndexOfRational).Max();
            int maxIndexAlg = selectedRows.Select(row => row.LastIndexOfAlgebraic).Max();
            int maxIndexQua = selectedRows.Select(row => row.LastIndexOfQuadratic).Max();

            foreach (GaussianRow row in selectedRows)
            {
                row.ResizeRationalPart(maxIndexRat);
                row.ResizeAlgebraicPart(maxIndexAlg);
                row.ResizeQuadraticPart(maxIndexQua);
            }

            GaussianRow exampleRow = selectedRows.First();
            int newLength = exampleRow.GetBoolArray().Length;

            newLength++;

            selectedRows = selectedRows.Take(newLength).ToList();


            foreach (GaussianRow row in selectedRows)
            {
                relationMatrixTuple.Add(new Tuple<Relation, bool[]>(row.SourceRelation, row.GetBoolArray()));
            }
        }

        public void TransposeAppend()
        {
            List<bool[]> result = new List<bool[]>();
            ColumnIndexRelationDictionary = new Dictionary<int, Relation>();

            int index = 0;
            int numRows = relationMatrixTuple[0].Item2.Length;
            while (index < numRows)
            {
                ColumnIndexRelationDictionary.Add(index, relationMatrixTuple[index].Item1);

                List<bool> newRow = relationMatrixTuple.Select(bv => bv.Item2[index]).ToList();
                newRow.Add(false);
                result.Add(newRow.ToArray());

                index++;
            }

            M = result;
            freeCols = new bool[M.Count];
        }

        public void Elimination()
        {
            if (eliminationStep)
            {
                return;
            }

            int numRows = RowCount;
            int numCols = ColumnCount;

            freeCols = Enumerable.Repeat(false, numCols).ToArray();

            int h = 0;

            for (int i = 0; i < numRows && h < numCols; i++)
            {
                bool next = false;

                if (M[i][h] == false)
                {
                    int t = i + 1;

                    while (t < numRows && M[t][h] == false)
                    {
                        t++;
                    }

                    if (t < numRows)
                    {
                        //swap rows M[i] and M[t]

                        bool[] temp = M[i];
                        M[i] = M[t];
                        M[t] = temp;
                        temp = null;
                    }
                    else
                    {
                        freeCols[h] = true;
                        i--;
                        next = true;
                    }
                }
                if (next == false)
                {
                    for (int j = i + 1; j < numRows; j++)
                    {
                        if (M[j][h] == true)
                        {
                            // Add rows
                            // M [j] ← M [j] + M [i]

                            M[j] = Add(M[j], M[i]);
                        }
                    }
                    for (int j = 0; j < i; j++)
                    {
                        if (M[j][h] == true)
                        {
                            // Add rows
                            // M [j] ← M [j] + M [i]

                            M[j] = Add(M[j], M[i]);
                        }
                    }
                }
                h++;
            }

            eliminationStep = true;
        }

        public List<Relation> GetSolutionSet(int numberOfSolutions)
        {
            bool[] solutionSet = GetSolutionFlags(numberOfSolutions);

            int index = 0;
            int max = ColumnIndexRelationDictionary.Count;

            List<Relation> result = new List<Relation>();
            while (index < max)
            {
                if (solutionSet[index] == true)
                {
                    result.Add(ColumnIndexRelationDictionary[index]);
                }

                index++;
            }

            return result;
        }

        private bool[] GetSolutionFlags(int numSolutions)
        {
            if (!eliminationStep)
            {
                throw new Exception("Must call Elimination() method first!");
            }

            if (numSolutions < 1)
            {
                throw new ArgumentException($"{nameof(numSolutions)} must be greater than 1.");
            }

            int numRows = RowCount;
            int numCols = ColumnCount;

            if (numSolutions >= numCols)
            {
                throw new ArgumentException($"{nameof(numSolutions)} must be less than the column count.");
            }

            bool[] result = new bool[numCols];

            int j = -1;
            int i = numSolutions;

            while (i > 0)
            {
                j++;

                while (freeCols[j] == false)
                {
                    j++;
                }

                i--;
            }

            result[j] = true;

            for (i = 0; i < numRows - 1; i++)
            {
                if (M[i][j] == true)
                {
                    int h = i;
                    while (h < j)
                    {
                        if (M[i][h] == true)
                        {
                            result[h] = true;
                            break;
                        }
                        h++;
                    }
                }
            }

            return result;
        }

        public static bool[] Add(bool[] left, bool[] right)
        {
            if (left.Length != right.Length) throw new ArgumentException($"Both vectors must have the same length.");

            int length = left.Length;
            bool[] result = new bool[length];

            int index = 0;
            while (index < length)
            {
                result[index] = left[index] ^ right[index];
                index++;
            }

            return result;
        }

        public static string VectorToString(bool[] vector)
        {
            return string.Join(",", vector.Select(b => b ? '1' : '0'));
        }

        public static string MatrixToString(List<bool[]> matrix)
        {
            return string.Join(Environment.NewLine, matrix.Select(i => VectorToString(i)));
        }

        public override string ToString()
        {
            return MatrixToString(M);
        }
    }

    public class GaussianRow
    {
        public bool Sign { get; set; }

        public List<bool> RationalPart { get; set; }
        public List<bool> AlgebraicPart { get; set; }
        public List<bool> QuadraticPart { get; set; }

        public int LastIndexOfRational { get { return RationalPart.LastIndexOf(true); } }
        public int LastIndexOfAlgebraic { get { return AlgebraicPart.LastIndexOf(true); } }
        public int LastIndexOfQuadratic { get { return QuadraticPart.LastIndexOf(true); } }

        public Relation SourceRelation { get; private set; }

        public GaussianRow(GNFS gnfs, Relation relation)
        {
            SourceRelation = relation;

            if (relation.RationalNorm.Sign == -1)
            {
                Sign = true;
            }
            else
            {
                Sign = false;
            }

            FactorPairCollection qfb = gnfs.QuadraticFactorPairCollection;

            BigInteger rationalMaxValue = gnfs.PrimeFactorBase.RationalFactorBaseMax;
            BigInteger algebraicMaxValue = gnfs.PrimeFactorBase.AlgebraicFactorBaseMax;

            RationalPart = GetVector(relation.RationalFactorization, rationalMaxValue).ToList();
            AlgebraicPart = GetVector(relation.AlgebraicFactorization, algebraicMaxValue).ToList();
            QuadraticPart = qfb.Select(qf => QuadraticResidue.GetQuadraticCharacter(relation, qf)).ToList();
        }

        protected static bool[] GetVector(CountDictionary primeFactorizationDict, BigInteger maxValue)
        {
            int primeIndex = PrimeFactory.GetIndexFromValue(maxValue);

            bool[] result = new bool[primeIndex];

            if (primeFactorizationDict.Any())
            {
                foreach (KeyValuePair<BigInteger, BigInteger> kvp in primeFactorizationDict)
                {
                    if (kvp.Key > maxValue)
                    {
                        continue;
                    }
                    if (kvp.Key == -1)
                    {
                        continue;
                    }
                    if (kvp.Value % 2 == 0)
                    {
                        continue;
                    }

                    int index = PrimeFactory.GetIndexFromValue(kvp.Key);
                    result[index] = true;
                }
            }

            return result;
        }

        public bool[] GetBoolArray()
        {
            List<bool> result = new List<bool>() { Sign };
            result.AddRange(RationalPart);
            result.AddRange(AlgebraicPart);
            result.AddRange(QuadraticPart);
            //result.Add(false);
            return result.ToArray();
        }

        public void ResizeRationalPart(int size)
        {
            RationalPart = RationalPart.Take(size + 1).ToList();
        }

        public void ResizeAlgebraicPart(int size)
        {
            AlgebraicPart = AlgebraicPart.Take(size + 1).ToList();
        }

        public void ResizeQuadraticPart(int size)
        {
            QuadraticPart = QuadraticPart.Take(size + 1).ToList();
        }
    }

    public class QuadraticResidue
    {
        // a^(p-1)/2 ≡ 1 (mod p)
        public static bool IsQuadraticResidue(BigInteger a, BigInteger p)
        {
            BigInteger quotient = BigInteger.Divide(p - 1, 2);
            BigInteger modPow = BigInteger.ModPow(a, quotient, p);

            return modPow.IsOne;
        }

        public static bool GetQuadraticCharacter(Relation rel, FactorPair quadraticFactor)
        {
            BigInteger ab = rel.A + rel.B;
            BigInteger abp = BigInteger.Abs(BigInteger.Multiply(ab, quadraticFactor.P));

            int legendreSymbol = Legendre.Symbol(abp, quadraticFactor.R);
            return (legendreSymbol != 1);
        }
    }

    public static class Legendre
    {
        /// <summary>
        /// Legendre Symbol returns 1 for a (nonzero) quadratic residue mod p, -1 for a non-quadratic residue (non-residue), or 0 on zero.
        /// </summary>		
        public static int Symbol(BigInteger a, BigInteger p)
        {
            if (p < 2) { throw new ArgumentOutOfRangeException(nameof(p), $"Parameter '{nameof(p)}' must not be < 2, but you have supplied: {p}"); }
            if (a == 0) { return 0; }
            if (a == 1) { return 1; }

            int result;
            if (a.Mod(2) == 0)
            {
                result = Symbol(a >> 2, p); // >> right shift == /2
                if (((p * p - 1) & 8) != 0) // instead of dividing by 8, shift the mask bit
                {
                    result = -result;
                }
            }
            else
            {
                result = Symbol(p.Mod(a), a);
                if (((a - 1) * (p - 1) & 4) != 0) // instead of dividing by 4, shift the mask bit
                {
                    result = -result;
                }
            }
            return result;
        }

        /// <summary>
        ///  Find r such that (r | m) = goal, where  (r | m) is the Legendre symbol, and m = modulus
        /// </summary>
        public static BigInteger SymbolSearch(BigInteger start, BigInteger modulus, BigInteger goal)
        {
            if (goal != -1 && goal != 0 && goal != 1)
            {
                throw new Exception($"Parameter '{nameof(goal)}' may only be -1, 0 or 1. It was {goal}.");
            }

            BigInteger counter = start;
            BigInteger max = counter + modulus + 1;
            do
            {
                if (Symbol(counter, modulus) == goal)
                {
                    return counter;
                }
                counter++;
            }
            while (counter <= max);

            //return counter;
            throw new Exception("Legendre symbol matching criteria not found.");
        }
    }

    public static class BigIntegerCollectionExtensionMethods
    {
        public static BigInteger Sum(this IEnumerable<BigInteger> source)
        {
            BigInteger result = BigInteger.Zero;
            foreach (BigInteger bi in source)
            {
                result = BigInteger.Add(result, bi);
            }

            return result;
        }

        public static BigInteger Product(this IEnumerable<BigInteger> input)
        {
            BigInteger result = 1;
            foreach (BigInteger bi in input)
            {
                result = BigInteger.Multiply(result, bi);
            }
            return result;
        }

        public static BigInteger ProductMod(this IEnumerable<BigInteger> input, BigInteger modulus)
        {
            BigInteger result = 1;
            foreach (BigInteger bi in input)
            {
                result = BigInteger.Multiply(result, bi);
                if (result >= modulus || result <= -modulus)
                {
                    result = result % modulus;
                }
            }
            return result;
        }
    }
}
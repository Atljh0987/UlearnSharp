using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Incapsulation.RationalNumbers
{
    public struct Rational
    {
        public int Numerator { get; private set; }
        public int Denominator { get; private set; }
        public bool IsNan => Denominator == 0;

        public Rational(int numerator, int denominator)
        {
            Numerator = numerator;
            //Denominator = denominator;
            Denominator = (numerator == 0) ? ((denominator == 0) ? 0 : 1) : denominator;

            if (numerator != 0)
            {
                int gcd = (int)BigInteger.GreatestCommonDivisor(numerator, denominator);
                gcd = (denominator < 0) ? -gcd : gcd; 

                Numerator /= gcd;
                Denominator /= gcd;
            }
        }

        public Rational(int numerator)
            : this(numerator, 1)
        { }

        public static Rational operator +(Rational x, Rational y)
        {
            return new Rational(x.Numerator * y.Denominator + y.Numerator * x.Denominator, x.Denominator * y.Denominator);
        }

        public static Rational operator -(Rational x, Rational y)
        {
            return new Rational(x.Numerator * y.Denominator - y.Numerator * x.Denominator, x.Denominator * y.Denominator);
        }

        public static Rational operator *(Rational x, Rational y)
        {
            return new Rational(x.Numerator * y.Numerator, x.Denominator * y.Denominator);
        }

        public static Rational operator /(Rational x, Rational y)
        {
            return new Rational(x.Numerator * y.Denominator, ((y.Denominator == 0) ? 0 : x.Denominator) * y.Numerator);
        }

        public static implicit operator double(Rational x)
        {
            if (x.Denominator == 0) x.Numerator = 0;
            return (double)x.Numerator / (double)x.Denominator;
        }

        public static implicit operator Rational(int x)
        {
            return new Rational(x);
        }

        public static explicit operator int(Rational x)
        {
            if(x.Denominator == 1)
                return x.Numerator;
            else
                throw new ArgumentException();
        }

    }
}

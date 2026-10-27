using System.Globalization;

namespace SlideShowMaker
{
    public partial struct Fraction(long numerator, long denominator): IConvertible
    {
        public long Numerator { get; set; } = numerator / Gcd(denominator, numerator);
        public long Denominator { get; set; } = denominator / Gcd(denominator, numerator);

        public static Fraction FromDouble(double value)
        {
            if (value == Math.Floor(value)) return new Fraction((long)value, 1);

            var fractionalPart = value - Math.Floor(value);
            long scale = 1;

            while (fractionalPart != Math.Floor(fractionalPart) && scale < 1000000)
            {
                scale *= 10;
                fractionalPart = value * scale;
            }

            var numerator = (long)Math.Round(value * scale);
            var denominator = scale;

            var gcd = Gcd(numerator, denominator);
            return new Fraction(numerator / gcd, denominator / gcd);
        }

        private static long Gcd(long a, long b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return Math.Abs(a);
        }

        private static long Lcm(long a, long b)
        {
            return Math.Abs(a * b) / Gcd(a, b);
        }

        public readonly Fraction Inversed => new(Denominator, Numerator);

        public readonly Fraction FindNearestMultiple(Fraction to)
        {
            var lcm = Lcm(Denominator, to.Denominator);

            var aScaled = Numerator * (lcm / Denominator);
            var cScaled = to.Numerator * (lcm / to.Denominator);

            var quotient = aScaled / cScaled;
            var lowerMultiple = quotient * cScaled;
            var upperMultiple = (quotient + 1) * cScaled;

            var closestMultiple = (aScaled - lowerMultiple) <= (upperMultiple - aScaled) ? lowerMultiple : upperMultiple;

            var newNumerator = closestMultiple / (lcm / Denominator);
            return new Fraction(newNumerator, Denominator);
        }

        public override readonly string ToString()
        {
            if (Denominator == 1) return Numerator.ToString();

            if (IsFiniteDecimal()) return ((double) Numerator / Denominator).ToString(CultureInfo.InvariantCulture);

            return $"{Numerator.ToString(CultureInfo.InvariantCulture)}/{Denominator.ToString(CultureInfo.InvariantCulture)}";
        }

        public readonly double Value => (double) Numerator / Denominator;

        public override readonly int GetHashCode() => Value.GetHashCode();

        private readonly bool IsFiniteDecimal()
        {
            var simplifiedDenominator = Denominator;

            // Убираем все множители 2
            while (simplifiedDenominator % 2 == 0)
            {
                simplifiedDenominator /= 2;
            }

            // Убираем все множители 5
            while (simplifiedDenominator % 5 == 0)
            {
                simplifiedDenominator /= 5;
            }

            // Если осталась единица, знаменатель состоял только из 2 и 5
            return simplifiedDenominator == 1;
        }

        public readonly TypeCode GetTypeCode() => TypeCode.Double;

        public bool ToBoolean(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public byte ToByte(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public char ToChar(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public readonly double ToDouble(IFormatProvider? provider) => Value;

        public short ToInt16(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public int ToInt32(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public long ToInt64(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public sbyte ToSByte(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public float ToSingle(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public string ToString(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public object ToType(Type conversionType, IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public ushort ToUInt16(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public uint ToUInt32(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public ulong ToUInt64(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }
    }
}

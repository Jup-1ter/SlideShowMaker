namespace SlideShowMaker
{
    partial struct Fraction : IComparable<double>, IComparable<Fraction>, IEquatable<Fraction>, IEquatable<double>
    {
        public static bool operator ==(Fraction left, Fraction right) => left.Equals(right);

        public static bool operator !=(Fraction left, Fraction right) => !(left == right);

        public override readonly bool Equals(object? obj) => obj is Fraction f && Equals(f) || obj is double d && Equals(d);

        public readonly int CompareTo(double other) => Value.CompareTo(other);

        public readonly int CompareTo(Fraction other) => (int)(Numerator * other.Denominator - other.Numerator * Denominator);

        public readonly bool Equals(Fraction other) => other.Numerator * Denominator == other.Denominator * Numerator;

        public readonly bool Equals(double other) => other == Value;
    }
}

namespace SlideShowMaker
{
    partial struct Fraction
    {
        public static Fraction operator /(Fraction left, double right) => new(left.Numerator, (long)(left.Denominator * right));
        public static Fraction operator *(Fraction left, double right) => new((long)(left.Numerator * right), left.Denominator);
        public static Fraction operator +(Fraction left, Fraction right) =>
            new(left.Numerator * right.Denominator + right.Numerator * left.Denominator, left.Denominator * right.Denominator);
        public static Fraction operator -(Fraction left, Fraction right) =>
            new(left.Numerator * right.Denominator - right.Numerator * left.Denominator, left.Denominator * right.Denominator);
        public static Fraction operator +(Fraction left, double right) => left + Fraction.FromDouble(right);
        public static Fraction operator -(Fraction left, double right) => left - Fraction.FromDouble(right);
    }
}

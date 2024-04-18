namespace Raytracing.Math
{
	internal class Interval (double min, double max)
	{
		public double Min = min;
		public double Max = max;

		public Interval() : this(double.PositiveInfinity, double.NegativeInfinity) { }

		public bool Contains(double x) => Min <= x && x <= Max;
		public bool Surrounds(double x) => Min < x && x < Max;
		public double Clamp(double x) => System.Math.Clamp(x, Min, Max);

		public static Interval Empty => new Interval();
		public static Interval Universe => new Interval(double.NegativeInfinity, double.PositiveInfinity);
	}
}

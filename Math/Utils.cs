using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing.Math
{
	internal static class Utils
	{
		private static readonly Random random = new();
		public static double RandomDouble()
		{
			return random.NextDouble();
		}
		public static double RandomDouble(double min, double max)
		{
			return min + (max - min) * random.NextDouble();
		}
		public static double DegToRad(double degrees)
		{
			return degrees * double.Pi / 180;
		}
	}
}

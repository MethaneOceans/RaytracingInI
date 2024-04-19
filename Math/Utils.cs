using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing.Math
{
	internal static class Utils
	{
		public static double DegToRad(double degrees)
		{
			return degrees * double.Pi / 180;
		}
	}
}

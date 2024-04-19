using Raytracing.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing.Rendering.Materials
{
	internal class Dielectric(double refractionIndex) : IMaterial
	{
		// Refractive index in vacuum or air, or the ratio of the material's refractive index over
		// the refractive index of the enclosing media
		private double refractionIndex = refractionIndex;

		public bool Scatter(in Ray rayIn, in HitRecord rec, ref Vec3 colAttentuation, out Ray scattered)
		{
			colAttentuation = new Vec3(1.0, 1.0, 1.0);
			double ri = rec.FrontFace ? (1.0 / refractionIndex) : refractionIndex;

			Vec3 unitDirection = rayIn.Direction.Unit();
			Vec3 refracted = Vec3.Refract(unitDirection, rec.Normal, ri);

			scattered = new Ray(rec.Point, refracted);
			return true;
		}
	}
}

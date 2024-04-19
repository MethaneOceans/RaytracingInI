using Raytracing.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing.Rendering.Materials
{
	internal class Lambertian : IMaterial
	{
		private Vec3 albedo;

		public Lambertian(in Vec3 albedo)
		{
			this.albedo = albedo;
		}

		public bool Scatter(in Ray ray, in HitRecord rec, ref Vec3 attentuation, out Ray scattered)
		{
			Vec3 scatterDirection = rec.Normal + Vec3.RandomUnit();

			if (scatterDirection.NearZero()) scatterDirection = rec.Normal;

			scattered = new Ray(rec.Point, scatterDirection);
			attentuation = albedo;
			return true;
		}
	}
}

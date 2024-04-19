using Raytracing.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing.Rendering.Materials
{
	internal class Metal(in Vec3 albedo, double fuzz) : IMaterial
	{
		private Vec3 albedo = albedo;
		private double fuzz = fuzz <= 1 ? fuzz : 1;

		public bool Scatter(in Ray rayIn, in HitRecord rec, ref Vec3 colAttentuation, out Ray scattered)
		{
			Vec3 reflected = Vec3.Reflect(ref rec.Point, rec.Normal);
			reflected = reflected.Unit() + (fuzz * Vec3.RandomUnit());
			scattered = new Ray(rec.Point, reflected);
			colAttentuation = albedo;
			return (Vec3.Dot(scattered.Direction, rec.Normal) > 0);
		}
	}
}

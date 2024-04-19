using Raytracing.Math;
using static System.Math;
using static Raytracing.Math.Vec3;

namespace Raytracing.Rendering.Materials
{
	internal class Dielectric(double refractionIndex) : IMaterial
	{
		// Refractive index in vacuum or air, or the ratio of the material's refractive index over
		// the refractive index of the enclosing media
		private readonly double refractionIndex = refractionIndex;

		public bool Scatter(in Ray rayIn, in HitRecord rec, ref Vec3 colAttentuation, out Ray scattered)
		{
			// Color
			colAttentuation = new Vec3(1.0, 1.0, 1.0);

			// Invert the index if the ray is colliding on a backface
			double ri = rec.FrontFace ? (1.0 / refractionIndex) : refractionIndex;

			// Normalize and refract ray
			Vec3 unitDirection = rayIn.Direction.Unit();

			double cosTheta = Min(Dot(-unitDirection, rec.Normal), 1.0);
			double sinTheta = Sqrt(1.0 - cosTheta * cosTheta);

			bool cannotRefract = ri * sinTheta > 1.0;

			Vec3 direction;
			
			if (cannotRefract)
			{
				direction = Reflect(unitDirection, rec.Normal);
			}
			else
			{
				direction = Refract(unitDirection, rec.Normal, ri);
			}

			scattered = new Ray(rec.Point, direction);
			return true;
		}
	}
}

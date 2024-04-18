namespace Raytracing.Rendering.Hittables
{
    using Raytracing.Math;
    using Raytracing.Rendering;
    using static System.Math;

    internal class Sphere(Vec3 center, double radius) : IHittable
    {
        private Vec3 Center = center;
        private double Radius = radius;

        public bool Hit(in Ray ray, ref HitRecord rec, Interval tRange)
        {
            if (tRange.Max == 0)
            {
                Console.WriteLine();
            }
            Vec3 oc = Center - ray.Origin;

            double a = ray.Direction.LengthSquared();
            double h = Vec3.Dot(ray.Direction, oc);
            double c = oc.LengthSquared() - Radius * Radius;

            double D = h * h - a * c;

            if (D < 0) return false;

            double sqrtD = Sqrt(D);

            // Find nearest acceptable root
            double root = (h - sqrtD) / a;
            if (!tRange.Surrounds(root))
            {
                root = (h + sqrtD) / a;
                if (!tRange.Surrounds(root)) return false;
            }

            rec.t = root;
            rec.Point = ray.At(rec.t);

            Vec3 outwardNormal = (rec.Point - Center) / Radius;
            rec.SetFaceNormal(ray, outwardNormal);

            return true;
        }
    }
}

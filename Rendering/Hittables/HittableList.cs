using Raytracing.Math;
using Raytracing.Rendering;

namespace Raytracing.Rendering.Hittables
{
    internal class HittableList : IHittable
    {
        public List<IHittable> Hittables = [];

        public bool Hit(in Ray ray, ref HitRecord hitRec, Interval tRange)
        {
            HitRecord tempRec = new HitRecord();
            bool anyHit = false;
            double closestHit = tRange.Max;

            foreach (IHittable hittable in Hittables)
            {
                if (hittable.Hit(ray, ref tempRec, new Interval(tRange.Min, closestHit)))
                {
                    anyHit = true;
                    closestHit = tempRec.t;
                    hitRec = tempRec;
                }
            }

            return anyHit;
        }
    }
}

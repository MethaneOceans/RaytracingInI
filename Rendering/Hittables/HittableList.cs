using Raytracing.Math;
using Raytracing.Rendering;

namespace Raytracing.Rendering.Hittables
{
    internal class HittableList : IHittable
    {
        private readonly List<IHittable> Hittables = [];
        public void Add(IHittable hittable)
        {
            Hittables.Add(hittable);
        }
        public void Clear() => Hittables.Clear();

        public bool Hit(in Ray ray, ref HitRecord hitRec, Interval tRange)
        {
            HitRecord tempRec = new();
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

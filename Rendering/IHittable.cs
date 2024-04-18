namespace Raytracing.Rendering
{
	using Raytracing.Math;

	internal interface IHittable
    {
        public bool Hit(in Ray ray, ref HitRecord hitRec, Interval tRange);
    }
}

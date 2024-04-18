using Raytracing.Math;

namespace Raytracing.Rendering
{
    internal class HitRecord(Vec3 point, Vec3 normal, double t)
    {
        public Vec3 Point = point;
        public Vec3 Normal = normal;
        public double t = t;
        public bool FrontFace;

        public HitRecord() : this(new Vec3(), new Vec3(), 0) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="outwardNormal">Unit length normal</param>
        public void SetFaceNormal(in Ray ray, in Vec3 outwardNormal)
        {
            FrontFace = Vec3.Dot(ray.Direction, outwardNormal) < 0;
            Normal = FrontFace ? outwardNormal : -outwardNormal;
        }
    }
}

namespace Raytracing.Math
{
    internal class Ray(Vec3 origin, Vec3 direction)
    {
        public Vec3 Origin { get; private set; } = origin;
        public Vec3 Direction { get; private set; } = direction;

        public Ray() : this(new Vec3(), new Vec3()) { }

        public Vec3 At(double t)
        {
            return Origin + t * Direction;
        }
    }
}

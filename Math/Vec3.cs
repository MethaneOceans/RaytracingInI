namespace Raytracing.Math
{
    using static System.Math;
    using SixLabors.ImageSharp.PixelFormats;
    using System.Numerics;
	using SixLabors.ImageSharp;

	internal struct Vec3(double x, double y, double z)
    {
        public double X = x;
        public double Y = y;
        public double Z = z;

        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static Vec3 operator -(Vec3 v)
        {
            return new Vec3(-v.X, -v.Y, -v.Z);
        }
        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return a + -b;
        }
        public static Vec3 operator *(Vec3 v, double s)
        {
            return new Vec3(s * v.X, s * v.Y, s * v.Z);
        }
        public static Vec3 operator *(double s, Vec3 v)
        {
            return v * s;
        }
        public static Vec3 operator /(Vec3 v, double s)
        {
            return v * (1 / s);
        }

        public readonly double LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
        }
        public readonly double Length()
        {
            return Sqrt(LengthSquared());
        }
        public static double Dot(Vec3 a, Vec3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }
        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            return new Vec3(
                x: a.Y * b.Z - a.Z * b.Y,
                y: a.Z * b.X - a.X * b.Z,
                z: a.X * b.Y - a.Y * b.X
            );
        }

        public override readonly string ToString()
        {
            return $"Vec3: (X:{X}; Y:{Y}; Z:{Z})";
        }

        public readonly Vec3 Unit()
        {
            return this / Length();
        }

        public static implicit operator Vector3(Vec3 v)
        {
            return new Vector3((float)v.X, (float)v.Y, (float)v.Z);
        }
        public static implicit operator Rgba32(Vec3 v)
        {
            return new Rgba32(v);
        }
    }
}

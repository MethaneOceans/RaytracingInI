namespace Raytracing.Math
{
    using static System.Math;
    using SixLabors.ImageSharp.PixelFormats;
    using System.Numerics;
    using static Math.Utils;
	internal struct Vec3(double x, double y, double z)
    {
        static readonly Random random = new();

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
        public static Vec3 operator *(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
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

        

        /// <summary>
        /// Create a unit length copy of the vector instance
        /// </summary>
        /// <returns>A unit vector on the same line as the calling instance</returns>
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
        
        /// <summary>
        /// Generate a vector with components randomized in the range of [0,1)
        /// </summary>
        public static Vec3 RandomVec3()
        {
            return new Vec3(random.NextDouble(), random.NextDouble(), random.NextDouble());
        }
        /// <summary>
        /// Generate a vector with components randomized in the range of [min,max)
        /// </summary>
        public static Vec3 RandomVec3(double min, double max)
        {
            double x = RandomDouble(min, max);
            double y = RandomDouble(min, max);
            double z = RandomDouble(min, max);
            return new Vec3(x, y, z);
        }
        /// <summary>
        /// Generate a vector with randomized components with a magnitude less than one
        /// </summary>
        public static Vec3 RandomInUnitSphere()
        {
            while (true)
            {
                Vec3 v = RandomVec3(-1, 1);
                if (v.LengthSquared() < 1) return v;   
            }
        }
        /// <summary>
        /// Generate a random unit vector
        /// </summary>
        /// <returns></returns>
        public static Vec3 RandomUnit()
        {
            return RandomInUnitSphere().Unit();
        }
        /// <summary>
        /// Generate a random unit vector on the same hemisphere as the given vector
        /// </summary>
        public static Vec3 RandomOnHemisphere(in Vec3 normal)
        {
            Vec3 onUnitSphere = RandomUnit();
            if (Dot(onUnitSphere, normal) > 0) return onUnitSphere;
            else return -onUnitSphere;
        }
        public static Vec3 RandomInDisk()
        {
            while (true)
            {
                Vec3 p = new(RandomDouble(-1, 1), RandomDouble(-1, 1), 0);
                if (p.LengthSquared() < 1) return p;
			}
        }


        /// <summary>
        /// Refract a ray into the geometry given a normal and the refraction index
        /// </summary>
        /// <param name="uv">The vector that is refracted (Value won't be changed)</param>
        /// <param name="n">Geometry normal, determines the angle of incidence</param>
        /// <param name="etaiOverEtat">Refraction index</param>
        /// <returns>Resulting vector after refraction</returns>
        public static Vec3 Refract(in Vec3 uv, in Vec3 n, double etaiOverEtat)
        {
            double cosTheta = Min(Dot(-uv, n), 1.0);
            Vec3 rOutPerp = etaiOverEtat * (uv + cosTheta * n);
            Vec3 rOutParallel = -Sqrt(Abs(1.0 - rOutPerp.LengthSquared())) * n;
            return rOutPerp + rOutParallel;
        }
        /// <summary>
        /// Reflect a vector along the given normal
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
		public static Vec3 Reflect(in Vec3 vector, in Vec3 normal)
		{
			return vector - 2 * Dot(vector, normal) * normal;
		}

		/// <summary>
		/// Checks if the vectors has a near zero magnitude (componentwise since length calculation takes long)
		/// </summary>
		/// <returns></returns>
		public readonly bool NearZero()
		{
			double s = 1e-8;
			return Abs(X) < s && Abs(Y) < s && Abs(Z) < s;
		}

		public override readonly string ToString()
		{
			return $"Vec3: (X:{X}; Y:{Y}; Z:{Z})";
		}
	}
}

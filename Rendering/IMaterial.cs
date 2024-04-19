using Raytracing.Math;

namespace Raytracing.Rendering
{
	internal interface IMaterial
	{
		public bool Scatter(in Ray rayIn, in HitRecord rec, ref Vec3 colAttentuation, out Ray scattered);
	}
}

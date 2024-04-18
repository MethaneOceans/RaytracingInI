namespace Raytracing
{
    using Raytracing.Math;
    using Raytracing.Rendering;
    using Raytracing.Rendering.Hittables;

    // Raytracing in imaginary time. Because I never finish it
    internal class RaytracingInI
	{
		static void Main(string[] args)
		{
			_ = args;

			HittableList world = new();
			world.Hittables.Add(new Sphere(new Vec3(0, 0, -1), 0.5));
			world.Hittables.Add(new Sphere(new Vec3(0, -100.5, -1), 100));

			Camera cam = new()
			{
				Width = 512,
				Height = 512,
				SamplesPPixel = 100,
			};

			cam.Render(world);
		}
	}
}

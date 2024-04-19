namespace Raytracing
{
    using Raytracing.Math;
    using Raytracing.Rendering;
    using Raytracing.Rendering.Hittables;
	using Raytracing.Rendering.Materials;

	// Raytracing in imaginary time. Because I never finish it
	internal class RaytracingInI
	{
		static void Main(string[] args)
		{
			_ = args;
			Console.WriteLine();

			HittableList world = new();

			IMaterial groundMat = new Lambertian(new Vec3(0.8, 0.8, 0.0));
			IMaterial centerMat = new Lambertian(new Vec3(0.1, 0.2, 0.5));
			IMaterial leftMat = new Metal(new Vec3(0.8, 0.8, 0.8), 0.0);
			IMaterial rightMat = new Metal(new Vec3(0.8, 0.6, 0.2), 0.1);
			IMaterial glassMat = new Dielectric(1.5);

			world.Hittables.Add(new Sphere(new Vec3( 0.0, -100.5, -1.0), 100.0, groundMat)); // Ground
			world.Hittables.Add(new Sphere(new Vec3( 0.0,    0.0, -1.2),   0.5, glassMat)); // Center
			world.Hittables.Add(new Sphere(new Vec3(-1.0,    0.0, -1.0),   0.5, leftMat)); // Left
			world.Hittables.Add(new Sphere(new Vec3( 1.0,    0.0, -1.0),   0.5, rightMat)); // Right

			Camera cam = new()
			{
				Width = 512,
				Height = 512,
				SamplesPPixel = 100,
				MaxDepth = 25,
			};

			cam.ThreadedRender(world);
		}
	}
}

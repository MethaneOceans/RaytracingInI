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
			Console.Title = "Raytracing";
			Console.WriteLine();

			HittableList world = new();

			IMaterial groundMat = new Lambertian(new Vec3(0.8, 0.8, 0.0));
			IMaterial centerMat = new Lambertian(new Vec3(0.1, 0.2, 0.5));
			IMaterial leftMat	= new Dielectric(1.50);
			IMaterial bubbleMat	= new Dielectric(1.00 / 1.50);
			IMaterial rightMat	= new Metal		(new Vec3(0.8, 0.6, 0.2), 1.0);

			world.Add( new Sphere (new Vec3 ( 0.0, -100.5, -1.0), 100.0, groundMat)); // Ground
			world.Add( new Sphere (new Vec3 ( 0.0,    0.0, -1.2),   0.5, centerMat)); // Center
			world.Add( new Sphere (new Vec3 (-1.0,    0.0, -1.0),   0.5, leftMat));	// Left
			world.Add( new Sphere (new Vec3 (-1.0,    0.0, -1.0),   0.4, bubbleMat)); // Left
			world.Add( new Sphere (new Vec3 ( 1.0,    0.0, -1.0),   0.5, rightMat));  // Right

			Camera cam = new()
			{
				Width = 960,
				Height = 540,
				SamplesPPixel = 1000,
				MaxDepth = 50,

				vFOV = 40,
				LookFrom = new Vec3(-2, 2, 1),
				LookAt = new Vec3(0, 0, -1),
				ViewUp = new Vec3(0, 1, 0),

				DefocusAngle = 4.0,
				FocusDistance = 3.4,
			};

			cam.ThreadedRender(world);
		}
	}
}

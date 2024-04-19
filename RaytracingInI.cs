namespace Raytracing
{
	using Raytracing.Math;
	using static Raytracing.Math.Utils;
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

			IMaterial groundMat = new Lambertian(new Vec3(0.5, 0.5, 0.5));
			world.Add(new Sphere(new Vec3(0, -1000, 0), 1000, groundMat));

			for (int a = -11; a < 11; a++)
			{
				for (int b = -11; b < 11; b++)
				{
					double chooseMaterial = RandomDouble();
					Vec3 center = new(a + 0.9 * RandomDouble(), 0.2, b + 0.9 * RandomDouble());

					if ((center - new Vec3(4, 0.2, 0)).Length() > 0.9)
					{
						IMaterial sphereMat;

						if (chooseMaterial < 0.8)
						{
							// diffuse
							Vec3 albedo = Vec3.RandomVec3() * Vec3.RandomVec3();
							sphereMat = new Lambertian(albedo);
						}
						else if (chooseMaterial < 0.95)
						{
							// metal
							Vec3 albedo = Vec3.RandomVec3(0.5, 1);
							double fuzz = RandomDouble(0, 0.5);
							sphereMat = new Metal(albedo, fuzz);
						}
						else
						{
							// Glass
							sphereMat = new Dielectric(1.5);
						}

						world.Add(new Sphere(center, 0.2, sphereMat));
					}
				}
			}

			IMaterial matA = new Dielectric(1.5);
			world.Add(new Sphere(new Vec3(0, 1, 0), 1.0, matA));

			IMaterial matB = new Lambertian(new Vec3(0.4, 0.2, 0.1));
			world.Add(new Sphere(new Vec3(-4, 1, 0), 1.0, matB));

			IMaterial matC = new Metal(new Vec3(0.7, 0.6, 0.5), 0.0);
			world.Add(new Sphere(new Vec3(4, 1, 0), 1.0, matC));

			Camera cam = new()
			{
				Width = 960 / 2,
				Height = 540 / 2,
				SamplesPPixel = 100,
				MaxDepth = 50,

				vFOV = 20,
				LookFrom = new Vec3(13, 2, 3),
				LookAt = new Vec3(),
				ViewUp = new Vec3(0, 1, 0),

				DefocusAngle = 0.6,
				FocusDistance = 10,
			};

			cam.ThreadedRender(world);
		}
	}
}

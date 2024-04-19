namespace Raytracing.Rendering
{
	using static System.Math;

	using Math;
	using static Raytracing.Math.Utils;

	using SixLabors.ImageSharp;
	using SixLabors.ImageSharp.PixelFormats;
	using Raytracing.Rendering.Hittables;

	internal class Camera
	{
		private readonly Random random = new();

		public int Width;			// Image width
		public int Height;			// Image height
		public int SamplesPPixel;	// Sample rays per pixel
		public int MaxDepth;		// Maximum ray bounces

		public double vFOV = 90;					// Vertical FOV
		public Vec3 LookFrom = new Vec3();			// Point the camera is looking from
		public Vec3 LookAt = new Vec3(0, 0, -1);	// Point to look at
		public Vec3 ViewUp = new Vec3(0, 1, 0);     // Camera relative up direction

		public double DefocusAngle = 0;		// Variation angle of rays through each pixel
		public double FocusDistance = 10;	// Distance from camera LookFrom to plane of perfect focus

		private Vec3 center;
		private Vec3 pixel00Location;
		private Vec3 pixelDeltaU;
		private Vec3 pixelDeltaV;
		private double pixelSampleScale;
		private Vec3 u, v, w;
		private Vec3 defocusDiskU;
		private Vec3 defocusDiskV;

		// Only used for threads
		Mutex imageMutex = new Mutex();
		Image<Rgba32>? protectedImage;
		int pixelsDone = 0;

		private void Initialize()
		{
			center = LookFrom;
			pixelSampleScale = 1.0 / SamplesPPixel;

			// Set viewport dimensions;
			double theta = DegToRad(vFOV);
			double h = Tan(theta / 2);

			double aspectRatio = Width / (float)Height;
			double viewportHeight = 2 * h * FocusDistance;
			double viewportWidth = viewportHeight * aspectRatio;

			// Calculate camera space basis vectors
			w = (LookFrom - LookAt).Unit();
			u = Vec3.Cross(ViewUp, w).Unit();
			v = Vec3.Cross(w, u);

			// Calculate the vectors across the horizontal and vertical viewport edges
			Vec3 viewportU = viewportWidth * u;
			Vec3 viewportV = viewportHeight * -v;

			// Calculate horizontal and vertical delta vectors from pixel to pixel.
			pixelDeltaU = viewportU / Width;
			pixelDeltaV = viewportV / Height;

			// Calculate position of upper left pixel.
			Vec3 viewportUpperLeft = center - FocusDistance * w - viewportU / 2 - viewportV / 2;
			pixel00Location = viewportUpperLeft + 0.5 * (pixelDeltaU + pixelDeltaV);

			// Calculate defocus disk basis
			double defocusRadius = FocusDistance * Tan(DegToRad(DefocusAngle / 2));
			defocusDiskU = u * defocusRadius;
			defocusDiskV = v * defocusRadius;
		}

		public void ThreadedRender(in IHittable world)
		{
			Initialize();

			protectedImage = new(Width, Height);

			TimerCallback updatePixCount = (_) =>
			{
				int pixCount;
				if (imageMutex.WaitOne(100))
				{
					pixCount = pixelsDone;
					imageMutex.ReleaseMutex();
					ConsoleManager.WritePercent(pixCount, Width * Height, 0);
				}
			};
			Timer pixelCountUpdate = new Timer(updatePixCount, null, 1000, 1000);

			DateTime start = DateTime.Now;

			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					Ray ray = GetRay(x, y);

					HittableList list = (HittableList)world;

					int x1 = x;
					int y1 = y;

					void callback(object? _)
					{
						ThreadRayColor(list, x1, y1);
					}
					ThreadPool.QueueUserWorkItem(callback);
				}
			}

			while (pixelsDone < Width * Height)
			{
				Thread.Yield();
			}

			TimeSpan renderTime = DateTime.Now - start;

			Thread.Sleep(200);

			Console.Clear();
			Console.WriteLine("Render finished!");
			Console.WriteLine("Total time for image, {0:hh}:{0:mm}:{0:ss}", renderTime);

			int timestamp = (int)(DateTime.Now - DateTime.UnixEpoch).TotalSeconds;
			protectedImage.SaveAsPng($"Output\\Trace-{timestamp}.png");
		}
		private void ThreadRayColor(in HittableList world, int x, int y)
		{
			Vec3 sample = new();
			// Pixel center
			for (int i = 0; i < SamplesPPixel; i++)
			{
				Ray ray = GetRay(x, y);
				sample += RayColor(ray, world, MaxDepth);
			}
			sample *= pixelSampleScale;
			sample = LinearToGamma(sample);

			if (imageMutex.WaitOne(1000))
			{
				protectedImage![x, y] = sample;
				pixelsDone++;

				imageMutex.ReleaseMutex();
			}
		}

		public void Render(in IHittable world)
		{
			Initialize();

			Image<Rgba32> image = new(Width, Height);

			protectedImage = new(Width, Height);

			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					ConsoleManager.WriteProgress(y + 1, Height, "lines");

					Vec3 sample = new();
					// Pixel center
					for (int i = 0; i < SamplesPPixel; i++)
					{
						Ray ray = GetRay(x, y);
						sample += RayColor(ray, world, MaxDepth);
					}
					sample *= pixelSampleScale;
					sample = LinearToGamma(sample);
					image[x, y] = sample;
				}
			}

			int timestamp = (int)(DateTime.Now - DateTime.UnixEpoch).TotalSeconds;
			image.SaveAsPng($"Output\\Trace-{timestamp}.png");
		}
		private Vec3 RayColor(in Ray ray, in IHittable world, int depth)
		{
			if (depth <= 0) return new Vec3();

			HitRecord rec = new();
			if (world.Hit(ray, ref rec, new Interval(0.001, double.PositiveInfinity)))
			{
				Ray scattered;
				Vec3 attentuation = new();
				if (rec.Material.Scatter(ray, rec, ref attentuation, out scattered))
				{
					return attentuation * RayColor(scattered, world, depth - 1);
				}
				Vec3 direction = Vec3.RandomUnit();
				return new Vec3();
			}

			// Lerp value based on ray direction y component
			Vec3 unitDirection = ray.Direction.Unit();
			double a = 0.7 * (unitDirection.Y + 1.0);

			// Lerp color
			Vec3 colorVec = (1.0 - a) * new Vec3(1, 1, 1) + a * new Vec3(0.5, 0.7, 1.0);
			return colorVec;
		}

		private Ray GetRay(int i, int j)
		{
			// Construct a ray from origin aimed at i, j with random sampling

			Vec3 offset = SampleSquare();
			Vec3 pixelSample = pixel00Location + (i + offset.X) * pixelDeltaU + (j + offset.Y) * pixelDeltaV;

			Vec3 rayOrigin = (DefocusAngle <= 0) ? center : DefocusDiskSample();
			Vec3 rayDirection = pixelSample - rayOrigin;
			
			return new Ray(rayOrigin, rayDirection);
		}

		private Vec3 SampleSquare()
		{
			return new Vec3(random.NextDouble() - 0.5, random.NextDouble() - 0.5, 0);
		}
		private Vec3 DefocusDiskSample()
		{
			Vec3 point = Vec3.RandomInDisk();
			return center + (point.X * defocusDiskU) + (point.Y * defocusDiskV);
		}

		private static double LinearToGamma(double linearComponent)
		{
			if (linearComponent > 0) return Sqrt(linearComponent);
			else return 0;
		}
		private Vec3 LinearToGamma(Vec3 v)
		{
			return new Vec3(LinearToGamma(v.X), LinearToGamma(v.Y), LinearToGamma(v.Z));
		}
	}
}

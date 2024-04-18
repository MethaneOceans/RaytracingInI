namespace Raytracing.Rendering
{
	using Math;

	using SixLabors.ImageSharp;
	using SixLabors.ImageSharp.PixelFormats;

	internal class Camera
	{
		// Image properties
		public int Width;
		public int Height;

		// Camera properties

		private Vec3 center;
		private Vec3 pixel00Location;
		private Vec3 pixelDeltaU;
		private Vec3 pixelDeltaV;


		public void Render(in IHittable world)
		{
			Initialize();

			Image<Rgba32> render = new (Width, Height);

			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					// Pixel center
					Vec3 pixelCenter = pixel00Location + x * pixelDeltaU + y * pixelDeltaV;
					Vec3 rayDirection = pixelCenter - center;
					Ray ray = new(center, rayDirection);

					render[x, y] = RayColor(ray, world);
				}
			}

			int timestamp = (int)(DateTime.Now - DateTime.UnixEpoch).TotalSeconds;
			render.SaveAsBmp($"Output\\Trace-{timestamp}.bmp");
		}

		private void Initialize()
		{
			center = new();

			// Set viewport dimensions;
			double aspectRatio = Width / (float)Height;
			double focalLength = 1.0;
			double viewportHeight = 2.0;
			double viewportWidth = viewportHeight * aspectRatio;

			// Calculate the vectors across the horizontal and vertical viewport edges
			Vec3 viewportU = new(viewportWidth, 0, 0);
			Vec3 viewportV = new(0, -viewportHeight, 0);

			// Calculate horizontal and vertical delta vectors from pixel to pixel.
			pixelDeltaU = viewportU / Width;
			pixelDeltaV = viewportV / Height;

			// Calculate position of upper left pixel.
			Vec3 viewportUpperLeft = center - new Vec3(0, 0, focalLength) - viewportU / 2 - viewportV / 2;
			pixel00Location = viewportUpperLeft + 0.5 * (pixelDeltaU + pixelDeltaV);
		}
		private Rgba32 RayColor(in Ray ray, in IHittable world)
		{
			Vec3 unitDirection = ray.Direction.Unit();

			HitRecord rec = new();
			if (world.Hit(ray, ref rec, new Interval(0, double.PositiveInfinity)))
			{
				return 0.5 * (rec.Normal + new Vec3(1, 1, 1));
			}

			// Lerp value based on ray direction y component
			double a = 0.5 * (unitDirection.Y + 1.0);
			// Lerp color
			Vec3 colorVec = (1.0 - a) * new Vec3(1, 1, 1) + a * new Vec3(0.5, 0.7, 1.0);

			return colorVec;
		}
	}
}

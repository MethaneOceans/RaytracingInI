using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing
{
	internal static class ConsoleManager
	{
		public static void WriteProgress(int currentLine, int totalLines)
		{
			const int barLength = 30;

			// Store cursor position
			(int oldCursorX, int oldCursorY) = Console.GetCursorPosition();

			// Go to start of console
			Console.SetCursorPosition(0, 0);

			int progress = (int)((currentLine / (float)totalLines) * barLength) + 1;

			string progressBar = new StringBuilder(progress).Insert(0, "#", progress).Insert(progress, "_", barLength - progress).ToString();

			Console.WriteLine("Progress {0}/{1} scanlines [{2}]", currentLine, totalLines, progressBar);

			// Restore cursor position
			Console.SetCursorPosition(oldCursorX, oldCursorY);
		}
	}
}

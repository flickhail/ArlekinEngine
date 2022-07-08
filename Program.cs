using System;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

using ArlekinEngine;

namespace Program;

class Program
{
	static void Main()
	{
		Console.Title = "Arlekin.Debug";
		
		var nativeWinSettings = new NativeWindowSettings()
		{
			Size = new Vector2i(800, 600),
			Title = "Arlekin.Window",
		};

		using (var mainWindow = new MainWindow(GameWindowSettings.Default, nativeWinSettings))
        {
			mainWindow.Run();
        }
	}
}


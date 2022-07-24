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
			WindowState = WindowState.Fullscreen,
			
		};

		var gameWinSettings = new GameWindowSettings()
		{
			//RenderFrequency = 60.0f,
		};

		//using (var mainWindow = new MainWindow(GameWindowSettings.Default, nativeWinSettings))
		using(var window2 = new Window2(GameWindowSettings.Default, nativeWinSettings))
        {
			//mainWindow.Run();
			window2.Run();
        }
	}
}


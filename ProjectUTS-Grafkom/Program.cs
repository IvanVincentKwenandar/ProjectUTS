using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace ConsoleApp4
{
	class Program
	{
		static void Main(string[] args)
		{
			var ourWindow = new NativeWindowSettings()
			{
				Size = new Vector2i(1000, 1000),
				Title = "ConsoleApp4"
			};

			using (var window = new Window(GameWindowSettings.Default, ourWindow))
			{
				window.Run();
			}
		}
	}
}

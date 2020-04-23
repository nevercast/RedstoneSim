using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Desktop;
using Redstonesim;

namespace RedstoneSim
{
    public static class Program
    {
        private static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "LearnOpenTK - Colors",
                StartFocused = false,
            };

            using (var window = new Engine(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}

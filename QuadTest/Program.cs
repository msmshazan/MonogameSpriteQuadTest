using System;

namespace QuadTest
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new QuadSpriteTransformTest())
                game.Run();
        }
    }
}

using System;

namespace Galaga
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Galaga game = new Galaga())
            {
                game.Run();
            }
        }
    }
#endif
}


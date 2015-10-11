using System;

namespace AlphaQuadrant
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (AlphaQuadrant game = new AlphaQuadrant())
            {
                game.Run();
            }
        }
    }
#endif
}


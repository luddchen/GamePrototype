using System;

namespace Battlestation_Antares
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// Main function of Antares
        /// </summary>
        static void Main(string[] args)
        {
            using (Antares game = new Antares())
            {
                game.Run();
            }
        }
    }
#endif
}


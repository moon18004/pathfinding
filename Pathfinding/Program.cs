using System;

namespace Pathfinding
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            Player player = new Player();
            board.Initialize(25, player);       // make a board of size 25
            player.Initialize(1, 1, board);     // make a player and starts at (1, 1)
            Console.CursorVisible = false;

            const int WAIT_TICK = 1000 / 30;

            int lastTick = 0;
            while (true)
            {
                #region Frame manager
                int currentTick = System.Environment.TickCount;   // measure the time passed
                if (currentTick - lastTick < WAIT_TICK)
                    continue;
                int deltaTick = currentTick - lastTick;
                lastTick = currentTick;
                #endregion 
                

                // Logic
                player.Update(deltaTick);

                // Rendering
                Console.SetCursorPosition(0, 0);
                board.Render();
            }

        }
    }
}

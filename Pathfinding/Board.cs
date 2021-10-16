using System;
using System.Collections.Generic;
using System.Text;

namespace Pathfinding
{

    class Board
    {
        const char CIRCLE = '\u25cf';
        public TileType[,] Tile { get; private set; }
        public int Size { get; private set; }
        public int DestY { get; private set; }
        public int DestX { get; private set; }


        Player _player;

        public enum TileType
        {
            Empty,         // can move
            Wall           // cannot move
        }

        public void Initialize(int size, Player player)
        {
            if (size % 2 == 0)
                return;

            _player = player;

            Tile = new TileType[size, size];
            Size = size;

            DestY = Size - 2;
            DestX = Size - 2;

            CreateMap();
        }

        void CreateMap()
        {
            // make even coordinates to wall
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)      // make even coordinates to wall
                    {
                        Tile[y, x] = TileType.Wall;
                    }
                    else
                    {
                        Tile[y, x] = TileType.Empty;
                    }
                }
            }

            // Randomly make wall to the empty to the right or down
            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                int count = 1;
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)           // if the position is even coordinate,
                        continue;

                    if (y == Size - 2 && x == Size - 2)     // if the position is destination,
                        continue;

                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;    // make right position empty
                        continue;
                    }
                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;    // make down position empty
                        continue;
                    }

                    if (rand.Next(0, 2) == 0)
                    {
                        Tile[y, x + 1] = TileType.Empty;    // make right position empty
                        count++;
                    }
                    else
                    {
                        int randomIndex = rand.Next(0, count);
                        Tile[y + 1, x - randomIndex * 2] = TileType.Empty;  // at random x position, make down position empty
                        count = 1;
                    }
                }
            }
        }

        public void Render()
        {
            ConsoleColor prevColor = Console.ForegroundColor;
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    // if the player's position is same with the y, x position make the color for the player
                    if (y == _player.PosY && x == _player.PosX)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;     // Player color: Blue
                    }
                    else if (y == DestY && x == DestX)
                        Console.ForegroundColor = ConsoleColor.Yellow;   // destination color : Yellow
                    else
                    {
                        Console.ForegroundColor = GetTileColor(Tile[y, x]);
                    }

                    Console.Write(CIRCLE);
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = prevColor;
        }


        ConsoleColor GetTileColor(TileType type)
        {
            switch (type)
            {
                case TileType.Empty:
                    return ConsoleColor.Green;    // Empty color : Green

                case TileType.Wall:
                    return ConsoleColor.Red;      // Wall color : Red

                default:
                    return ConsoleColor.Green;
            }
        }
    }
}
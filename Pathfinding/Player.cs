using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Pathfinding
{
    class Pos
    {
        public Pos(int y, int x) { Y = y; X = x; }
        public int Y;
        public int X;
    }

    class Player
    {
        public int PosY { get; private set; }
        public int PosX { get; private set; }
        Random _random = new Random();

        Board _board;

        enum Dir
        {
            Up = 0,
            Left = 1,
            Down = 2,
            Right = 3
        }
        int _dir = (int)Dir.Up;
        List<Pos> _points = new List<Pos>();
        public void Initialize(int posY, int posX, Board board)
        {
            PosY = posY;
            PosX = posX;
            _board = board;

            AStar();
        }

        struct PQNode : IComparable<PQNode>
        {
            public int F;
            public int G;
            public int Y;
            public int X;

            public int CompareTo(PQNode other)
            {
                if (F == other.F)
                    return 0;
                return F < other.F ? 1 : -1;

            }
        }

        void AStar()
        {
            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1 };
            int[] cost = new int[] { 1, 1, 1, 1 };

            // F = G + HTotal
            // G = the distance from the starting position to the selected position
            // H = the distance to the destination

            //(y, x) if already visited,  visited = closed(true)
            bool[,] closed = new bool[_board.Size, _board.Size];

            // if find the way to (y, x) at least once
            // did not find => maxValue
            // found => F = G + H
            int[,] open = new int[_board.Size, _board.Size];
            for (int y = 0; y < _board.Size; y++)
                for (int x = 0; x < _board.Size; x++)
                    open[y, x] = Int32.MaxValue;

            // store the previous position
            Pos[,] parent = new Pos[_board.Size, _board.Size];

            // finds the fastest path
            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();

            // begin from the starting position(1,1)
            open[PosY, PosX] = Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX);
            pq.Push(new PQNode() { F = Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX), G = 0, Y = PosY, X = PosX });
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while (pq.Count > 0)
            {
                // store the best path
                PQNode node = pq.Pop();
                
                if (closed[node.Y, node.X]) // if it is already visited
                    continue;

                closed[node.Y, node.X] = true;
                // if it arrived, 
                if (node.Y == _board.DestY && node.X == _board.DestX)
                    break;

                // check if it is possible to move up, down, left, and right
                for (int i = 0; i < deltaY.Length; i++)
                {
                    int nextY = node.Y + deltaY[i];
                    int nextX = node.X + deltaX[i];

                    // check if it is our of the size of the board
                    if (nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size)
                        continue;
                    // if it is wall
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    // if already visited
                    if (closed[nextY, nextX])
                        continue;

                    // calculate the distance
                    int g = node.G + cost[i];
                    int h = Math.Abs(_board.DestY - nextY) + Math.Abs(_board.DestX - nextX);


                    // store the visited position
                    open[nextY, nextX] = g + h;
                    pq.Push(new PQNode() { F = g + h, G = g, Y = nextY, X = nextX });
                    parent[nextY, nextX] = new Pos(node.Y, node.X);

                }

            }
            CalcPathFromParent(parent);

        }

        // Find the path from the destination to the stating point in the parent array
        void CalcPathFromParent(Pos[,] parent)
        {
            int y = _board.DestY;
            int x = _board.DestX;
            while (parent[y, x].Y != y || parent[y, x].X != x)
            {
                _points.Add(new Pos(y, x));
                Pos pos = parent[y, x];
                y = pos.Y;
                x = pos.X;
            }
            _points.Add(new Pos(y, x));
            _points.Reverse();        // It is from the destination to starting point, so reverse
        }


        const int MOVE_TICK = 30;
        int _sumTick = 0;
        int _lastIndex = 0;
        public void Update(int deltaTick)
        {
            if (_lastIndex >= _points.Count)
            {
                _lastIndex = 0;
                _points.Clear();
                _board.Initialize(_board.Size, this);
                Initialize(1, 1, _board);
            }
            _sumTick += deltaTick;
            if (_sumTick >= MOVE_TICK)
            {
                _sumTick = 0;

                PosY = _points[_lastIndex].Y;
                PosX = _points[_lastIndex].X;
                _lastIndex++;
            }
        }
    }
}
using System.Diagnostics;
using static AdventOfCode2022.utils.Utils;

namespace AdventOfCode2022.day9
{
    [ProblemDay("day9")]
    [ReturnType(typeof(int), typeof(int))]
    [TestResult(88, 36)]
    internal class Solver : Problem
    {

        struct Position
        {
            public int x = 0, y = 0;

            public Position() { }
            public Position(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public override string ToString()
            {
                return $"{x}-{y}";
            }
        }

        enum Direction
        {
            L, R, U, D
        }

        int lowestY = -20;
        int lowestX = -20;
        int greatestY = 20;
        int greatestX = 20;

        private void UpdateTails(ref List<Position> tails, ref Position headPos, ref Position lastHeadPos, HashSet<string> visited)
        {
            int xMove = 0, yMove = 0;
            for (int i = 0; i < tails.Count(); i++)
            {
                if (i == 0)
                {
                    if (!IsNear(headPos, tails[i]))
                    {
                        xMove = lastHeadPos.x - tails[i].x;
                        yMove = lastHeadPos.y - tails[i].y;
                        tails[i] = lastHeadPos;
                        if (tails.Count() == 1) visited.Add(tails[i].ToString());
                    }
                }
                else
                {
                    if (!IsNear(tails[i - 1], tails[i]))
                    {
                        Position movementVector = GetMovementVector(tails[i-1], tails[i]);
                        tails[i] = new Position()
                        {
                            x = tails[i].x + movementVector.x,
                            y = tails[i].y + movementVector.y
                        };
                    }
                }
                if (i == tails.Count() - 1) visited.Add(tails[i].ToString());
            }
#if DEBUG
            //PrintTails(ref tails);
#endif
        }

        public override T Solve<T>(ProblemChoice pc)
        {
            HashSet<string> visited = new();
            List<string> lines = ReadLinesAs<string>();
            Position headPos = new();
            List<Position> tails = new();
            if (pc == ProblemChoice.A)
            {
                tails.Add(new Position());
            } else
            {
                for (int i = 0; i < 9; i++)
                {
                    tails.Add(new Position());
                }
            }
            Position lastHeadPos = new();
            visited.Add("0-0");
            foreach (string line in lines)
            {
                Enum.TryParse<Direction>(GetCharsAs<string>(line, 0, 1), false, out Direction dir);
                (int quantity, _) = GetUntilSpaceAs<int>(line, 2);
#if DEBUG
                //if (pc == ProblemChoice.B) Console.WriteLine($"\n== {line} ==\n");
#endif
                if (dir == Direction.L)
                {
                    for (int i = 0; i < quantity; i++)
                    {
#if DEBUG
                        //if (pc == ProblemChoice.B) Print(ref tails, ref headPos);
#endif
                        lastHeadPos = headPos;
                        headPos.x--;
                        UpdateTails(ref tails, ref headPos, ref lastHeadPos, visited);
                    }
                } else if (dir == Direction.R )
                {
                    for (int i = 0; i < quantity; i++)
                    {
#if DEBUG
                        //if (pc == ProblemChoice.B) Print(ref tails, ref headPos);
#endif
                        lastHeadPos = headPos;
                        headPos.x++;
                        UpdateTails(ref tails, ref headPos, ref lastHeadPos, visited);
                    }
                } else if (dir == Direction.U)
                {
                    for (int i = 0; i < quantity; i++)
                    {
#if DEBUG
                        //if (pc == ProblemChoice.B) Print(ref tails, ref headPos);
#endif
                        lastHeadPos = headPos;
                        headPos.y--;
                        UpdateTails(ref tails, ref headPos, ref lastHeadPos, visited);
                    }
                } else if (dir == Direction.D)
                {
                    for (int i = 0; i < quantity; i++)
                    {
#if DEBUG
                        //if (pc == ProblemChoice.B) Print(ref tails, ref headPos);
#endif
                        lastHeadPos = headPos;
                        headPos.y++;
                        UpdateTails(ref tails, ref headPos, ref lastHeadPos, visited);
                    }
                }

            }

            return Cast<T>(visited.Count());
        }

        private bool IsNear(Position head, Position tail)
        {
            if (Math.Abs(head.x - tail.x) < 2 && Math.Abs(head.y - tail.y) < 2) return true;
            return false;
        }

        private Position GetMovementVector(Position head, Position tail)
        {
            return new Position()
            {
                x = Math.Clamp(head.x - tail.x, -1, 1),
                y = Math.Clamp(head.y - tail.y, -1, 1)
            };
            
        }

        private void Print(ref List<Position> tails, ref Position head)
        {
            HashSet<Position> cellsPainted = new();
            for (var row = lowestY; row < greatestY; row++)
            {
                for (var col = lowestX; col < greatestX; col++)
                {
                    bool painted = false;
                    if (head.x == col && head.y == row && !cellsPainted.Contains(new Position(col, row)))
                    {
                        Console.Write("H");
                        painted = true;
                        cellsPainted.Add(new Position(col, row));
                    }
                    for (int i = 0; i < tails.Count(); i++)
                    {
                        if (tails[i].x == col && tails[i].y == row && !cellsPainted.Contains(new Position(col, row)))
                        {
                            Console.Write(i+1);
                            painted = true;
                            cellsPainted.Add(new Position(col, row));
                        }
                    }
                    if (!painted) Console.Write(".");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}

using static AdventOfCode2022.utils.Utils;

namespace AdventOfCode2022.day8
{
    [ProblemDay("day8")]
    [ReturnType(typeof(int), typeof(int))]
    [TestResult(21)]
    internal class Solver : Problem
    {

        enum Direction
        {
            LEFT,
            RIGHT,
            UP,
            DOWN
        }

        public override T Solve<T>(ProblemChoice pc)
        {
            if (pc == ProblemChoice.B) throw new NotImplementedException();
            List<string> lines = ReadLinesAs<string>();

            int row = 0;
            int finalColumn = lines[0].Length - 1;
            int finalRow = lines.Count - 1;
            int visibles = 0;
            foreach (var line in lines)
            {
                int column = 0;
                foreach (int tree in ConvertCharArray<int>(line.ToCharArray()))
                {
                    if (row == 0 || row == finalRow || column == 0 || column == finalColumn)
                    {
                        visibles++;
                    } else
                    {
                        bool visibleLeft = true;
                        bool visibleRight = true;
                        bool visibleUp = true;
                        bool visibleDown = true;
                        // check horizontal
                        for (int i = 0; i <= finalRow; i++)
                        {
                            if (i == column) continue;
                            int treeToCompare = Cast<int>(lines[row][i]);
                            Direction dir = i < column ? Direction.LEFT : Direction.RIGHT;
                        }

                        // check vertical
                        for (int i = 0; i <= finalColumn; i++)
                        {
                            if (i == row) continue;
                            int treeCompare = Cast<int>(lines[i][column]);
                            Direction dir = i < row ? Direction.UP : Direction.DOWN;
                        }
                    }
                    column++;
                }

                row++;
            }

            return Cast<T>(0);
        }
    }
}

using static AdventOfCode2022.utils.Utils;

namespace AdventOfCode2022.day8
{
    [ProblemDay("day8")]
    [ReturnType(typeof(int), typeof(int))]
    [TestResult(21, 8)]
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
            List<string> lines = ReadLinesAs<string>();

            int row = 0;
            int finalColumn = lines[0].Length - 1;
            int finalRow = lines.Count - 1;
            int visibles = 0;
            int scenicPoints = 0;
            foreach (var line in lines)
            {
                int column = 0;
                foreach (int tree in ConvertCharArray<int>(line.ToCharArray()))
                {
                    int countLeft = 0;
                    int countRight = 0;
                    int countUp = 0;
                    int countDown = 0;
                    if (row == 0 || row == finalRow || column == 0 || column == finalColumn)
                    {
                        visibles++;
                    } else
                    {
                        bool visibleLeft = true;
                        bool visibleRight = true;
                        bool visibleUp = true;
                        bool visibleDown = true;

                        if (pc == ProblemChoice.A)
                        {
                            // check horizontal
                            for (int i = 0; i <= finalRow; i++)
                            {
                                if (i == column) continue;
                                int treeToCompare = Cast<int>(lines[row][i]);
                                Direction dir = i < column ? Direction.LEFT : Direction.RIGHT;
                                if (Direction.LEFT == dir && treeToCompare >= tree)
                                {
                                    visibleLeft = false;
                                } else if (Direction.RIGHT == dir && treeToCompare >= tree)
                                {
                                    visibleRight = false;
                                }
                            }

                            // check vertical
                            for (int i = 0; i <= finalColumn; i++)
                            {
                                if (i == row) continue;
                                int treeToCompare = Cast<int>(lines[i][column]);
                                Direction dir = i < row ? Direction.UP : Direction.DOWN;
                                if (Direction.UP == dir && treeToCompare >= tree)
                                {
                                    visibleUp = false;
                                } else if (Direction.DOWN == dir && treeToCompare >= tree)
                                {
                                    visibleDown = false;
                                }
                            }
                            if (visibleLeft || visibleRight || visibleUp || visibleDown) visibles++;
                        } else
                        {
                            // check left
                            for (int i = column - 1; i >= 0; i--)
                            {
                                int treeToCompare = Cast<int>(lines[row][i]);
                                if (treeToCompare >= tree)
                                {
                                    countLeft++;
                                    break;
                                }
                                countLeft++;
                            }

                            // check right
                            for (int i = column + 1; i <= finalColumn; i++)
                            {
                                int treeToCompare = Cast<int>(lines[row][i]);
                                if (treeToCompare >= tree)
                                {
                                    countRight++;
                                    break;
                                }
                                countRight++;
                            }

                            // check up
                            for (int i = row - 1; i >= 0; i--)
                            {
                                int treeToCompare = Cast<int>(lines[i][column]);
                                if (treeToCompare >= tree)
                                {
                                    countUp++;
                                    break;
                                }
                                countUp++;
                            }

                            // check down
                            for (int i = row + 1; i <= finalRow; i++)
                            {
                                int treeToCompare = Cast<int>(lines[i][column]);
                                if (treeToCompare >= tree)
                                {
                                    countDown++;
                                    break;
                                }
                                countDown++;
                            }
                        }

                    }
                    column++;
                    int treePoints = countUp * countDown * countLeft * countRight;
                    if (scenicPoints < treePoints) scenicPoints = treePoints;

                }

                row++;
            }

            if (pc == ProblemChoice.A) return Cast<T>(visibles);
            return Cast<T>(scenicPoints);
        }
    }
}

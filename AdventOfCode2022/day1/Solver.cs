using static AdventOfCode2022.utils.Utils;

namespace AdventOfCode2022.day1
{
    [ProblemDay("day1")]
    [ReturnType(typeof(int), typeof(int))]
    [TestResult(24000, 45000)]
    internal class Solver : Problem
    {
        public override T Solve<T>(ProblemChoice pc)
        {
            var lines = ReadLinesAs<uint>();
            uint sum = 0;
            List<uint> sumsList = new List<uint>(lines.Count);
            foreach (var line in lines)
            {
                if (line > 0)
                {
                    sum += line;
                }
                else
                {
                    sumsList.Add(sum);
                    sum = 0;
                }
            }
            sumsList.Add(sum);
            sumsList.Sort((a, b) => b.CompareTo(a));
            if (pc == ProblemChoice.A)
            {
                return Cast<T>(sumsList.Take(1).Sum(d => d));
            } else
            {
                return Cast<T>(sumsList.Take(3).Sum(d => d));
            }
        }
    }
}


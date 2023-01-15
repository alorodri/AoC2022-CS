using static AdventOfCode2022.utils.Utils;

namespace AdventOfCode2022.day5
{
    [ProblemDay("day5")]
    [ReturnType(typeof(string), typeof(string))]
    [TestResult("CMZ")]
    internal class Solver : Problem
    {
        public override T Solve<T>(ProblemChoice pc)
        {
            if (pc == ProblemChoice.B) throw new NotImplementedException();
            List<string> lines = ReadLinesAs<string>();
            List<List<string>> crates = new();
            List<string> orders = new();
            foreach (string line in lines)
            {
                int ptr = 0;
                while (ptr < line.Length)
                {
                    if (crates.Count < ptr / 4 + 1)
                    {
                        crates.Add(new List<string>());
                    }
                    var part = GetCharsAs<string>(line, ptr, 4);
                    if (part.Length == 0)
                    {
                        // just a space
                    } else if (part.Length == 1)
                    {
                        // id

                    } else if(part.StartsWith('['))
                    {
                        // crate
                        crates[ptr / 4].Add(part.Substring(1, 1));
                    } else
                    {
                        // move order
                        orders.Add(line);
                        break;
                    }
                    ptr += 4;
                }
            }

            foreach (var crate in crates) crate.Reverse();

            foreach (string order in orders)
            {
                var (quantity, ptr) = GetUntilSpaceAs<int>(order, 5);
                var (from, ptr2) = GetUntilSpaceAs<int>(order, ptr + 4);
                var (to, _)= GetUntilSpaceAs<int>(order, ptr2 + 2);

                for (int i = 0;  i < quantity; i++)
                {
                    var innerCrate = crates[from - 1];
                    var item = innerCrate[innerCrate.Count - 1];
                    innerCrate.RemoveAt(innerCrate.Count - 1);
                    crates[to - 1].Add(item);
                }
            }

            string result = string.Empty;
            foreach (var crate in crates)
            {
                result += crate[crate.Count - 1];
            }

            return Cast<T>(result);
        }
    }
}

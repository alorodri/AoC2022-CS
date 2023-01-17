using static AdventOfCode2022.utils.Utils;

namespace AdventOfCode2022.day6
{
    [ProblemDay("day6")]
    [ReturnType(typeof(int), typeof(int))]
    [TestResult(11, 26)]
    internal class Solver : Problem
    {

        public override T Solve<T>(ProblemChoice pc)
        {
            List<string> lines = ReadLinesAs<string>();
            var line = lines[0];

            int messageMarkerLength = pc == ProblemChoice.A ? 4 : 14;
            for (int i = 0; i < line.Length; i++)
            {
                HashSet<char> chars = line.Skip(i).Take(messageMarkerLength).ToHashSet();
                if (chars.Count == messageMarkerLength) return Cast<T>(i + messageMarkerLength);
            }

            // old code, dunno why got 1767 instead of 1766 (right solution for A)
            /*Queue<char> buffer = new();
            buffer.EnsureCapacity(messageMarkerLength);
            uint index = 0;
            List<char> repeatedCache = new();
            foreach (char c in line)
            {
                index++;
                buffer.Enqueue(c);
                if (buffer.Count == messageMarkerLength + 1)
                {
                    char removed = buffer.Dequeue();
                    if (repeatedCache.Contains(removed)) repeatedCache.Remove(repeatedCache.First(c => c == removed));
                }

                foreach (char q in buffer.Take(buffer.Count - 1))
                {
                    if (q == c) repeatedCache.Add(c);
                }

                if (buffer.Count == messageMarkerLength)
                {
                    if (repeatedCache.Count == 0) return Cast<T>(index);
                }

            }*/

            return Cast<T>(0);
        }
    }
}

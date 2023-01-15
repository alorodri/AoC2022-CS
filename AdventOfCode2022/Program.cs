using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2022
{
    internal class Program
    {
        [RequiresDynamicCode("Calls AdventOfCode2022.utils.Utils.Problem.PrintResults()")]
        static void Main()
        {
            utils.Utils.ExecuteAll();
        }
    }
}
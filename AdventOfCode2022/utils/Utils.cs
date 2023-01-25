using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace AdventOfCode2022.utils
{
    internal class Utils
    {

        public static void ExecuteAll()
        {
            List<Problem> problems = new List<Problem>()
            {
                new day1.Solver(),
                new day5.Solver(),
                new day6.Solver(),
                new day7.Solver(),
                new day8.Solver(),
                new day9.Solver(),
                new day10.Solver(),
            };

            foreach (var problem in problems) problem.PrintResults();
        }

        public static T Cast<T>(object obj)
        {
            if (typeof(T) == typeof(uint))
            {
                if (obj.GetType() == typeof(string) && "".Equals(obj))
                {
                    return (T)Convert.ChangeType(0, typeof(T));
                }
            }

            return (T)Convert.ChangeType(obj, typeof(T));
        }

        public static T[] ConvertCharArray<T>(char[] array)
        {
            T[] arr = new T[array.Length];
            for(int i = 0; i < array.Length; i++)
            {
                arr[i] = Cast<T>(array[i]);
            }
            return arr;
        }

        public static T GetCharsAs<T>(string str, int skip, int take)
        {
            var r = string.Join("", str.Skip(skip).Take(take));
            r = string.Concat(r.Where(c => !Char.IsWhiteSpace(c)));
            return Cast<T>(r);
        }

        public static (T, int) GetUntilSpaceAs<T>(string str, int skip)
        {
            StringBuilder sb = new StringBuilder();
            var chars = str.Skip(skip);
            var ptr = skip;
            bool foundChar = false;
            bool endedStr = false;
            foreach (var c in chars)
            {
                if (!Char.IsWhiteSpace(c))
                {
                    if (foundChar && endedStr) break;
                    sb.Append(c);
                    foundChar = true;
                } else
                {
                    if (foundChar) endedStr = true;
                }
                ptr++;
            }

            return (Cast<T>(sb.ToString()), ptr);
        }

        public enum ProblemChoice
        {
            A,
            B
        }

        public enum ProblemType
        {
            TEST,
            INPUT
        }

        [AttributeUsage(AttributeTargets.Class)]
        public class ProblemDay : Attribute
        {
            public string Day { get; set; }
            public ProblemDay (String day) { this.Day = day; }
        }

        [AttributeUsage(AttributeTargets.Class)]
        public class ReturnType : Attribute
        {
            public Type Type { get; set; }
            public Type Type2 { get; set; }
            public ReturnType (Type type, Type type2)
            {
                Type = type;
                Type2 = type2;
            }
        }

        [AttributeUsage(AttributeTargets.Class)]
        public class TestResult : Attribute
        {
            public object ResultA { get; set; }
            public object? ResultB { get; set; }
            public TestResult(object ResultA) { this.ResultA = ResultA; }
            public TestResult(object resultA, object resultB)
            {
                ResultA = resultA;
                ResultB = resultB;
            }
        }

        internal abstract class Problem
        {
            private ProblemType ProblemType { get; set; }

            public Problem()
            {
                ProblemType = ProblemType.TEST;
            }

            public abstract T Solve<T>(ProblemChoice pc);
            public List<T> ReadLinesAs<T>()
            {
                ProblemDay attrProblemDay = (ProblemDay)Attribute.GetCustomAttribute(GetType(), typeof(ProblemDay));
                if (attrProblemDay == null) throw new Exception("ProblemDay attribute needed");
                string day = attrProblemDay.Day;
                string filename = ProblemType == ProblemType.TEST ? "test.txt" : "input.txt";
                string projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                List<T> lines = new List<T>();
                string path = Path.Combine(Path.GetDirectoryName(projectFolder), $"inputs/{day}/{filename}");
                foreach (var line in File.ReadLines(path))
                {
                    lines.Add(Cast<T>(line));
                }
                return lines;
            }

            [RequiresDynamicCode("Calls System.Reflection.MethodInfo.MakeGenericMethod(params Type[])")]
            public void PrintResults()
            {
                ProblemDay attrProblemDay = (ProblemDay)Attribute.GetCustomAttribute(GetType(), typeof(ProblemDay));
                if (attrProblemDay == null) throw new Exception("ProblemDay attribute needed");
                string day = attrProblemDay.Day;
                Console.WriteLine($"- {day.ToUpper()} ---------------------------------------------------");
                bool passedTests = ExecuteTest();

                if ( passedTests )
                {
                    ReturnType attrReturnType = (ReturnType)Attribute.GetCustomAttribute(GetType(), typeof(ReturnType)) ?? throw new Exception("ReturnType attribute needed");
                    Type type1 = attrReturnType.Type;
                    Type type2 = attrReturnType.Type2;
                    var solveMethod = GetType().GetMethod("Solve") ?? throw new Exception("Method Solve isn't defined");
                    ProblemType = ProblemType.INPUT;
                    var chrono = Stopwatch.StartNew();
                    chrono.Start();
                    try
                    {
                        var result1 = solveMethod.MakeGenericMethod(type1).Invoke(this, new object[] { ProblemChoice.A });
                        var elapsed1 = chrono.Elapsed.TotalMilliseconds;
                        Console.WriteLine("[PROBLEM 1] = {0, -10} -- {1, 5} ms", result1, elapsed1);
                    } catch (TargetInvocationException ex)
                    {
                        if (ex.InnerException == null) throw ex;

                        if (ex.InnerException.GetType() == typeof(NotImplementedException))
                        {
                            Console.WriteLine("[PROBLEM 1] = Not implemented");
                        }
                        else
                        {
                            throw ex.InnerException;
                        }
                    }
                    chrono.Restart();
                    try
                    {
                        var result2 = solveMethod.MakeGenericMethod(type2).Invoke(this, new object[] { ProblemChoice.B });
                        var elapsed2 = chrono.Elapsed.TotalMilliseconds;
                        Console.WriteLine("[PROBLEM 2] = {0, -10} -- {1, 5} ms", result2, elapsed2);
                    } catch (TargetInvocationException ex)
                    {
                        if (ex.InnerException == null) throw ex;

                        if (ex.InnerException.GetType() == typeof(NotImplementedException))
                        {
                            Console.WriteLine("[PROBLEM 2] = Not implemented");
                        }
                        else
                        {
                            throw ex.InnerException;
                        }
                    }
                    chrono.Stop();
                }

                Console.WriteLine("----------------------------------------------------------");
            }

            private bool ExecuteTest()
            {
                TestResult attrTestResult = (TestResult)Attribute.GetCustomAttribute(GetType(), typeof(TestResult));
                if (attrTestResult == null) return true;
                object resultA = attrTestResult.ResultA;
                object? resultB = attrTestResult.ResultB;
                ReturnType attrReturnType = (ReturnType)Attribute.GetCustomAttribute(GetType(), typeof(ReturnType)) ?? throw new Exception("ReturnType attribute needed");
                Type type1 = attrReturnType.Type;
                Type type2 = attrReturnType.Type2;
                try
                {
                    var solveMethod = GetType().GetMethod("Solve") ?? throw new Exception("Method Solve isn't defined");
                    var result1 = solveMethod.MakeGenericMethod(type1).Invoke(this, new object[] { ProblemChoice.A });
                    var (testMessage, passed) = TestMessageValues(result1, resultA);
                    Console.WriteLine("[TEST A] = {0}", testMessage);

                    bool testsApproved = passed;

                    if (resultB != null)
                    {
                        var result2 = solveMethod.MakeGenericMethod(type2).Invoke(this, new object[] { ProblemChoice.B });
                        (testMessage, passed) = TestMessageValues(result2, resultB);
                        Console.WriteLine("[TEST B] = {0}", testMessage);
                        testsApproved = testsApproved && passed;
                    }

                    return testsApproved;
                } catch (TargetInvocationException ex)
                {
                    if (ex.InnerException == null) throw ex;

                    if (ex.InnerException.GetType() == typeof(NotImplementedException)) {
                        Console.WriteLine("[TEST] Solve method not implemented");
                        return false;
                    } else
                    {
                        throw ex.InnerException;
                    }
                }
            }

            private (string, bool) TestMessageValues(object value1, object value2)
            {
                return value1?.Equals(value2) ?? value2 == null ? ($"Test passed, received {value1}", true) : ($"Test failed. Received {value1}, expected {value2}", false);
            }
        }
    }
}

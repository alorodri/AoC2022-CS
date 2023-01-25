using System.Text;
using static AdventOfCode2022.utils.Utils;

namespace AdventOfCode2022.day10
{
    [ProblemDay("day10")]
    [ReturnType(typeof(int), typeof(string))]
    [TestResult(13140, B_TEST_RESULT)]
    internal class Solver : Problem
    {

        const string B_TEST_RESULT =    "##..##..##..##..##..##..##..##..##..##..\r\n" +
                                        "###...###...###...###...###...###...###.\r\n" +
                                        "####....####....####....####....####....\r\n" +
                                        "#####.....#####.....#####.....#####.....\r\n" +
                                        "######......######......######......####\r\n" +
                                        "#######.......#######.......#######.....";

        public override T Solve<T>(ProblemChoice pc)
        {
            List<string> lines = ReadLinesAs<string>();
            CPU cpu = new();
            cpu.AddTickEventA(20);
            cpu.AddTickEventA(60);
            cpu.AddTickEventA(100);
            cpu.AddTickEventA(140);
            cpu.AddTickEventA(180);
            cpu.AddTickEventA(220);

            cpu.AddTickEventB(40);
            cpu.AddTickEventB(80);
            cpu.AddTickEventB(120);
            cpu.AddTickEventB(160);
            cpu.AddTickEventB(200);
            cpu.AddTickEventB(240);
            foreach (string line in lines)
            {
                Enum.TryParse<Command>(line, out var cmd);
                switch (cmd)
                {
                    case Command.noop:
                        cpu.IncrementTick();
                        break;
                    case Command.addx:
                        (int value, _) = GetUntilSpaceAs<int>(line, 5);
                        cpu.IncrementTick();
                        cpu.IncrementTick();
                        cpu.XRegister += value;
                        break;
                    default: throw new Exception("Command not supported");
                }
            }

            if (pc == ProblemChoice.A) return Cast<T>(cpu.Result); else return Cast<T>(cpu.LCDMessage.ToString().TrimEnd());
        }

        enum Command
        {
            addx,
            noop
        }

        public class CPU
        {
            public int XRegister { get; set; }
            public int Result { get; set; }
            public uint Tick { get; private set; }
            private List<TickEvent> TickEvents { get; }
            public StringBuilder LCDMessage { get; private set; }
            public CPU()
            {
                XRegister = 1;
                Tick = 0;
                TickEvents = new();
                LCDMessage = new StringBuilder();
            }

            public void IncrementTick()
            {
                Tick++;
                BuildLCDMessage();
                foreach (TickEvent e in  TickEvents)
                {
                    if (e.TickToActivate == Tick) e.Event();
                }
            }

            private void BuildLCDMessage()
            {
                var LCDRegisters = new int[]
                {
                XRegister-1, XRegister, XRegister+1
                };
                if (LCDRegisters.Contains((int)(Tick-1)%40)) LCDMessage.Append("#"); else LCDMessage.Append(".");
            }

            public void AddTickEventA(uint tickTrigger)
            {
                TickEvent e = new(tickTrigger, () => Result += (int)(XRegister * tickTrigger));
                TickEvents.Add(e);
            }

            public void AddTickEventB(uint tickTrigger)
            {
                TickEvent e = new(tickTrigger, () => LCDMessage.Append("\r\n"));
                TickEvents.Add(e);
            }
        }

        class TickEvent
        {
            public uint TickToActivate { get; set; }
            public Action Event { get; set; }
            public TickEvent(uint tickToActivate, Action e) { 
                TickToActivate = tickToActivate;
                Event = e;
            }
        }
    }
}

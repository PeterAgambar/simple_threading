using System;
using System.Diagnostics;

namespace POC.Threading
{
    /// <summary>
    /// A simple three thread example.  each thread must complete a count before the program ends
    /// </summary>
    internal class SimpleThread
    {
        private readonly object _consoleLock = new object();
        public SimpleThread() {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            int seconds = 0;

            // Declare the tasks outside the loop to then use inside
            Task taskA = TaskA();
            Task taskB = TaskB();
            Task taskC = TaskC();

            while (true)
            {
                seconds = (int)stopwatch.Elapsed.TotalSeconds;
                WriteToConsole(0, 0, $"Loop count: {seconds}   ");

                if (taskA.IsCompleted && taskB.IsCompleted && taskC.IsCompleted)
                {
                    stopwatch.Stop();
                    break;
                }
            }

            Console.Clear();
            Console.WriteLine($"Tasks completed in {seconds} seconds");
        }

        private async Task TaskA()
        {
            for (int i = 0; i < 1000000; i++)
            {
                WriteToConsole(0, 1, $"Task A: {i}   ");
                await Task.Delay(50);
            }
        }
        private async Task TaskB()
        {
            for (int i = 0; i < 1000000; i++)
            {
                WriteToConsole(0, 2, $"Task B: {i}   ");
                await Task.Delay(100);
            }
        }
        private async Task TaskC()
        {
            for (int i = 0; i < 1000000; i++)
            {
                WriteToConsole(0, 3, $"Task C: {i}   ");
                await Task.Delay(150);
            }
        }

        private void WriteToConsole(int x, int y, string message)
        {
            lock (_consoleLock)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(message);
            }
        }
    }
}

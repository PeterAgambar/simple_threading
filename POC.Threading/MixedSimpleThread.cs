using System;
using System.Diagnostics;

namespace POC.Threading
{
    /// <summary>
    /// In each of the three threads, each updates a value. TaskA only starts once we reach 2 seconds.  TaskB starts when TaskA reaches 100. TaskB starts when task A reaches 200 then stops taskB but continues counting value B
    /// </summary>
    internal class MixedSimpleThread
    {
        private readonly object _consoleLock = new object();
        private Dictionary<string, CancellationTokenSource> _tokens = new();
        private int _valueA = 0;
        private int _valueB = 0;
        private int _valueC = 0;
        public MixedSimpleThread() 
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int seconds = 0;
            bool isTaskBStarted = false;
            // Declare tasks outside the loop
            Task taskA = null;
            Task taskB = null;
            Task taskC = null;
            while (true)
            {
                // Update elapsed time
                seconds = (int)stopwatch.Elapsed.TotalSeconds;
                WriteToConsole(0, 0, $"Elapsed Time: {seconds}s");

                // Start TaskA if it hasn't started yet
                if (taskA == null && seconds > 5)
                {
                    taskA = Task.Run(() => TaskA(Token("TaskA")), Token("TaskA"));
                }
                // Start TaskB and TaskC only after TaskA completes 100 iterations
                if (_valueA >= 100 && taskB?.Status != TaskStatus.Running && taskB?.Status != TaskStatus.WaitingForActivation)
                {
                    taskB = TaskB();
                    taskC = TaskC();
                    CancelToken("TaskA");
                }

                if(_valueC >= 100)
                {
                    taskA = Task.Run(() => TaskA(Token("TaskA")), Token("TaskA"));
                }


                // Stop the loop when all tasks are completed
                if (taskA != null && taskA.IsCompleted &&
                    taskB != null && taskB.IsCompleted &&
                    taskC != null && taskC.IsCompleted)
                {
                    break;
                }
                Thread.Sleep(500); 
            }
            stopwatch.Stop();
            Console.Clear();
            Console.WriteLine($"All tasks completed in {seconds} seconds.");
        }

        private async Task TaskA(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested) return;

                _valueA++;
                WriteToConsole(0, 1, $"Task A: {_valueA}   ");
                await Task.Delay(50);
            }
        }
        private async Task TaskB()
        {
            while (true)
            {
                _valueB++;
                WriteToConsole(0, 2, $"Task B: {_valueB}   ");
                await Task.Delay(100);
            }
        }
        private async Task TaskC()
        {
            while (true)
            {
                _valueC++;
                WriteToConsole(0, 3, $"Task C: {_valueC}   ");
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

        private CancellationToken Token(string name)
        {
            if (_tokens.ContainsKey(name))
            {
                var token = _tokens[name];
                if (token.IsCancellationRequested)
                {
                    CancellationTokenSource tokenSource = new();
                    _tokens[name] = tokenSource;
                    return tokenSource.Token;
                }

                return _tokens[name].Token;
                    
            }

            CancellationTokenSource tokenSource2 = new();
            _tokens.Add(name, tokenSource2);
            return tokenSource2.Token;
        }

        private void CancelToken(string name)
        {
            if(_tokens.ContainsKey(name))
                _tokens[name].Cancel();
        }
    }
}

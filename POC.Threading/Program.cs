using POC.Threading;
using System;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
internal class Program
{
    

    private static async Task Main(string[] args)
    {
        Console.WriteLine("Multiple Threading Example!");
        await Task.Delay(2000);
        Console.Clear();

        //SimpleThread thread = new();
        MixedSimpleThread thread = new();

    }

}

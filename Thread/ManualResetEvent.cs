using System;
using System.Threading;

class Program
{
    // The starting gun/gate: Initial state is 'false' (closed/unsignaled).
    private static ManualResetEventSlim _startGate = new ManualResetEventSlim(false);

    static void Main(string[] args)
    {
        Console.WriteLine("--- Race Start Coordinator ---\n");
        
        // 1. Line up 5 runners (threads)
        for (int i = 1; i <= 5; i++)
        {
            Thread runner = new Thread(WorkerRun);
            runner.Name = $"Runner {i}";
            runner.Start();
        }

        // 2. Wait a moment for all runners to get to the starting line and call Wait().
        Thread.Sleep(100); 
        Console.WriteLine("\nCoach: All runners are at the gate, waiting for the signal...\n");
        
        // 3. Fire the starting gun (Open the gate)
        _startGate.Set();
        Console.WriteLine("Coach: 💥 SET! All runners are GO!\n");

        // We can reuse the gate for a second race
        Thread.Sleep(3000); // Wait for the first race to finish
        
        // 4. Close the gate
        _startGate.Reset();
        Console.WriteLine("\nCoach: Gate closed. Ready for the next race.");

        // Any new thread would block again here
    }

    static void WorkerRun()
    {
        Console.WriteLine($"{Thread.CurrentThread.Name}: Arrived at the starting line.");

        // Block until the Main thread calls Set()
        _startGate.Wait(); 

        // Execution resumes immediately after Set() is called
        Console.WriteLine($"-> {Thread.CurrentThread.Name}: Running!");
        Thread.Sleep(1000); // Simulate running
        Console.WriteLine($"<- {Thread.CurrentThread.Name}: Finished.");
    }
}

using System;
using System.Threading;

class Program
{
    // 1. The Bouncer (SemaphoreSlim is preferred over the older 'Semaphore' class 
    //    even for threads, as it is lighter and faster within a single app).
    //    Capacity: 3 concurrent threads.
    private static SemaphoreSlim _bouncer = new SemaphoreSlim(3, 3);

    static void Main(string[] args)
    {
        Console.WriteLine("--- Club opens! Capacity: 3 people ---\n");

        // Create a list to keep track of threads so we can join them later
        Thread[] guests = new Thread[10];

        for (int i = 0; i < 10; i++)
        {
            // Create a new explicit Thread
            // We pass the method name 'EnterClub' that the thread will execute
            guests[i] = new Thread(EnterClub);
            guests[i].Name = $"Guest {i + 1}";

            // Start the thread immediately and pass the ID as a parameter
            guests[i].Start(i + 1);
        }

        // Optional: Wait for all threads to finish before closing the console
        foreach (Thread t in guests)
        {
            t.Join(); // This blocks the Main thread until thread 't' finishes
        }

        Console.WriteLine("\n--- Club closed! ---");
    }

    // This method must accept 'object' to be compatible with ParameterizedThreadStart
    static void EnterClub(object data)
    {
        int id = (int)data; // Unbox the ID
        Console.WriteLine($"{Thread.CurrentThread.Name} is waiting in line...");

        // 2. Ask to enter (Blocking Call)
        // If count is 0, this thread literally FREEZES here. 
        // The operating system puts it to sleep until notified.
        _bouncer.Wait();

        try
        {
            // --- CRITICAL SECTION ---
            Console.WriteLine($"-> {Thread.CurrentThread.Name} entered the club!");

            // Simulate work using Thread.Sleep (blocks execution)
            Thread.Sleep(2000); 

            Console.WriteLine($"<- {Thread.CurrentThread.Name} is leaving.");
        }
        finally
        {
            // 3. Release the slot
            _bouncer.Release();
        }
    }
}

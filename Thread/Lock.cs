using System;
using System.Threading.Tasks;
using System.Management;
using System.Collections.Immutable;

//
// Locks automatically released when leaving the lock block or fail
//
public class Program
{
    public static void Main()
    {
        int counter = 0;
        System.Threading.Lock counterLock = new Lock();

        Thread t1 = new Thread(IncrementCounter);
        Thread t2 = new Thread(IncrementCounter);        

        t1.Start();        
        t2.Start();        

        t1.Join();
        t2.Join();

        Console.WriteLine($"Counter Value is: {counter}");

        void IncrementCounter()
        {
            for (int i = 0; i < 10000; i++)
            {
                lock (counterLock)
                {
                    counter = counter + 1;
                }
            }    
        }    
    }
}


//
// Using Interlocked
//
using System;
using System.Threading; // Required for Thread and Interlocked

public class Program
{
    public static void Main()
    {
        int counter = 0;

        Thread t1 = new Thread(IncrementCounter);
        Thread t2 = new Thread(IncrementCounter);        

        t1.Start();        
        t2.Start();        

        t1.Join();
        t2.Join();

        Console.WriteLine($"Counter Value is: {counter}");

        void IncrementCounter()
        {
            for (int i = 0; i < 10000; i++)
            {
                // Atomically increments the variable
                Interlocked.Increment(ref counter); 
            }
        }    
    }
}


//
// Using Monitor.Enter (at the end locks are compiled into monitor)
//
using System;
using System.Threading.Tasks;
using System.Management;
using System.Collections.Immutable;

//
// Locks automatically released when leaving the lock block or fail
//
public class Program
{
    public static void Main()
    {
        int counter = 0;
        object counterLock = new object();

        Thread t1 = new Thread(IncrementCounter);
        Thread t2 = new Thread(IncrementCounter);        

        t1.Start();        
        t2.Start();        

        t1.Join();
        t2.Join();

        Console.WriteLine($"Counter Value is: {counter}");

        void IncrementCounter()
        {
            for (int i = 0; i < 10000; i++)
            {
                Monitor.Enter (counterLock);
                try
                {
                    counter = counter + 1;
                }
                finally
                {
                    Monitor.Exit (counterLock);
                }   
            }    
        }    
    }
}

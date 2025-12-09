using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    // The shared resource (The "Notice Board")
    private static List<string> _phoneBook = new List<string> { "Alice", "Bob", "Charlie" };

    // The Lock
    private static ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

    static void Main(string[] args)
    {
        Console.WriteLine("--- Phonebook Active ---\n");

        // 1. Create 3 Reader threads (People looking for numbers)
        for (int i = 0; i < 3; i++)
        {
            Thread t = new Thread(ReadPhoneBook);
            t.Name = $"Reader {i + 1}";
            t.Start();
        }

        // 2. Create 1 Writer thread (The Admin updating the book)
        Thread writer = new Thread(WriteToPhoneBook);
        writer.Name = "Writer Admin";
        writer.Start();
    }

    static void ReadPhoneBook()
    {
        while (true)
        {
            // Enter READ Lock
            // Multiple threads can pass this line at the same time!
            _rwLock.EnterReadLock(); 

            try
            {
                // Reading is safe here. The Writer cannot enter.
                Console.WriteLine($"   {Thread.CurrentThread.Name} is reading ({_phoneBook.Count} contacts).");
                Thread.Sleep(500); // Simulate reading time
            }
            finally
            {
                // ALWAYS exit in finally
                _rwLock.ExitReadLock();
            }
            
            // Wait a bit before reading again
            Thread.Sleep(100); 
        }
    }

    static void WriteToPhoneBook()
    {
        while (true)
        {
            Thread.Sleep(2000); // Admin writes every 2 seconds

            Console.WriteLine($"\n[!] {Thread.CurrentThread.Name} wants to update...");

            // Enter WRITE Lock
            // This waits for ALL current Readers to finish/leave.
            // Once inside, NO Readers and NO other Writers can enter.
            _rwLock.EnterWriteLock();

            try
            {
                Console.WriteLine($"[!] {Thread.CurrentThread.Name} is WRITING! (Everyone else waits)");
                _phoneBook.Add("New Contact " + new Random().Next(100));
                Thread.Sleep(1500); // Simulate slow writing
                Console.WriteLine($"[!] {Thread.CurrentThread.Name} finished writing.\n");
            }
            finally
            {
                // Release the lock so readers can come back
                _rwLock.ExitWriteLock();
            }
        }
    }
}

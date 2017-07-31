using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CancellationInManagedThreads
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Create the token source.
            CancellationTokenSource cts = new CancellationTokenSource();

            // Pass the token to the cancelable operation.
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoSomeWork), cts.Token);
            Thread.Sleep(2500);

            // Request cancellation.
            cts.Cancel();
            Console.WriteLine("Cancellation set in token source...");
            Thread.Sleep(2500);

            // Cancellation should have happened, so call Dispose.
            cts.Dispose();
        }

        // Thread 2: The listener
        static void DoSomeWork(object obj)
        {
            CancellationToken token = (CancellationToken)obj;

            for (int i = 0; i < 100000; i++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("In iteration {0}, cancellation has been requested...",
                                      i + 1);
                    // Perform cleanup if necessary.
                    //...
                    // Terminate the operation.
                    break;
                }
                // Simulate some work.
                Thread.SpinWait(500000);
            }
        }
    }
}

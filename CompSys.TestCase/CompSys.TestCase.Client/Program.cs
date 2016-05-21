using CompSys.TestCase.Client.SvcRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompSys.TestCase.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = string.Empty;

            try
            {
                CompSysServiceClient serviceClient = new CompSysServiceClient("BasicHttpBinding_ICompSysService");

                do
                {
                    Console.Write("Enter command > ");
                    input = Console.ReadLine();

                    switch (input.ToLower())
                    {
                        case "x":
                            break;
                        case "getoradd":
                            Console.Write("name=");
                            var name = Console.ReadLine();
                            var retId = serviceClient.GetOrAdd(name);
                            Console.WriteLine($"id = {retId}");
                            break;
                        case "addorupdate":
                            Console.Write("id=");
                            var id = int.Parse(Console.ReadLine());
                            Console.Write("value=");
                            var value = int.Parse(Console.ReadLine());
                            serviceClient.AddOrUpdate(id, value);
                            break;
                        case "transfer":
                            Console.Write("id1=");
                            var id1 = int.Parse(Console.ReadLine());
                            Console.Write("id2=");
                            var id2 = int.Parse(Console.ReadLine());
                            Console.Write("amount=");
                            var amount = decimal.Parse(Console.ReadLine());
                            serviceClient.Transfer(id1, id2, amount);
                            break;
                        default:
                            Console.WriteLine("Unknown command!");
                            break;
                    }
                } while (input != "x");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Error: {ex.InnerException.Message}");
                }

                Console.ReadKey();
            }
        }

        /// <summary>
        /// Race condition example
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="serviceFactory"></param>
        /// <param name="iterationCount"></param>
        /// <param name="taskCount"></param>
        private static void RunGetOrAdd(string prefix, Func<ICompSysService> serviceFactory, int iterationCount, int taskCount)
        {
            var barrier = new Barrier(taskCount);
            var cts = new CancellationTokenSource();
            var tasks = new Task<int[]>[taskCount];

            for (int i = 0; i < taskCount; i++)
            {
                tasks[i] = Task.Run(
                    () =>
                    {
                        try
                        {
                            var service = serviceFactory();
                            var ids = new int[iterationCount];
                            barrier.SignalAndWait(cts.Token);

                            for (int j = 0; j < iterationCount && !cts.IsCancellationRequested; j++)
                            {
                                ids[j] = service.GetOrAdd(prefix + j);
                            }

                            return ids;
                        }
                        catch
                        {
                            cts.Cancel();
                            throw;
                        }
                    });
            }

            var taskResults = Task.WhenAll(tasks).GetAwaiter().GetResult();

            for (int i = 0; i < taskResults.Length - 1; i++)
            {
                var a = taskResults[i];
                var b = taskResults[i + 1];

                for (int j = 0; j < a.Length; j++)
                {
                    if (a[j] != b[j])
                    {
                        throw new InvalidOperationException("RunGetorAdd does not work!!!");
                    }
                }
            }
        }
    }
}

using CompSys.TestCase.Client.SvcRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}

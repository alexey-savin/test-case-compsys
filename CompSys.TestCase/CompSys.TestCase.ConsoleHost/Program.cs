using CompSys.TestCase.WCFService.SP;
using CompSys.TestCase.WCFService.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace CompSys.TestCase.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost serviceHost = null;
            try
            {
                serviceHost = new ServiceHost(typeof(CompSysService_EF));
                serviceHost.Open();

                Console.WriteLine("Service online!");
                Console.ReadKey();

                serviceHost.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.ReadKey();
            }
        }
    }
}

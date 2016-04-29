using CompSys.TestCase.WCFService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace CompSys.TestCase.ConsoleHost
{
    class Program
    {
        private const string ServiceHostSectionName = "serviceHost";

        static void Main(string[] args)
        {
            ServiceHost serviceHost = null;
            try
            {
                ServiceHostSection serviceHostSection = (ServiceHostSection)ConfigurationManager.GetSection(ServiceHostSectionName);

                serviceHost = new ServiceHost(serviceHostSection.Implementation.GetImplementationType());
                serviceHost.Open();

                Console.WriteLine(serviceHostSection.Implementation.GetImplementationType().Name);
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

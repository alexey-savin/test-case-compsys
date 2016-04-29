using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompSys.TestCase.ConsoleHost
{
    internal class ServiceHostSection : ConfigurationSection
    {
        private const string ImplementationKey = "implementation";

        public ServiceHostSection()
        {
        }

        [ConfigurationProperty(ImplementationKey)]
        public virtual ImplementationElement Implementation
        {
            get { return (ImplementationElement)this[ImplementationKey]; }
            set { this[ImplementationKey] = value; }
        }
    }
}

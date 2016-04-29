using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompSys.TestCase.ConsoleHost
{
    internal class ImplementationElement : ConfigurationElement
    {
        private const string TypeKey = "type";

        [ConfigurationProperty(TypeKey, IsRequired = true)]
        public string ImplementationTypeName
        {
            get { return (string)this[TypeKey]; }
            set { this[TypeKey] = value; }
        }

        public Type GetImplementationType() => Type.GetType(ImplementationTypeName, throwOnError: true);
    }
}

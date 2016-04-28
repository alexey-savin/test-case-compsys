using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CompSys.TestCase.WCFService
{
    [ServiceContract]
    public interface ICompSysService
    {
        [OperationContract]
        int GetOrAdd(string name);

        [OperationContract]
        void AddOrUpdate(int id, int value);

        [OperationContract]
        void Transfer(int id1, int id2, decimal amount);
    }
}

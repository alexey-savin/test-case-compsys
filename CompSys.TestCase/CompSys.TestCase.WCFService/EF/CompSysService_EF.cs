using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompSys.TestCase.Data;

namespace CompSys.TestCase.WCFService.EF
{
    public class CompSysService_EF : ICompSysService
    {
        public void AddOrUpdate(int id, int value)
        {
            throw new NotImplementedException();
        }

        public int GetOrAdd(string name)
        {
            //context
            TestCaseDataEntities dataContext = new TestCaseDataEntities();
            Table_GetOrAdd entity = new Table_GetOrAdd();
            entity.Name = name;
            dataContext.Table_GetOrAdd.Add(entity);

            dataContext.SaveChanges();


            return entity.Id;
        }

        public void Transfer(int id1, int id2, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}

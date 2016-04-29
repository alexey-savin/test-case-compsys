using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompSys.TestCase.WCFService;
using CompSys.TestCase.Data;
using System.Linq;

namespace CompSys.TestCase.Test
{
    [TestClass]
    public class CompSysTest
    {
        private ICompSysService _service = new CompSysService_EF();

        [TestMethod]
        public void Test_Transfer()
        {
            int id1 = 2;
            int id2 = 1;
            decimal amount = 10;

            _service.Transfer(id1, id2, amount);
        }

        [TestMethod]
        public void Test_GetOrAdd()
        {
            string name = "test";
            Table_GetOrAdd entity = null;

            using (var context = new TestCaseDataEntities())
            {
                entity = context.Table_GetOrAdd.FirstOrDefault(e => e.Name == name);
                if (entity != null)
                {
                    context.Table_GetOrAdd.Remove(entity);
                    context.SaveChanges();
                }
            }

            using (var context = new TestCaseDataEntities())
            {
                entity = context.Table_GetOrAdd.FirstOrDefault(e => e.Name == name);
                Assert.IsNull(entity);

                entity = new Table_GetOrAdd { Name = name };
                context.Table_GetOrAdd.Add(entity);
                Assert.AreEqual(0, entity.Id);
                context.SaveChanges();
                Assert.AreNotEqual(0, entity.Id);
            }

            entity = null;
            using (var context = new TestCaseDataEntities())
            {
                entity = context.Table_GetOrAdd.FirstOrDefault(e => e.Name == name);
                Assert.IsNotNull(entity);
            }

            using (var context = new TestCaseDataEntities())
            {
                entity = context.Table_GetOrAdd.FirstOrDefault(e => e.Name == name);
                if (entity != null)
                {
                    context.Table_GetOrAdd.Remove(entity);
                    context.SaveChanges();
                }
            }

            int actual = _service.GetOrAdd(name);
            Assert.AreNotEqual(0, actual);
        }

        [TestMethod]
        public void Test_AddOrUpdate()
        {
            _service.AddOrUpdate(1, 10);
        }
    }
}

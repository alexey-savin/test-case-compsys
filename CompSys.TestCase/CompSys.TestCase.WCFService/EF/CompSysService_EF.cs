using System;
using System.Linq;
using CompSys.TestCase.Data;

namespace CompSys.TestCase.WCFService
{
    public class CompSysService_EF : ICompSysService
    {
        public void AddOrUpdate(int id, int value)
        {
            using (var dataContext = new TestCaseDataEntities())
            {
                using (var transaction = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        Table_AddOrUpdate entity = dataContext.Table_AddOrUpdate.FirstOrDefault(e => e.Id == id);
                        if (entity == null)
                        {
                            entity = new Table_AddOrUpdate { Id = id, Value = value };
                            dataContext.Table_AddOrUpdate.Add(entity);
                        }
                        else
                        {
                            entity.Value = value;
                        }

                        dataContext.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public int GetOrAdd(string name)
        {
            Table_GetOrAdd entity = null;
            int entityId = -1;

            using (var dataContext = new TestCaseDataEntities())
            {
                using (var transaction = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        entity = dataContext.Table_GetOrAdd.FirstOrDefault(e => e.Name == name);
                        if (entity == null)
                        {
                            entity = new Table_GetOrAdd { Name = name };
                            dataContext.Table_GetOrAdd.Add(entity);

                            dataContext.SaveChanges();
                            transaction.Commit();
                        }

                        entityId = entity.Id;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return entityId;
        }

        public void Transfer(int id1, int id2, decimal amount)
        {
            using (var dataContext = new TestCaseDataEntities())
            {
                using (var transaction = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        Table_Transfer transferFrom = dataContext.Table_Transfer.FirstOrDefault(e => e.Id == id1);
                        Table_Transfer transferTo = dataContext.Table_Transfer.FirstOrDefault(e => e.Id == id2);
                        if (transferFrom != null && transferTo != null)
                        {
                            if (transferFrom.Balance >= amount)
                            {
                                transferFrom.Balance -= amount;
                                transferTo.Balance += amount;

                                dataContext.SaveChanges();
                                transaction.Commit();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}

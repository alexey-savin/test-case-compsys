using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CompSys.TestCase.WCFService
{
    public class CompSysService : ICompSysService
    {
        public void AddOrUpdate(int id, int value)
        {
            using (var connection = CreateSqlConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("sp_AddOrUpdate", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@value", value);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public int GetOrAdd(string name)
        {
            using (var connection = CreateSqlConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("sp_GetOrAdd", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.Add("@id", SqlDbType.Int)
                        .Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();
                    connection.Close();

                    return Convert.ToInt32(command.Parameters["@id"].Value);
                }
            }
        }

        public void Transfer(int id1, int id2, decimal amount)
        {
            using (var connection = CreateSqlConnection())
            {
                connection.Open();

                using (var command = new SqlCommand("sp_Transfer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@id1", id1);
                    command.Parameters.AddWithValue("@id2", id2);
                    command.Parameters.AddWithValue("@amount", id2);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        private SqlConnection CreateSqlConnection() 
            => new SqlConnection(ConfigurationManager.ConnectionStrings["TestCaseData"].ToString());
    }
}

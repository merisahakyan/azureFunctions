using MyFunctionsApp.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace MyFunctionsApp.Services
{
    public class SqlService : ISqlService
    {
        public async Task<int> GetByUrlCountAsync(string url)
        {
            var str = Environment.GetEnvironmentVariable("sqldb_connection");
            using (SqlConnection conn = new SqlConnection(str))
            {
                var query = $"select count(*) from dbo.Images where Images.Url = '{url}'";

                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    var result = await cmd.ExecuteScalarAsync();
                    conn.Close();
                    return Convert.ToInt32(result);
                }
            }
        }

        public async Task InsertToDbAsync(string imageUrl, string fileName)
        {
            var str = Environment.GetEnvironmentVariable("sqldb_connection");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var command = $"insert into dbo.Images([Url],[FileName]) values('{imageUrl}', '{fileName}')";

                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    var rows = await cmd.ExecuteNonQueryAsync();
                    conn.Close();
                }
            }
        }
    }
}

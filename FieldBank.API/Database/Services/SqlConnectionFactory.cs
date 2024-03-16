using System.Data;
using System.Data.SqlClient;

namespace FieldBank.API.Database.Services
{
    public class SqlConnectionFactory(string connectionString)
    {
        public IDbConnection Create()
        {
            return new SqlConnection(connectionString);
        }
    }
}
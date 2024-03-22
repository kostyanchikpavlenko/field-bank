using System.Data;
using System.Data.SqlClient;

namespace FieldBank.API.Persistence.Services
{
    public class SqlConnectionFactory(string connectionString)
    {
        public IDbConnection Create()
        {
            return new SqlConnection(connectionString);
        }
    }
}
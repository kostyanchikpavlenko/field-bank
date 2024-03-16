using FieldBank.API.Database.Interfaces;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data;

namespace FieldBank.API.Database.Services
{
    public class SqlProvider : ISqlProvider
    {
        private readonly IDbConnection _dbConnection;
        public QueryFactory Db { get; }

        public SqlProvider(SqlConnectionFactory factory)
        {
            _dbConnection = factory.Create();
            Db = new QueryFactory(_dbConnection, new SqlServerCompiler());
        }

        public void Dispose()
        {
            _dbConnection?.Dispose();
            Db?.Dispose();
        }
    }
}
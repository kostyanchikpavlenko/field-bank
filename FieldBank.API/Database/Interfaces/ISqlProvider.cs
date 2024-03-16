using SqlKata.Execution;

namespace FieldBank.API.Database.Interfaces
{
    public interface ISqlProvider
    {
        QueryFactory Db { get; }
    }
}
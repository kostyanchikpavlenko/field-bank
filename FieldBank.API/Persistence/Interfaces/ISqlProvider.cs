using SqlKata.Execution;

namespace FieldBank.API.Persistence.Interfaces
{
    public interface ISqlProvider
    {
        QueryFactory Db { get; }
    }
}
using ChustaSoft.Tools.ExecutionControl.Model;

namespace ChustaSoft.Tools.ExecutionControl.Repositories
{
    public interface IExecutionRepository<TKey>
    {

        void Save(Execution<TKey> execution);

        void Update(Execution<TKey> execution);

    }
}

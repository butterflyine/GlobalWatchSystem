using GlobalWatchSystem.Models;
using SharpRepository.Repository;

namespace GlobalWatchSystem.Data.Repository
{
    public interface IDtuDataRepository : IRepository<DtuData>
    {
    }

    public class DtuDataRepository : ConfigurationBasedRepository<DtuData, int>, IDtuDataRepository
    {
    }
}
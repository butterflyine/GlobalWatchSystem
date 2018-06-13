using GlobalWatchSystem.Models;
using SharpRepository.Repository;

namespace GlobalWatchSystem.Data.Repository
{
    public interface IDtuGpsRepository : IRepository<DtuGPS>
    {
    }

    public class DtuGpsRepository : ConfigurationBasedRepository<DtuGPS, int>, IDtuGpsRepository
    {
    }
}
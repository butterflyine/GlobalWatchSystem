using GlobalWatchSystem.Models;
using SharpRepository.Repository;

namespace GlobalWatchSystem.Data.Repository
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
    }

    public class AuditLogRepository : ConfigurationBasedRepository<AuditLog, int>, IAuditLogRepository
    {
    }
}
using GlobalWatchSystem.Models;
using SharpRepository.Repository;

namespace GlobalWatchSystem.Data.Repository
{
    public interface IAppSettingRepository : IRepository<AppSetting>
    {
    }

    public class AppSettingRepository : ConfigurationBasedRepository<AppSetting, int>, IAppSettingRepository
    {
    }
}
using System.Configuration;
using System.Data.Entity;
using GlobalWatchSystem.Data;
using GlobalWatchSystem.Data.Repository;
using Ninject.Modules;
using Ninject.Web.Common;

namespace GlobalWatchSystem
{
    public class Module : NinjectModule
    {
        public override void Load()
        {
            Bind<DbContext>()
                .To<DatabaseContext>()
                .InRequestScope()
                .WithConstructorArgument("connectionString",
                    ConfigurationManager.ConnectionStrings["GlobalWatchSystem"].ConnectionString);

            Bind<IAreaRepository>().To<AreaRepository>();
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IDeviceRepository>().To<DeviceRepository>();
            Bind<IAuditLogRepository>().To<AuditLogRepository>();
            Bind<IDtuDataRepository>().To<DtuDataRepository>();
            Bind<IDtuGpsRepository>().To<DtuGpsRepository>();
            Bind<IAppSettingRepository>().To<AppSettingRepository>();
            Bind<ITransportPlanRepository>().To<TransportPlanRepository>();
            Bind<IDevicePlanRecordRepository>().To<DevicePlanRecordRepository>();
            Bind<IDeviceMetaRepository>().To<DeviceMetaRepository>();
            Bind<IDeviceUnitRepository>().To<DeviceUnitRepository>();
        }
    }
}
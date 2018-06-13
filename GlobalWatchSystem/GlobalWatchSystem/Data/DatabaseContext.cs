using System.Data.Entity;
using GlobalWatchSystem.Models;
using GlobalWatchSystem.Services.Alarm;

namespace GlobalWatchSystem.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("GlobalWatchSystem")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DatabaseContext>());
            //this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public DbSet<Area> Areas { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceAlarm> Alarms { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<DtuData> DtuDatas { get; set; }
        public DbSet<DtuGPS> DtuGpsDatas { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }

        public DbSet<TransportPlan> TransportPlans { get; set; }

        public DbSet<DevicePlanRecord> DevicePlanRecords { get; set; }

        public DbSet<DeviceMeta> DeviceMetas { get; set; }

        public DbSet<DeviceUnit> DeviceUnits { get; set; }

        public DbSet<DeviceResult> DeviceResults { get; set; }

        public DbSet<DeviceParam> DeviceParams { get; set; }
    }
}
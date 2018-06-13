using GlobalWatchSystem.Services.Alarm;
using NLog;
using Quartz;
using Quartz.Impl;

namespace GlobalWatchSystem.Services
{
    public class JobScheduler
    {
        public const int ScanIntervalMinutes = 1;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void StartScan()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail alarmScanJob = JobBuilder.Create<AlarmScanJob>().Build();
            ITrigger alarmScanTrigger = TriggerBuilder.Create().StartNow().WithSimpleSchedule(x => x.WithIntervalInMinutes(ScanIntervalMinutes).RepeatForever()).Build();
            scheduler.ScheduleJob(alarmScanJob, alarmScanTrigger);


            IJobDetail dataScanJob = JobBuilder.Create<DataScanJob>().Build();
            ITrigger dataScanTrigger = TriggerBuilder.Create().StartNow().WithSimpleSchedule(x => x.WithIntervalInMinutes(ScanIntervalMinutes).RepeatForever()).Build();
            scheduler.ScheduleJob(dataScanJob, dataScanTrigger);

            Logger.Info("Scheduled jobs.");
        }
    }
}
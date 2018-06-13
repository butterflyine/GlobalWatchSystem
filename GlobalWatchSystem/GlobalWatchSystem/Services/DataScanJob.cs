using System;
using System.Collections.Generic;
using System.Linq;
using GlobalWatchSystem.Data.Repository;
using GlobalWatchSystem.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using NLog;
using Quartz;
using WebGrease.Activities;

namespace GlobalWatchSystem.Services
{
    public class DataScanJob : IJob
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly DeviceDataService dataService;
        private readonly DeviceRepository deviceRepository;

        public DataScanJob()
        {
            dataService = new DeviceDataService();
            deviceRepository = new DeviceRepository();
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                DoDeviceDataScan();
            }
            catch (Exception e)
            {
                logger.Error("Error occurred in DataScanJob: {0}", e.Message);
                logger.Error(e.StackTrace);
            }

            DoDeviceDataScan();
        }

        private void DoDeviceDataScan()
        {
            IEnumerable<DtuData> unProcessedData = dataService.GetLatestData(TimeSpan.FromMinutes(JobScheduler.ScanIntervalMinutes));
            IEnumerable<IGrouping<string, DtuData>> grouped = from data in unProcessedData
                group data by data.IMEI
                into newGroup
                select newGroup;

            foreach (var group in grouped)
            {
                Device device;
                var key = @group.Key;
                if (deviceRepository.TryFind(dev => dev.IMEI.Equals(key), out device))
                {
                    var data = @group.DistinctBy(dtuData => dtuData.Channel).Select(dtuData => new DataEntry {Channel = dtuData.Channel, Type = dtuData.DataType, Value = dtuData.Value}).ToList();
                    device.Data = JsonConvert.SerializeObject(data);
                    device.DataDttm = @group.First().RecvTime;
                    deviceRepository.Update(device);
                }
            }
        }
    }
}
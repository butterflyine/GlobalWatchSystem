using System;
using System.Collections.Generic;
using GlobalWatchSystem.Data.Repository;
using GlobalWatchSystem.Models;
using GlobalWatchSystem.Services.Alarm;

namespace GlobalWatchSystem.Services
{
    public class DeviceDataService
    {
        private DtuDataRepository dtuDataRepository;

        public DeviceDataService()
        {
            dtuDataRepository = new DtuDataRepository();
        }


        public IEnumerable<DtuData> GetLatestData(TimeSpan duration)
        {
            DateTime end = DateTime.Now;
            DateTime start = DateTime.Now.Subtract(duration);
            return dtuDataRepository.FindAll(data => data.RecvTime <= end && data.RecvTime > start);
        }
    }
}
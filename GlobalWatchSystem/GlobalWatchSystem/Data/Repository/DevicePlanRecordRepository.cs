using GlobalWatchSystem.Models;
using SharpRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalWatchSystem.Data.Repository
{
    public interface IDevicePlanRecordRepository : IRepository<DevicePlanRecord>
    {
    }

    public class DevicePlanRecordRepository : ConfigurationBasedRepository<DevicePlanRecord, int>, IDevicePlanRecordRepository
    {
    }
}
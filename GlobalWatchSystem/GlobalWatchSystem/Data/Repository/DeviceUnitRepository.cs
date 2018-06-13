using GlobalWatchSystem.Models;
using SharpRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalWatchSystem.Data.Repository
{
    public interface IDeviceUnitRepository : IRepository<DeviceUnit>
    {
    }
    public class DeviceUnitRepository : ConfigurationBasedRepository<DeviceUnit, int>, IDeviceUnitRepository
    { }
}
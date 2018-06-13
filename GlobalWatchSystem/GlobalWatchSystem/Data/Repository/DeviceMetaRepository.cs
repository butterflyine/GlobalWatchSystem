using GlobalWatchSystem.Models;
using SharpRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalWatchSystem.Data.Repository
{
    public interface IDeviceMetaRepository : IRepository<DeviceMeta>
    {
    }

    public class DeviceMetaRepository : ConfigurationBasedRepository<DeviceMeta, int>, IDeviceMetaRepository
    { 
    }
}
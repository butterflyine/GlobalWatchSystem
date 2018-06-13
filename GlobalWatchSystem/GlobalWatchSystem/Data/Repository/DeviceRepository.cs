using System.Collections.Generic;
using System.Linq;
using GlobalWatchSystem.Models;
using SharpRepository.Repository;

namespace GlobalWatchSystem.Data.Repository
{
    public interface IDeviceRepository : IRepository<Device>
    {
        IEnumerable<Device> FindDevicesInAreas(IList<int> areaIds);
    }

    public class DeviceRepository : ConfigurationBasedRepository<Device, int>, IDeviceRepository
    {
        public IEnumerable<Device> FindDevicesInAreas(IList<int> areaIds)
        {
            if (areaIds == null || !areaIds.Any()) return new List<Device>();
            return FindAll(device => areaIds.Contains(device.AreaId));
        }
    }
}
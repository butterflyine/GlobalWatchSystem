using GlobalWatchSystem.Data.Repository;
using GlobalWatchSystem.Models;
using GlobalWatchSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GlobalWatchSystem.Controllers
{
    public class TableController : ApiController
    {
        private readonly IDeviceRepository deviceRepository;
        private readonly IAreaRepository areaRepository;

        public TableController()
        {
            this.deviceRepository = new DeviceRepository();
            this.areaRepository = new AreaRepository();
        }
        // GET api/<controller>/5
        public IEnumerable<Device> Get(int areaId = 1)
        {
            List<int> areaIds = areaRepository.GetChildAreas(areaId).ToList().Select(area => area.Id).ToList();
            IEnumerable<Device> devices = deviceRepository
                .FindAll(device => areaIds.Contains(device.AreaId)).Select(device => new Device{
                    IMEI = device.IMEI,
                    PowerMode = device.PowerMode,
                    Battery = device.Battery
                });


            return devices;

        }
    }
}
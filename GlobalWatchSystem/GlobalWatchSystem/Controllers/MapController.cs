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
    public class MapController : ApiController
    {
        private readonly IDtuGpsRepository gpsRepository = null;
        private readonly IAreaRepository areaRepository;
        private readonly IDeviceRepository deviceRepository;

        public MapController()
        {
            gpsRepository = new DtuGpsRepository();
            areaRepository = new AreaRepository();
            deviceRepository = new DeviceRepository();
        }
        // GET api/<controller>
        public IEnumerable<DeviceMapMode> Get(int areaId)
        {
            List<int> areaIds = areaRepository.GetChildAreas(areaId).ToList().Select(area => area.Id).ToList();
            IEnumerable<DeviceModel> devices = deviceRepository
                .FindAll(device => areaIds.Contains(device.AreaId)).Select(device =>
                    new DeviceModel
                    {
                        Id = device.Id,
                        Name = device.Name,
                        Description = device.Description,
                        IMEI = device.IMEI,
                        SimNumber = device.SimNumber,
                        AreaName = areaRepository.Get(device.AreaId).Name
                    });

            List<DeviceMapMode> devInfoList = new List<DeviceMapMode>();
            foreach (DeviceModel imei in devices)
            {
                var gps = gpsRepository.FindAll(p => p.IMEI == imei.IMEI).OrderByDescending(p => p.RecvTime).FirstOrDefault();

                if (gps != null)
                {
                    DeviceMapMode mode = new DeviceMapMode();
                    mode.Device = imei;
                    mode.DtuGPS = gps;
                    devInfoList.Add(mode);
                }
            }
            return devInfoList;
        }

       
    }
}
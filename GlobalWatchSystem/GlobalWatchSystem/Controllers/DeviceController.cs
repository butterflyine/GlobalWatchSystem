using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GlobalWatchSystem.Data.Repository;
using GlobalWatchSystem.Helpers;
using GlobalWatchSystem.Models;
using GlobalWatchSystem.Models.ViewModel;
using NLog;
using SharpRepository.Repository.Queries;
using System;
using System.IO;
using WebGrease.Css.Extensions;
using GlobalWatchSystem.Data;

namespace GlobalWatchSystem.Controllers
{
    public class DeviceController : BaseController
    {
        private readonly IAreaRepository areaRepository;
        private readonly IAuditLogRepository auditLogRepository;
        private readonly IDeviceRepository deviceRepository;
        private readonly IDtuDataRepository dtuDataRepository;
        private readonly IDtuGpsRepository dtuGpsRepository;
        private readonly ITransportPlanRepository transportPlanRepository;
        private readonly IDevicePlanRecordRepository devicePlanRepository;
        private readonly IDeviceMetaRepository deviceMetaRepository;
        private readonly IDeviceUnitRepository deviceUnitRepository;

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public DeviceController(IAreaRepository areaRepository, IDeviceRepository deviceRepository,
            IDtuDataRepository dtuDataRepository,
            IAuditLogRepository auditLogRepository,
            IDtuGpsRepository dtuGpsRepository,
            ITransportPlanRepository transportPlanRepository,
            IDevicePlanRecordRepository devicePlanRepository,
            IDeviceMetaRepository deviceMetaRepository,
            IDeviceUnitRepository deviceUnitRepository)
            : base(areaRepository)
        {
            this.areaRepository = areaRepository;
            this.deviceRepository = deviceRepository;
            this.dtuDataRepository = dtuDataRepository;
            this.auditLogRepository = auditLogRepository;
            this.dtuGpsRepository = dtuGpsRepository;
            this.transportPlanRepository = transportPlanRepository;
            this.devicePlanRepository = devicePlanRepository;
            this.deviceMetaRepository = deviceMetaRepository;
            this.deviceUnitRepository = deviceUnitRepository;
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Device exising = deviceRepository.Get(id);
            deviceRepository.Delete(exising);

            auditLogRepository.Add(AuditLogBuilder.Builder()
                .User(HttpContext.User.Identity.Name)
                .Deleted(typeof(Device), exising.Name)
                .With(new ChangeInfo().AddChange(() => exising.Name).ToJson())
                .Build());

            logger.Info("User '{0}' deleted device '{1}'.", Identity.Name, exising.Name);

            return RedirectToAction("TableMode");
        }

        [HttpGet]
        public ActionResult Export(int from, int to, string deviceImei)
        {
            DateTime start = UnixTimeStampToDateTime(from);
            DateTime end = UnixTimeStampToDateTime(to);

            var tempFileName = Path.GetTempFileName();
            var fileInfo = new FileInfo(tempFileName);
            fileInfo.Attributes = FileAttributes.Temporary;
            using (var writer = System.IO.File.AppendText(tempFileName))
            {
                writer.WriteLine("IMEI,RecvDttm,Channel,Unit,Value");
                var dtuDatas = dtuDataRepository.FindAll(data => data.IMEI == deviceImei && data.RecvTime >= start && data.RecvTime <= end).OrderBy(data => data.RecvTime);
                dtuDatas.ForEach(record =>
                    writer.WriteLine("{0},{1},{2},{3},{4}", record.IMEI, record.RecvTime, record.Channel, record.DataType, record.Value));
            }
            return File(System.IO.File.ReadAllBytes(tempFileName), "text/csv", String.Format("{0}.csv", deviceImei));
        }

        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }


        public ActionResult Edit(int id)
        {
            Device existing = deviceRepository.Get(id);
            if (existing == null)
            {
                return RedirectToAction("TableMode");
            }

            var model = new DeviceModel
            {
                Id = existing.Id,
                Name = existing.Name,
                Description = existing.Description,
                IMEI = existing.IMEI,
                SimNumber = existing.SimNumber,
                AreaId = existing.AreaId,
                TransportPlanId = existing.TransportPlanId
            };

            var sortingOptions = new SortingOptions<DtuGPS, DateTime>(x => x.RecvTime, true);
            DtuGPS gps = dtuGpsRepository.FindAll(g => g.IMEI == existing.IMEI, sortingOptions).FirstOrDefault();
            if (gps != null)
            {
                model.longitude = gps.Longitude;
                model.latitude = gps.Latitude;
            }

            ViewBag.Areas = GetAreaSelectItems();
            ViewBag.Plans = GetPlanSelectItems();
            ViewBag.DeviceTypes = GetDeviceTypeItems();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(DeviceModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Areas = GetAreaSelectItems();
                ViewBag.Plans = GetPlanSelectItems();
                ViewBag.DeviceTypes = GetDeviceTypeItems();
                return View(model);
            }

            Device existing = deviceRepository.Get(model.Id);

            if (existing.TransportPlanId != model.TransportPlanId)
            {
                var ele = devicePlanRepository.Find(p => p.DeviceId == existing.Id && p.PlanId == existing.TransportPlanId, new SortingOptions<DevicePlanRecord>("startTime", true));
                if (ele != null)
                {
                    ele.endTime = DateTime.Now;
                    devicePlanRepository.Update(ele);
                }
                if (model.TransportPlanId != null)
                {
                    devicePlanRepository.Add(new DevicePlanRecord { DeviceId = model.Id, PlanId = model.TransportPlanId, startTime = DateTime.Now });
                }
            }

            UpdateDeviceFromModel(existing, model);
            deviceRepository.Update(existing);

            dtuGpsRepository.Add(new DtuGPS
            {
                IMEI = model.IMEI,
                RecvTime = DateTime.Now,
                Longitude = model.longitude,
                Latitude = model.latitude
            });

            AuditDeviceChange(existing);


            return RedirectToAction("TableMode");
        }

        private void AuditDeviceChange(Device existing)
        {
            string changes = new ChangeInfo()
                .AddChange(() => existing.Name)
                .AddChange(() => existing.Description)
                .AddChange(() => existing.SimNumber)
                .AddChange(() => existing.IMEI)
                .AddChange(() => existing.AreaId)
                .AddChange(()=>existing.DeviceMetaId)
                .ToJson();
            auditLogRepository.Add(AuditLogBuilder.Builder()
                .User(HttpContext.User.Identity.Name)
                .Updated(typeof(Device), existing.Name)
                .With(changes)
                .Build());
        }

        public ActionResult Create()
        {
            ViewBag.Areas = GetAreaSelectItems();
            ViewBag.Plans = GetPlanSelectItems();
            ViewBag.DeviceTypes = GetDeviceTypeItems();
            return View();
        }

        [HttpPost]
        public ActionResult Create(DeviceModel deviceModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Areas = GetAreaSelectItems();
                ViewBag.Plans = GetPlanSelectItems();
                ViewBag.DeviceTypes = GetDeviceTypeItems();
                return View(deviceModel);
            }

            var device = new Device();
            UpdateDeviceFromModel(device, deviceModel);
            deviceRepository.Add(device);

            if (device.TransportPlanId != null)
            {
                devicePlanRepository.Add(new DevicePlanRecord { DeviceId = device.Id, PlanId = device.TransportPlanId, startTime = DateTime.Now });
            }

            dtuGpsRepository.Add(new DtuGPS
            {
                IMEI = deviceModel.IMEI,
                RecvTime = DateTime.Now,
                Longitude = deviceModel.longitude,
                Latitude = deviceModel.latitude
            });

            auditLogRepository.Add(AuditLogBuilder.Builder()
                .User(HttpContext.User.Identity.Name)
                .Added(typeof(Device), device.Name)
                .With(new ChangeInfo().AddChange(() => device.Name).ToJson())
                .Build());

            logger.Info("User '{0}' created device '{1}'.", Identity.Name, device.Name);

            return RedirectToAction("TableMode");
        }

        public ActionResult Tempmode12chs(int page = 1)
        {
            string deviceType = "12路温度仪";

            ViewBag.Pagination = new Pagination { TotalCount = deviceRepository.Count(), CurrentPage = page };
            List<int> areaIds = CurrentAvailableAreas.Select(area => area.Id).ToList();
            IEnumerable<DeviceModel> devices = deviceRepository.
                FindAll(device => areaIds.Contains(device.AreaId) && (device.DeviceMeta == null ? true : device.DeviceMeta.DeviceType == deviceType),
                    new PagingOptions<Device>(page, Pagination.DefaultPageSize, "Name"))
                .Select(device =>
                    new DeviceModel
                    {
                        Id = device.Id,
                        Name = device.Name,
                        Description = device.Description,
                        IMEI = device.IMEI,
                        SimNumber = device.SimNumber,
                        AreaName = areaRepository.Get(device.AreaId).Name,
                        Battery = device.Battery,
                        DataDttm = device.DataDttm,
                        PowerMode = device.PowerMode,
                        TransportPlanId = device.TransportPlanId,
                        TransportPlan = device.TransportPlan
                    });

            DeviceMeta metaData = deviceMetaRepository.Find(p => p.DeviceType == deviceType);
            if (metaData == null || string.IsNullOrEmpty(metaData.MetaContent))
            {
                return this.HttpNotFound();
            }

            string[] chnInfos = metaData.MetaContent.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            
            var val = chnInfos.Select(p => new DeviceUnitMode{
                Ch = p.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0],
                Unit  = deviceUnitRepository.Get(Convert.ToInt32(p.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1]))
            });

            ViewBag.chInfos = val;
            return View(devices);
        }

        public ActionResult TableMode(int page = 1)
        {
            ViewBag.Pagination = new Pagination { TotalCount = deviceRepository.Count(), CurrentPage = page };
            List<int> areaIds = CurrentAvailableAreas.Select(area => area.Id).ToList();
            IEnumerable<DeviceModel> devices = deviceRepository.
                FindAll(device => areaIds.Contains(device.AreaId),
                    new PagingOptions<Device>(page, Pagination.DefaultPageSize, "Name"))
                .Select(device =>
                    new DeviceModel
                    {
                        Id = device.Id,
                        Name = device.Name,
                        Description = device.Description,
                        IMEI = device.IMEI,
                        SimNumber = device.SimNumber,
                        AreaName = areaRepository.Get(device.AreaId).Name,
                        Battery = device.Battery,
                        DataDttm = device.DataDttm,
                        PowerMode = device.PowerMode,
                        TransportPlanId = device.TransportPlanId,
                        TransportPlan = device.TransportPlan
                    });

            return View(devices);
        }

        public ActionResult MapMode(int id = 1)
        {
            ViewBag.deviceId = id;
            ViewBag.areaId = CurrentAreaId;
            return View();
        }

        public ActionResult CurveMode(int id = 1)
        {
            return ShowCurveMode(id);
        }

        private ActionResult ShowCurveMode(int id)
        {
            Device existing = deviceRepository.Get(id);
            if (existing == null)
            {
                return RedirectToAction("TableMode");
            }
            var mode = new DeviceModel
            {
                Id = existing.Id,
                Name = existing.Name,
                Description = existing.Description,
                IMEI = existing.IMEI,
                SimNumber = existing.SimNumber,
                AreaId = existing.AreaId,
                Battery = existing.Battery,
                PowerMode = existing.PowerMode
            };
            ViewBag.Areas = GetAreaSelectItems();
            return View(mode);
        }

        public ActionResult HistoryMode(int id = 1)
        {
            return ShowCurveMode(id);
        }

        private static void UpdateDeviceFromModel(Device toUpdate, DeviceModel model)
        {
            if (toUpdate == null) return;

            toUpdate.Name = model.Name;
            toUpdate.Description = model.Description;
            toUpdate.IMEI = model.IMEI;
            toUpdate.SimNumber = model.SimNumber;
            toUpdate.AreaId = model.AreaId;
            toUpdate.TransportPlanId = model.TransportPlanId;
            toUpdate.DeviceMetaId = model.DeviceMetaId;
        }

        private IOrderedEnumerable<SelectListItem> GetAreaSelectItems()
        {
            return CurrentAvailableAreas.Select(
                area => new SelectListItem { Text = area.Name, Value = area.Id.ToString() })
                .OrderBy(item => item.Text);
        }

        public IEnumerable<SelectListItem> GetDeviceTypeItems()
        {

            using (DatabaseContext ctx = new DatabaseContext())
            {
                var items = ctx.DeviceMetas.Select(p => new SelectListItem { Text = p.DeviceType, Value = p.Id.ToString() }).Distinct().ToList();
                items.Insert(0, new SelectListItem { Text = "", Value = null });
                return items;
            }
        }

        private IEnumerable<SelectListItem> GetPlanSelectItems()
        {
            var items = transportPlanRepository.FindAll(t => t.stopTime == null || t.startTime == null || t.stopTime > DateTime.Now).Select(
                area => new SelectListItem { Text = area.Name, Value = area.Id.ToString() })
                .OrderBy(item => item.Text).ToList();
            items.Insert(0, new SelectListItem { Text = "", Value = null });

            return items;
        }
    }
}
using GlobalWatchSystem.Data.Repository;
using GlobalWatchSystem.Helpers;
using GlobalWatchSystem.Models;
using GlobalWatchSystem.Models.ViewModel;
using Newtonsoft.Json;
using NLog;
using SharpRepository.Repository.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace GlobalWatchSystem.Controllers
{
    public class TransportPlanController : BaseController
    {
        private readonly ITransportPlanRepository transportPlanRepository;
        private readonly IAreaRepository areaRepository;
        private readonly IAuditLogRepository auditLogRepository;
        private readonly IDevicePlanRecordRepository devicePlanRepository;
        private readonly IDtuGpsRepository dtuGpsRepository;
        private readonly IDeviceRepository deviceRepository;

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public TransportPlanController(ITransportPlanRepository transportPlanRepository, 
            IAreaRepository areaRepository, 
            IAuditLogRepository auditLogRepository, 
            IDevicePlanRecordRepository devicePlanRepository,
            IDtuGpsRepository dtuGpsRepository,
            IDeviceRepository deviceRepository)
            : base(areaRepository)
        {
            this.transportPlanRepository = transportPlanRepository;
            this.areaRepository = areaRepository;
            this.auditLogRepository = auditLogRepository;
            this.devicePlanRepository = devicePlanRepository;
            this.dtuGpsRepository = dtuGpsRepository;
            this.deviceRepository = deviceRepository;
        }

        public ActionResult Index(int page = 1)
        {
            ViewBag.Pagination = new Pagination { TotalCount = transportPlanRepository.Count(), CurrentPage = page };
            List<int> areaIds = CurrentAvailableAreas.Select(area => area.Id).ToList();
            IEnumerable<TransportPlan> plans = transportPlanRepository.GetAll(new PagingOptions<TransportPlan>(page, Pagination.DefaultPageSize, "Name"));

            return View(plans);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            var exising = transportPlanRepository.Get(id);
            if(exising == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            transportPlanRepository.Delete(exising);

            auditLogRepository.Add(AuditLogBuilder.Builder()
                .User(HttpContext.User.Identity.Name)
                .Deleted(typeof(TransportPlan), exising.Name)
                .With(new ChangeInfo().AddChange(() => exising.Name).ToJson())
                .Build());

            logger.Info("User '{0}' deleted TransportPlan '{1}'.", Identity.Name, exising.Name);

            return RedirectToAction("Index");
        }

        public ActionResult ViewPlan(int id)
        {
            var plan = transportPlanRepository.Get(id);

            if(plan == null)
            {
                return HttpNotFound();
            }
            //首先查询到计划关联的所有的设备
            var records = devicePlanRepository.FindAll(p => p.PlanId == id);
            //if(plan.stopTime != null)
            //{
            //    records = records.Where(p => { if (p.endTime == null) { return true; } else return p.endTime <= plan.stopTime; });
            //}

            var devIds = records.Select(p=>p.DeviceId);

            var imeis = deviceRepository.FindAll(p=>devIds.Contains(p.Id)).Select(d=>d.IMEI);

            List<TransportTrack> tracks = new List<TransportTrack>();

            DateTime endTime = plan.stopTime == null ? DateTime.Now : plan.stopTime.Value;

            foreach(var ele in records)
            {
                DateTime devEndTime = ele.endTime == null ? DateTime.Now : ele.endTime.Value;

                DateTime realEndTime = devEndTime > endTime ? endTime : devEndTime;

                var query = dtuGpsRepository.FindAll(d => imeis.Contains(d.IMEI) && d.RecvTime >= ele.startTime && d.RecvTime < realEndTime, new SortingOptions<DtuGPS>("RecvTime"));

                Device device = deviceRepository.Get(ele.DeviceId.Value);
                var eles = query.Select(gps => new TransportTrack
                {
                    name = device != null ? device.Name : "",
                    latitude = gps.Latitude,
                    longitude = gps.Longitude
                });
                tracks.AddRange(eles);
            }
            ViewBag.PlanName = plan.Name;
            ViewBag.Data = JsonConvert.SerializeObject(tracks);
            return View();
        }

        public ActionResult StartPlan(int id)
        {
            var plan = transportPlanRepository.Get(id);

            if (plan == null)
            {
                return HttpNotFound();
            }
            plan.startTime = DateTime.Now;

            transportPlanRepository.Update(plan);

            return RedirectToAction("Index");
        }

        public ActionResult StopPlan(int id)
        {
            var plan = transportPlanRepository.Get(id);

            if (plan == null)
            {
                return HttpNotFound();
            }
            plan.stopTime = DateTime.Now;

            transportPlanRepository.Update(plan);

            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
           
            var plan = transportPlanRepository.Get(id);

            if(plan == null)
            {
                return HttpNotFound();
            }

            return View(plan);
        }
        [HttpPost]
        public ActionResult Edit(TransportPlan model)
        {
            if(!ModelState.IsValid)
            {
                View(model); 
            }


            var existing = transportPlanRepository.Get(model.Id);
            existing.Name = model.Name;
            existing.startTime = model.startTime;
            existing.stopTime = model.stopTime;
            existing.Remark = model.Remark;

            transportPlanRepository.Update(existing);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(TransportPlan model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            transportPlanRepository.Add(model);

         
            auditLogRepository.Add(AuditLogBuilder.Builder()
                .User(HttpContext.User.Identity.Name)
                .Added(typeof(TransportPlan), model.Name)
                .With(new ChangeInfo().AddChange(() => model.Name).ToJson())
                .Build());

            logger.Info("User '{0}' created TransportPlan '{1}'.", Identity.Name, model.Name);

            return RedirectToAction("Index");
        }
    }
}
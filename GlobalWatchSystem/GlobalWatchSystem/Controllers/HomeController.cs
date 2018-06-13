using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalWatchSystem.Data.Repository;
using GlobalWatchSystem.Helpers;
using GlobalWatchSystem.Models;
using NLog;

namespace GlobalWatchSystem.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IAppSettingRepository appSettingRepository;
        private readonly IAreaRepository areaRepository;
        private readonly IAuditLogRepository auditLogRepository;
        private readonly IDeviceRepository devRepository;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUserRepository userRepository;
        private readonly ITransportPlanRepository transportPlanRepository;

        public HomeController(IAreaRepository areaRepository, IDeviceRepository devRepository,
            IUserRepository userRepository, IAppSettingRepository appSettingRepository, IAuditLogRepository auditLogRepository, ITransportPlanRepository transportPlanRepository)
            : base(areaRepository)
        {
            this.areaRepository = areaRepository;
            this.devRepository = devRepository;
            this.userRepository = userRepository;
            this.appSettingRepository = appSettingRepository;
            this.auditLogRepository = auditLogRepository;
            this.transportPlanRepository = transportPlanRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.areaId = CurrentAreaId;
            ViewBag.AreaCount = CurrentAvailableAreas.Count;
            ViewBag.DeviceCount =
                devRepository.FindDevicesInAreas(CurrentAvailableAreas.Select(area => area.Id).ToList()).Count();
            ViewBag.UserCount = userRepository.FindUsersInAreas(CurrentAvailableAreas.Select(area => area.Id)).Count();
            ViewBag.PlanCount = transportPlanRepository.Count();
            return View();
        }

        [HttpGet]
        [Authorize(Users = "admin")]
        public ActionResult Settings()
        {
            AppSetting model = appSettingRepository.GetAll().First();
            return View(model);
        }

        [HttpPost]
        [Authorize(Users = "admin")]
        public ActionResult Settings(AppSetting model)
        {
            if (model.TemperatureLower >= model.TemperatureUpper)
            {
                ModelState.AddModelError("", Resources.Resources.TemperatureThresholdConfigError);
            }

            if (model.HumidityLower >= model.HumidityUpper)
            {
                ModelState.AddModelError("", Resources.Resources.HumidityThresholdConfigError);
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            appSettingRepository.Update(model);
            auditLogRepository.Add(AuditLogBuilder.Builder()
                .User(Identity.Name)
                .Updated(typeof (AppSetting), model.Id.ToString())
                .With(new ChangeInfo()
                    .AddChange(() => model.TemperatureUpper)
                    .AddChange(() => model.TemperatureLower)
                    .AddChange(() => model.Battery)
                    .AddChange(() => model.HumidityUpper)
                    .AddChange(() => model.HumidityLower)
                    .ToJson()).Build());
            logger.Info("User '{0}' updated system setting.", Identity.Name);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult SetCulture(string culture)
        {
            SetCultureCookie(culture);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Switch(int id)
        {
            if (areaRepository.Exists(area => area.Id == id))
            {
                var currentAreaCookie = new HttpCookie(CookieKeyCurrentArea, id.ToString());
                HttpContext.Response.Cookies.Add(currentAreaCookie);
                HttpContext.Request.Cookies.Add(currentAreaCookie);
            }

            return RedirectToAction("Index");
        }
    }
}
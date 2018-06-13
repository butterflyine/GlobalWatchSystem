using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GlobalWatchSystem.Data.Repository;
using GlobalWatchSystem.Helpers;
using GlobalWatchSystem.Models;
using GlobalWatchSystem.Models.ViewModel;
using NLog;
using SharpRepository.Repository.Queries;

namespace GlobalWatchSystem.Controllers
{
    public class AreaController : BaseController
    {
        private const string MESSAGE_KEY = "Message";
        private readonly IAreaRepository areaRepository;
        private readonly Func<Area, SelectListItem> areaToSelectItem = area => new SelectListItem {Text = area.Name, Value = area.Id.ToString()};
        private readonly IAuditLogRepository auditLogRepository;
        private readonly IDeviceRepository deviceRepository;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUserRepository userRepository;
        private readonly Func<AreaModel, Area> viewModelToArea = areaModel => new Area {Name = areaModel.Name, Description = areaModel.Description, ParentId = areaModel.ParentId == null ? 0 : int.Parse(areaModel.ParentId)};

        public AreaController(IAreaRepository areaRepository, IUserRepository userRepository, IAuditLogRepository auditLogRepository, IDeviceRepository deviceRepository)
            : base(areaRepository)
        {
            this.areaRepository = areaRepository;
            this.userRepository = userRepository;
            this.auditLogRepository = auditLogRepository;
            this.deviceRepository = deviceRepository;
        }

        public ActionResult Index(int page = 1)
        {
            ViewBag.Message = TempData[MESSAGE_KEY];
            if (TempData.ContainsKey(MESSAGE_KEY))
            {
                ModelState.AddModelError("", ((String) TempData[MESSAGE_KEY]));
            }

            List<int> areaIds = CurrentAvailableAreas.Select(area => area.Id).ToList();
            IEnumerable<AreaModel> areaModels = areaRepository
                .FindAll(area => areaIds.Contains(area.Id), new PagingOptions<Area>(page, Pagination.DefaultPageSize, "Name"))
                .Select(AreaToViewModel);
            ViewBag.Pagination = new Pagination {CurrentPage = page, TotalCount = areaModels.Count()};
            return View(areaModels);
        }

        public ActionResult Create()
        {
            ViewBag.Areas = CurrentAvailableAreas.Select(areaToSelectItem).OrderBy(item => item.Text);
            return View();
        }

        [HttpPost]
        public ActionResult Create(AreaModel areaModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Areas = GetCurrentSelectableAreaItems();
                return View(areaModel);
            }

            areaRepository.Add(viewModelToArea(areaModel));

            auditLogRepository.Add(
                AuditLogBuilder.Builder()
                    .User(HttpContext.User.Identity.Name)
                    .Added(typeof (Area), areaModel.Name)
                    .With(new ChangeInfo().AddChange(() => areaModel.Name).ToJson())
                    .Build());

            logger.Info("User '{0}' created area '{1}'.", Identity.Name, areaModel.Name);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Areas = GetCurrentSelectableAreaItems();
            Area area = areaRepository.Get(id);
            return View(AreaToViewModel(area));
        }

        private AreaModel AreaToViewModel(Area area)
        {
            return new AreaModel
            {
                Id = area.Id,
                Name = area.Name,
                Description = area.Description,
                ParentId = area.ParentId != 0 ? area.ParentId.ToString() : "",
                ParentArea = area.ParentId != 0 ? areaRepository.Get(area.ParentId).Name : ""
            };
        }

        [HttpPost]
        public ActionResult Edit(AreaModel areaModel)
        {
            ViewBag.Areas = GetCurrentSelectableAreaItems();
            if (!ModelState.IsValid)
            {
                return View(areaModel);
            }

            if (areaModel.Id == Convert.ToInt32(areaModel.ParentId))
            {
                ModelState.AddModelError("ParentId", Resources.Resources.MsgCantSetParentAreaToSelf);
                return View(areaModel);
            }

            Area existing = areaRepository.Get(areaModel.Id);
            existing.Name = areaModel.Name;
            existing.Description = areaModel.Description;
            existing.ParentId = areaModel.ParentId == null ? 0 : int.Parse(areaModel.ParentId);

            areaRepository.Update(existing);

            AuditAreaChange(existing);

            return RedirectToAction("Index");
        }

        private IOrderedEnumerable<SelectListItem> GetCurrentSelectableAreaItems()
        {
            return CurrentAvailableAreas.Select(areaToSelectItem).OrderBy(item => item.Text);
        }

        private void AuditAreaChange(Area existing)
        {
            string changeInfo = new ChangeInfo()
                .AddChange(() => existing.Name)
                .AddChange(() => existing.Description)
                .AddChange(() => existing.ParentId)
                .ToJson();
            auditLogRepository.Add(
                AuditLogBuilder.Builder()
                    .User(HttpContext.User.Identity.Name)
                    .Updated(typeof (Area), existing.Name)
                    .With(changeInfo)
                    .Build());
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (userRepository.FindAll(user => user.AreaId == id, null).Any())
            {
                TempData[MESSAGE_KEY] = Resources.Resources.MsgCanNotDeleteAreaWithUser;
                return RedirectToAction("Index");
            }

            if (deviceRepository.FindAll(device => device.AreaId == id, null).Any())
            {
                TempData[MESSAGE_KEY] = Resources.Resources.MsgCanNotDeleteAreaWithDevice;
                return RedirectToAction("Index");
            }

            if (areaRepository.Count() == 1)
            {
                TempData[MESSAGE_KEY] = Resources.Resources.MsgAtLeastOneAreaInSystem;
                return RedirectToAction("Index");
            }

            Area existing = areaRepository.Get(id);
            areaRepository.Delete(existing);

            auditLogRepository.Add(
                AuditLogBuilder.Builder()
                    .User(HttpContext.User.Identity.Name)
                    .Deleted(typeof (Area), existing.Name)
                    .With(new ChangeInfo().AddChange(() => existing.Name).ToJson())
                    .Build());

            logger.Info("User '{0}' deleted area '{1}'.", Identity.Name, existing.Name);
            return RedirectToAction("Index");
        }
    }
}
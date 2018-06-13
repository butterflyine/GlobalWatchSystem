using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GlobalWatchSystem.Data.Repository;
using GlobalWatchSystem.Helpers;
using GlobalWatchSystem.Models;
using GlobalWatchSystem.Models.ViewModel;
using GlobalWatchSystem.Security;
using NLog;
using SharpRepository.Repository.Queries;

namespace GlobalWatchSystem.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private const string MESSAGE_KEY = "Message";
        private readonly IAreaRepository areaRepository;
        private readonly IAuditLogRepository auditLogRepository;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUserRepository userRepository;


        public AccountController(IUserRepository userRepository, IAreaRepository areaRepository,
            IAuditLogRepository auditLogRepository)
            : base(areaRepository)
        {
            this.userRepository = userRepository;
            this.areaRepository = areaRepository;
            this.auditLogRepository = auditLogRepository;
        }

        public ActionResult Index(int page = 1)
        {
            ViewBag.Message = TempData[MESSAGE_KEY];
            if (TempData.ContainsKey(MESSAGE_KEY))
            {
                ModelState.AddModelError("", ((String) TempData[MESSAGE_KEY]));
            }

            ViewBag.Pagination = new Pagination {CurrentPage = page, TotalCount = userRepository.Count()};

            List<int> areaIds = CurrentAvailableAreas.Select(area => area.Id).ToList();
            IEnumerable<IndexUser> indexUsers = userRepository
                .FindAll(user => areaIds.Contains(user.AreaId),
                    new PagingOptions<User>(page, Pagination.DefaultPageSize, "UserName"))
                .Select(user =>
                    new IndexUser
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Phone = user.Phone,
                        AreaName = areaRepository.Get(user.AreaId) != null ? areaRepository.Get(user.AreaId).Name : ""
                    });

            return View(indexUsers);
        }

        [HttpGet]
        [Authorize(Users = "admin")]
        public ActionResult Create()
        {
            ViewBag.Areas = GetAreaSelectItems();
            return View();
        }

        private IOrderedEnumerable<SelectListItem> GetAreaSelectItems()
        {
            return CurrentAvailableAreas.Select(
                area => new SelectListItem {Text = area.Name, Value = area.Id.ToString()})
                .OrderBy(item => item.Text);
        }

        [HttpPost]
        [Authorize(Users = "admin")]
        public ActionResult Create(User model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Areas = GetAreaSelectItems();
                return View(model);
            }
            if (userRepository.GetByUserName(model.UserName) != null)
            {
                ModelState.AddModelError("UserName", Resources.Resources.MsgUserNameAlreadyExists);
                ViewBag.Areas = GetAreaSelectItems();
                return View(model);
            }


            HashUserPassword(model, model.Password);
            userRepository.Add(model);

            auditLogRepository.Add(AuditLogBuilder.Builder()
                .User(Identity.Name)
                .Added(typeof (User), model.UserName)
                .With(new ChangeInfo().AddChange(() => model.UserName).ToJson())
                .Build());

            logger.Info("User '{0}' created new user '{1}'", Identity.Name, model.UserName);
            return RedirectToAction("Index");
        }

        private static void HashUserPassword(User user, string newPassword)
        {
            user.Password = PasswordHash.CreateHash(newPassword);
            user.ConfirmPassword = user.Password;
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            User existing = userRepository.Get(id);
            if (existing.UserName.Equals("admin"))
            {
                TempData[MESSAGE_KEY] = Resources.Resources.MsgCanNotDeleteAdmin;
                return RedirectToAction("Index");
            }

            userRepository.Delete(existing);

            auditLogRepository.Add(AuditLogBuilder.Builder()
                .User(HttpContext.User.Identity.Name)
                .Deleted(typeof (User), existing.UserName)
                .With(new ChangeInfo().AddChange(() => existing.UserName).ToJson())
                .Build());

            logger.Info("User '{0}' deleted user '{1}'.", Identity.Name, existing.UserName);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            User user = userRepository.Get(id);
            ViewBag.Areas = GetAreaSelectItems();
            return
                View(new EditUserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Phone = user.Phone,
                    Email = user.Email,
                    AreaId = user.AreaId.ToString()
                });
        }

        [HttpPost]
        public ActionResult Edit(EditUserModel user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Areas = GetAreaSelectItems();
                return View(user);
            }

            User updated = userRepository.Get(user.Id);
            updated.ConfirmPassword = updated.Password;
            updated.Phone = user.Phone;
            updated.Email = user.Email;
            updated.AreaId = user.AreaId == null ? 0 : int.Parse(user.AreaId);
            userRepository.Update(updated);

            AuditUserUpdate(updated);

            return RedirectToAction("Index");
        }

        private void AuditUserUpdate(User updated)
        {
            string changeInfo = new ChangeInfo()
                .AddChange(() => updated.Phone)
                .AddChange(() => updated.Email)
                .AddChange(() => updated.AreaId)
                .ToJson();
            auditLogRepository.Add(
                AuditLogBuilder.Builder()
                    .User(HttpContext.User.Identity.Name)
                    .Updated(typeof (User), updated.UserName)
                    .With(changeInfo).Build());
        }

        public ActionResult ChangePassword(int id)
        {
            User user = userRepository.Get(id);
            return View(new ChangePasswordModel {UserId = user.Id, UserName = user.UserName});
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = userRepository.Get(model.UserId);
            HashUserPassword(user, model.Password);
            userRepository.Update(user);

            return Redirect("/");
        }
    }
}
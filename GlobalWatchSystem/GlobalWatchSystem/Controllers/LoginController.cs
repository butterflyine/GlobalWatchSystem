using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GlobalWatchSystem.Data.Repository;
using GlobalWatchSystem.Models;
using GlobalWatchSystem.Models.ViewModel;
using GlobalWatchSystem.Security;
using NLog;

namespace GlobalWatchSystem.Controllers
{
    [AllowAnonymous]
    public class LoginController : BaseController
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public LoginController(IAreaRepository areaRepository) : base(areaRepository)
        {
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    logger.Info("User {0} logged in successfull.", model.UserName);
                    FormsAuthentication.RedirectFromLoginPage(model.UserName, true);
                }
                ModelState.AddModelError("", Resources.Resources.MsgPasswordNotMatch);
            }
            logger.Warn("User {0} logged in failed.", model.UserName);
            return View(model);
        }

        public ActionResult Logout()
        {
            logger.Info("User {0} logged out.", HttpContext.User.Identity.Name);
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
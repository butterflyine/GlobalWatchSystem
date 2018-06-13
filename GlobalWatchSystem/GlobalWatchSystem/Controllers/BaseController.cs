using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using GlobalWatchSystem.Data.Repository;
using GlobalWatchSystem.Models;
using GlobalWatchSystem.Security;

namespace GlobalWatchSystem.Controllers
{
    public class BaseController : Controller
    {
        protected const string CookieKeyCulture = "_culture";
        protected const string CookieKeyCurrentArea = "_currentArea";

        private readonly IAreaRepository areaRepository;


        public BaseController(IAreaRepository areaRepository)
        {
            this.areaRepository = areaRepository;
        }

        public int CurrentAreaId
        {
            get { return Convert.ToInt32(Request.Cookies[CookieKeyCurrentArea].Value); }
        }

        public IList<Area> CurrentAvailableAreas
        {
            get { return areaRepository.GetChildAreas(CurrentAreaId).ToList(); }
        }

        public CustomIdentity Identity
        {
            get
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    return (CustomIdentity) HttpContext.User.Identity;
                }
                return null;
            }
        }

        protected void SetCultureCookie(string culture)
        {
             HttpCookie cookie = Request.Cookies[CookieKeyCulture];
            if (cookie != null)
            {
                cookie.Value = culture;
            }
            else
            {
                cookie = new HttpCookie(CookieKeyCulture) {Value = culture, Expires = DateTime.Now.AddYears(1)};
            }
            Response.Cookies.Add(cookie);
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            SetupCultureInfo();
            SetupUserSwitchAreaList();
            SetupUserDefaultArea();
            SetupCurrentArea();

            return base.BeginExecuteCore(callback, state);
        }

        private void SetupUserSwitchAreaList()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var identity = ((CustomIdentity) HttpContext.User.Identity);
                if (identity.IsAdmin)
                {
                    ViewBag.SwitchAreaList = areaRepository.GetAll().OrderBy(area => area.Name);
                }
                else
                {
                    ViewBag.SwitchAreaList = areaRepository.GetChildAreas(identity.AreaId).OrderBy(area => area.Name);
                }
            }
        }

        private void SetupCurrentArea()
        {
            HttpCookie currentAreaCookie = Request.Cookies[CookieKeyCurrentArea];
            if (currentAreaCookie != null)
            {
                string areaId = currentAreaCookie.Value;
                ViewBag.CurrentArea = areaRepository.Get(Convert.ToInt32(areaId));
            }
        }


        private void SetupCultureInfo()
        {
            string cultureName = null;

            HttpCookie cultureCookie = Request.Cookies[CookieKeyCulture];
            if (cultureCookie != null)
            {
                cultureName = cultureCookie.Value;
            }
            else
            {
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0
                    ? Request.UserLanguages[0]
                    : null;

                if(cultureName == "zh-Hans-CN")
                {
                    cultureName = "zh-CN";
                }
            }
            SetCultureCookie(cultureName);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        protected void SetupUserDefaultArea()
        {
            IIdentity identity = HttpContext.User.Identity;

            if (!identity.IsAuthenticated)
            {
                return;
            }

            if (Request.Cookies[CookieKeyCurrentArea] == null)
            {
                var customIdentity = ((CustomIdentity) identity);
                int areaId = customIdentity.AreaId;
                if (customIdentity.IsAdmin)
                {
                    List<Area> areas = areaRepository.GetAll().ToList();
                    if (areas.Any())
                    {
                        areaId = areas.First().Id;
                    }
                }
                var newCookie = new HttpCookie(CookieKeyCurrentArea, areaId.ToString());
                HttpContext.Request.Cookies.Add(newCookie);
                HttpContext.Response.Cookies.Add(newCookie);
            }
        }
    }
}
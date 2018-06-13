using System.Web.Mvc;
using GlobalWatchSystem.Data.Repository;
using GlobalWatchSystem.Models;
using GlobalWatchSystem.Models.ViewModel;
using SharpRepository.Repository.Queries;

namespace GlobalWatchSystem.Controllers
{
    [Authorize(Users = "admin")]
    public class AuditLogController : BaseController
    {
        private readonly IAuditLogRepository logRepository;

        public AuditLogController(IAuditLogRepository logRepository, IAreaRepository areaRepository) : base(areaRepository)
        {
            this.logRepository = logRepository;
        }

        public ActionResult Index(int page = 1)
        {
            ViewBag.Logs = logRepository.GetAll(new PagingOptions<AuditLog>(page, Pagination.DefaultPageSize, "Dttm", true));
            ViewBag.Pagination = new Pagination
            {
                TotalCount = logRepository.Count(),
                CurrentPage = page
            };
            return View();
        }
    }
}
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using GlobalWatchSystem.Models.ViewModel;

namespace GlobalWatchSystem.Helpers
{
    public static class HtmlHelpsers
    {
        public static MvcHtmlString Pagination(this HtmlHelper html, Pagination pagination, string controller, string action = "index")
        {
            var sb = new StringBuilder();
            if (pagination.CurrentPage > 1)
            {
                sb.AppendLine(html.ActionLink(Resources.Resources.FirstPage, action, controller, new {page = 1}, null).ToHtmlString());
            }
            if (pagination.CurrentPage > 2)
            {
                sb.AppendLine(html.ActionLink(Resources.Resources.PreviousPage, action, controller, new {page = pagination.CurrentPage - 1}, null).ToHtmlString());
            }
            sb.AppendLine(string.Format("{0} {1}/{2}", Resources.Resources.CurrentPage, pagination.CurrentPage, pagination.TotalPageCount));

            if (pagination.CurrentPage + 1 < pagination.TotalPageCount)
            {
                sb.AppendLine(html.ActionLink(Resources.Resources.NextPage, action, controller, new {page = @pagination.CurrentPage + 1}, null).ToHtmlString());
            }
            if (pagination.CurrentPage < pagination.TotalPageCount)
            {
                sb.AppendLine(html.ActionLink(Resources.Resources.LastPage, action, controller, new {page = @pagination.TotalPageCount}, null).ToHtmlString());
            }
            return new MvcHtmlString(sb.ToString());
        }
    }
}
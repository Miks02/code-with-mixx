using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Infrastructure.Filters
{
    public class ToastFilter : IPageFilter
    {
        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (context.HandlerInstance is not PageModel pageModel)
                return;

            if (pageModel.TempData.TryGetValue("ToastTrigger", out var toastTrigger) && toastTrigger is string toastJson)
                context.HttpContext.Response.Headers["HX-Trigger"] = toastJson;
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            if (context.HandlerInstance is not PageModel pageModel)
                return;

            if (context.HttpContext.Response.Headers.TryGetValue("HX-Trigger", out var header) && header.ToString().Contains("showToast") && context.Result is RedirectToPageResult redirect)
                pageModel.TempData["ToastTrigger"] = header.ToString();
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context) { }
    }
}

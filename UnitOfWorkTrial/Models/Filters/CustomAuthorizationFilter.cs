using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UnitOfWorkTrial.Models.Filters
{
    public class CustomAuthorizationFilterr : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool isAuthorized = CheckCustomAuthorizationCondition(context);
            if (!isAuthorized)
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private bool CheckCustomAuthorizationCondition(ActionExecutingContext context)
        {

            if (!context.HttpContext.User.IsInRole("Admin"))
            {
                return true;
            }
            return false;
        }
    }
}
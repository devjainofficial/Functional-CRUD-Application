using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UnitOfWorkTrial.Models.Filters
{
    public class CustomAuthorizationFilter : ActionFilterAttribute
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

            if (!context.HttpContext.User.IsInRole("User"))
            {
                return true;
            }
            return false;
        }
    }
}
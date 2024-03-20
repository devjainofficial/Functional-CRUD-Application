using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UnitOfWorkTrial.Models.Filters
{
    public class AuthorizationExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is UnauthorizedAccessException)
            {
                context.Result = new ObjectResult("Access Denied")
                {
                    StatusCode = 403,
                };

                context.ExceptionHandled = true;
            }
        }
    }
}

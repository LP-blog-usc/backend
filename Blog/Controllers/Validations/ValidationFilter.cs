namespace Blog.Filters
{
    using global::Blog.Controllers.Validations;
    using global::Blog.Models.Dtos.Response;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Linq;

    namespace Blog.Filters
    {
        public class ValidationFilter : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                if (!context.ModelState.IsValid)
                {
                    context.Result = new ValidationFailedResult(context.ModelState);
                }
            }
        }
    }
}

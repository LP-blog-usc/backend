using Blog.Models.Dtos.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers.Validations
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(new ApiResponse<object>
            {
                Success = false,
                Message = "Validation errors occurred.",
                Errors = modelState.Keys
                            .SelectMany(key => modelState[key].Errors.Select(x => new { key, x.ErrorMessage }))
                .GroupBy(x => x.key, x => x.ErrorMessage)
                            .ToDictionary(g => g.Key, g => g.ToList())
            })
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}

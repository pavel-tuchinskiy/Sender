using Domain.Models.Response;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace Web.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ToOk<TResult, TContinue>(this Result<TResult> result, Func<TResult, Task<Result<TContinue>>> continuation = null)
        {
            var res = result.Match(s =>
            {
                if (continuation == null)
                {
                    return new OkObjectResult(new HttpResponseResult(200, s));
                }

                var continueResult = continuation(s).GetAwaiter().GetResult();

                return ToOk<TContinue, TContinue>(continueResult);
            }, f =>
            {
                var exResult = f switch
                {
                    ResponseException ex => new HttpResponseResult
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Exception = new ResponseExceptionDTO(ex.Message)
                    },
                    _ => new HttpResponseResult
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Exception = new ResponseExceptionDTO(f.Message)
                    }
                };

                return new ObjectResult(exResult);
            });

            return res;
        }
    }
}

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
                    return new OkObjectResult(s);
                }

                var continueResult = continuation(s).GetAwaiter().GetResult();

                return ToOk<TContinue, TContinue>(continueResult);
            }, f =>
            {
                if(f is ResponseException exception)
                {
                    return new ObjectResult(exception.Message)
                    {
                        StatusCode = 400
                    };
                }

                return new StatusCodeResult(500);
            });

            return res;
        }
    }
}

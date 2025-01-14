using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Api
{
    public static class JustHello
    {
        [FunctionName("JustHello")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "secured/HelloYou")] HttpRequest req,
            ILogger log, ClaimsPrincipal principal)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            //string responseMessage = string.IsNullOrEmpty(name)
            //    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //    : $"Hello, {name}. This HTTP triggered function executed successfully.";

            //return new OkObjectResult(responseMessage);
            log.LogInformation("C# HTTP trigger function processed a request.");

            bool isClaimValid = true;

            if (principal == null && !principal.Identity.IsAuthenticated)
            {
                log.LogWarning("Request was not authenticated.");
                isClaimValid = false;
            }

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This SECURED HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Bonjour Hi, {name}. This SECURED HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}

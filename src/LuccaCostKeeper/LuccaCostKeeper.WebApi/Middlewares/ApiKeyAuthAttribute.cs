using LuccaCostKeeper.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LuccaCostKeeper.WebApi.Middlewares;

/// <summary>
/// Attribute to check if the request has a valid api key
/// </summary>
public class ApiKeyAuthAttribute : Attribute, IAuthorizationFilter
{
	public void OnAuthorization(AuthorizationFilterContext context)
	{
		if (context != null)
		{
			var key = context.HttpContext.Request.Headers[Constants.API_KEY_HEADER].ToString();
			if (string.IsNullOrEmpty(key)) // try to get from querystring
			{
				key = context.HttpContext.Request.Query[Constants.API_KEY_HEADER].ToString();
			}

			if (string.IsNullOrEmpty(key))
			{
				context.Result = new UnauthorizedObjectResult("Please provide an api key");
				return;
			}

			IConfiguration? configuration = (IConfiguration?)context.HttpContext.RequestServices.GetService(typeof(IConfiguration));
			string apiKey = configuration?["ApiKey"] ?? string.Empty;

			if (!apiKey.Equals(key))
			{
				context.Result = new UnauthorizedObjectResult("Api key is wrong, please contact Lucca support");
				return;
			}
		}
	}
}

using FeatureFlag.Shared.Extensions;
using FeatureFlag.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FeatureFlag.API.Controllers.Infra.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class FeatureFlagAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    #region Ctor
    private readonly IConfiguration _configuration;
    
    public FeatureFlagAuthorizeAttribute(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    #endregion

    #region OnAuthorization
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var headerName = _configuration[AppSettingsKeys.Auth.HeaderName];
        var headerValue = _configuration[AppSettingsKeys.Auth.HeaderValue];
        
        if (!headerName.HasValue() || !headerValue.HasValue())
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        var headers = context.HttpContext.Request.Headers;

        if (!headers.TryGetValue(headerName, out var value) || value != headerValue)
        {
            context.Result = new UnauthorizedResult();
        }
    }
    #endregion
}
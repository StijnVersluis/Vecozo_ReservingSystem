using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ViewLayer.Util
{
    public class CheckAuthorizationMiddleware
    {
        private RequestDelegate _next;

        private User user;
        private UserContainer userContainer;
        private RoleContainer roleContainer;

        private const string AUTH_SESSION_KEY = "VECOZO_AUTH";
        private const string AUTH_SESSION_STATE_KEY = "VECOZO_AUTH_STATE";


        // Converts form data to json.
        private async Task<string> ConvertFormToJson(HttpRequest request)
        {
            var formData = await request.ReadFormAsync();
            var json = "{";
                formData.Keys.ToList().ForEach(key => json += $"\"{key}\": \"{formData[key]}\",");
            json = json.Remove(json.Length - 1, 1);
            json += "}";

            return json;
        }


        // Checks if the user has been authenticated or not based on session value.
        private bool IsAuthenticated(HttpContext context)
        {
            var sessionVal = context.Session.GetString(AUTH_SESSION_KEY);
            return !string.IsNullOrEmpty(sessionVal) && userContainer.FindUserByEmail(sessionVal) != null;
        }


        // Checks if the endpoints does not require authentication.
        private bool EndpointIsAnonymous(HttpContext context)
        {
            return context.GetEndpoint()?.Metadata?.GetMetadata<IAllowAnonymous>() is object;
        }

        // Checks if this endpoint authorises specific authorised roles.
        private string GetAuthorisedRoles(HttpContext context)
        {
            return context.GetEndpoint()?.Metadata?.GetMetadata<RequireAuthAttribute>()?.Roles;
        }

        public CheckAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
            user = new User(new UserDAL());
            userContainer = new(new UserDAL());
            roleContainer = new(new RoleDAL());
        }


        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;

            if (!IsAuthenticated(context))
            {
                user.Logout();
            }

            if (!IsAuthenticated(context) && (request.Path != "/Auth/Login"))
            {
                var hasFormData = request.HasFormContentType;
                if (hasFormData)
                {
                    var json = await ConvertFormToJson(request);
                    context.Session.SetString(AUTH_SESSION_STATE_KEY, json);
                }
            }

            if (!EndpointIsAnonymous(context) && !IsAuthenticated(context) && (request.Path != "/Auth/Login"))
            {
                var referer = HttpUtility.UrlEncode(request.Host.ToString() + request.Path.ToString());
                response.Redirect($"/Auth/Login?redirect_uri={referer}");
                return;
            }

            var authorisedRoles = GetAuthorisedRoles(context);
            if (!String.IsNullOrEmpty(authorisedRoles))
            {
                int roleId = userContainer.GetLoggedInUser().Role;
                string roleName = roleContainer.GetRole(roleId).Name;

                if (IsAuthenticated(context) && !authorisedRoles.Split(",").Contains(roleName))
                {
                    response.Redirect($"/Error?statuscode=401");
                    return;
                }
            }

            await _next(context);
        }
    }
}

using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FlinkUpworkDevApp.Security
{
    public class OpenIdHelper
    {

        public static void UseWithAzureAD(IAppBuilder app)
        {
            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = AzureADSettings.ClientId,
                    Authority = AzureADSettings.Authority,
                    PostLogoutRedirectUri = AzureADSettings.PostLogoutRedirectUri,

                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        // If there is a code in the OpenID Connect response, redeem it for an access token and refresh token, and store those away.
                        AuthorizationCodeReceived = (context) =>
                        {
                            var code = context.Code;
                            string signedInUserID = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                            AzureADHelper.AcquireTokenByAuthorizationCodeAsync(signedInUserID, code);

                            return Task.FromResult(0);
                        }
                    }
                });
        }


        public static void UseWithADFS(IAppBuilder app)
        {
            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = AzureADSettings.ClientId,
                    MetadataAddress = AzureADSettings.MetadataUri,
                    RedirectUri = AzureADSettings.PostLogoutRedirectUri,
                    PostLogoutRedirectUri = AzureADSettings.PostLogoutRedirectUri,

                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        AuthenticationFailed = context =>
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/Error?message=" + context.Exception.Message);
                            return Task.FromResult(0);
                        }

                        //// If there is a code in the OpenID Connect response, redeem it for an access token and refresh token, and store those away.
                        //AuthorizationCodeReceived = (context) =>
                        //{
                        //    var code = context.Code;
                        //    string signedInUserID = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                        //    AzureADHelper.AcquireTokenByAuthorizationCodeAsync(signedInUserID, code);

                        //    return Task.FromResult(0);
                        //}
                    }
                });
        }
    }
}
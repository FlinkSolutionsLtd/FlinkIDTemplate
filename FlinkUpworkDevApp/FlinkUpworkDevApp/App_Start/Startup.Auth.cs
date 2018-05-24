using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Claims;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Owin;
using FlinkUpworkDevApp.Models;
using FlinkUpworkDevApp.AzureAD;

namespace FlinkUpworkDevApp
{
    public partial class Startup
    {


        public void ConfigureAuth(IAppBuilder app)
        {
            //ApplicationDbContext db = new ApplicationDbContext();

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            //Authentication settings
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

 

    }
}

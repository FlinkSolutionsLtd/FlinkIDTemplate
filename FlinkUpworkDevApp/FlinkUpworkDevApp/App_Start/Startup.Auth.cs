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
using FlinkUpworkDevApp.Security;

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
            if(!string.IsNullOrEmpty(AzureADSettings.TenantId))
            {
                OpenIdHelper.UseWithAzureAD(app);
            }
            else
            {
                OpenIdHelper.UseWithADFS(app);
            }
            

            
        }



    }
}

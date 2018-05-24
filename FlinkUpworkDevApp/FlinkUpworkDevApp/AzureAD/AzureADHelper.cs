using FlinkUpworkDevApp.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace FlinkUpworkDevApp.AzureAD
{
    public class AzureADHelper
    {
        /// <summary>
        /// get a token for the Graph without triggering any user interaction (from the cache, via multi-resource refresh token, etc)
        /// </summary>
        /// <returns></returns>
        public static async Task<string> AcquireTokenSilentAsync()
        {
            // initialize AuthenticationContext with the token cache of the currently signed in user, as kept in the app's database
            var authenticationContext = GetContext(SignedInUserId);
            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenSilentAsync(
                AzureADSettings.GraphResourceId, AzureADSettings.ClientCredential, new UserIdentifier(UserObjectId, UserIdentifierType.UniqueId));
            return authenticationResult.AccessToken;
        }

        public static void AcquireTokenByAuthorizationCodeAsync(string signedInUserID, string code)
        {
           
            var authContext = GetContext(signedInUserID);
            AuthenticationResult result = authContext.AcquireTokenByAuthorizationCodeAsync(
                code, new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)),
                AzureADSettings.ClientCredential, AzureADSettings.GraphResourceId).Result;
        }

        public static Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext GetContext(string signedInUserID)
        {
            var authContext = new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext(
                AzureADSettings.Authority,
                //new NaiveSessionCache(signedInUserID));
                new ADALTokenCache(signedInUserID));

            return authContext;
        }

        public static UserAssertion GetUserAssertion()
        {
            var bootstrapContext = ClaimsPrincipal.Current.Identities.First().BootstrapContext as System.IdentityModel.Tokens.BootstrapContext;
            var userAssertion = new UserAssertion(bootstrapContext.Token);
            return userAssertion;
        }

        public static string TenantId
        {
            get { return ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value; }
        }

        public static string UserObjectId
        {
            get { return ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value; }
        }

        public static string SignedInUserId
        {
            get { return ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value; }
        }
    }
}
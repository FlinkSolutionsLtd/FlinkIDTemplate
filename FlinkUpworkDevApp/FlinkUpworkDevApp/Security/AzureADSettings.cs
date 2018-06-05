using FlinkUpworkDevApp.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FlinkUpworkDevApp.Security
{

    public class AzureADSettings
    {
        public static readonly string ClientId = ConfigurationManager.AppSettings["ida:ClientId"];
        public static readonly string ClientSecret = ConfigurationManager.AppSettings["ida:ClientSecret"];
        public static readonly string AADInstance = EnsureTrailingSlash(ConfigurationManager.AppSettings["ida:AADInstance"]);
        public static readonly string TenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        public static readonly string PostLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
        public static readonly string MetadataUri = ConfigurationManager.AppSettings["ida:ADFSMetadata"];

        public static readonly string Authority = AADInstance + TenantId;

        // This is the resource ID of the AAD Graph API.  We'll need this to request a token to call the Graph API.
        public static readonly string GraphResourceId = "https://graph.windows.net";


        private static ClientCredential _clientCredential;

        public static ClientCredential ClientCredential
        {
            get
            {
                if (_clientCredential == null)
                {
                    _clientCredential = new ClientCredential(ClientId, ClientSecret);
                }
                return _clientCredential;
            }
        }

        private static string EnsureTrailingSlash(string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            if (!value.EndsWith("/", StringComparison.Ordinal))
            {
                return value + "/";
            }

            return value;
        }
    }
}
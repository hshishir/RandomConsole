using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace ReleaseManagment
{
    public class HttpHelper
    {
        public static HttpClient GetJsonClient(VstsResource vstsResource)
        {
            var pat = GetPatFromKeyVault(vstsResource);
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", pat))));

            return httpClient;
        }

        public static TestManagementHttpClient GetTestManagementHttpClient()
        {
            var account = ConfigurationManager.AppSettings["Vsts.Account"];
            var teamCollectionUrl = $"https://{account}.visualstudio.com/";
            var pat = HttpHelper.GetPatFromKeyVault(VstsResource.TestManagement);
            var connection = new VssConnection(new Uri(teamCollectionUrl), new VssBasicCredential(string.Empty, pat));
            var client = connection.GetClient<TestManagementHttpClient>();
            return client;
        }

        public static string GetPatFromKeyVault(VstsResource vstsResource)
        {
            var pat = string.Empty;
            switch (vstsResource)
            {
                case VstsResource.Build:
                    break;
                case VstsResource.Release:
                    pat = GetTestResultPat(ConfigurationManager.AppSettings["VstsReleasePat.KeyVaultUri"]);
                    break;
                case VstsResource.TestManagement:
                    pat = GetTestResultPat(ConfigurationManager.AppSettings["VstsTestManagementPat.KeyVaultUri"]);
                    break;
                default:
                    break;
            }

            return pat;
        }

        private static string GetTestResultPat(string secretUri)
        {
            var tokenProvider = new AzureServiceTokenProvider("AuthenticateAs=User");
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));
            var secret = kv.GetSecretAsync(secretUri).Result;
            return secret.Value;
        }
    }

    public enum VstsResource
    {
        Build,
        Release,
        TestManagement
    }

    public enum CredentialType
    {
        VssAad,
        VssPat
    }
}

using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ReleaseManagment
{
    public class HttpHelper
    {
        public static HttpClient GetJsonClient(VstsResource vstsResource)
        {
            var pat = string.Empty;
            switch (vstsResource)
            {
                case VstsResource.Build:
                    break;
                case VstsResource.Release:
                    pat = ConfigurationManager.AppSettings["Release.Pat"];
                    break;
                case VstsResource.TestManagement:
                    pat = ConfigurationManager.AppSettings["TestManagement.Pat"];
                    break;
                default:
                    break;
            }
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", pat))));

            return httpClient;
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

using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReleaseManagment
{
    public class ReleaseApi
    {
        public static async Task<ReleaseResponse> GetRelease(int releaseId)
        {
            try
            {
                var account = ConfigurationManager.AppSettings["Vsts.Account"];
                var project = ConfigurationManager.AppSettings["Vsts.Project"];
                var requestUrl = $"https://{account}.vsrm.visualstudio.com/defaultcollection/{project}/_apis/release/releases/{releaseId}?api-version=3.0-preview.2";
                var jsonClient = HttpHelper.GetJsonClient(VstsResource.Release);
                var response = await jsonClient.GetAsync(requestUrl);
                var result = await response.Content.ReadAsAsync<ReleaseResponse>();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
                return null;
            }
        }

        //public static async Task<ReleaseResponse> GetEnvironments(int releaseId)
        //{
        //    try
        //    {
        //        var account = ConfigurationManager.AppSettings["Vsts.Account"];
        //        var project = ConfigurationManager.AppSettings["Vsts.Project"];
        //        var requestUrl = $"https://{account}.vsrm.visualstudio.com/defaultcollection/{project}/_apis/release/releases/{releaseId}?api-version=3.0-preview.2";
        //        var jsonClient = HttpHelper.GetJsonClient(VstsResource.Release);
        //        var response = await jsonClient.GetAsync(requestUrl);
        //        var result = await response.Content.ReadAsAsync<ReleaseResponse>();
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Exception: {e}");
        //        return null;
        //    }
        //}
    }
}

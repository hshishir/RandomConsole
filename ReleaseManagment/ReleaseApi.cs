using System.Configuration;
using System.Threading.Tasks;

namespace ReleaseManagment
{
    public class ReleaseApi
    {
        public static async Task<string> GetRelease(int releaseId)
        {
            var account = ConfigurationManager.AppSettings["Release.Account"];
            var project = ConfigurationManager.AppSettings["Release.Project"];
            var requestUrl = $"https://{account}.vsrm.visualstudio.com/{project}/_apis/release/releases/{releaseId}";
            var jsonClient = HttpHelper.GetJsonClient(VstsResource.Release);
            var response = await jsonClient.GetAsync(requestUrl);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}

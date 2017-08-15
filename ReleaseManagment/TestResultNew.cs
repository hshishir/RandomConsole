using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.TeamFoundation.Build.WebApi;

//using Microsoft.TeamFoundation.Test.WebApi;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseManagment
{
    public class TestResultNew
    {
        public static async Task<Dictionary<string, int>> GetTestRunData(int releaseId, IEnumerable<string> categoryNames = null)
        {
            var runDict = new Dictionary<string, int>();
            var resultDetails = await GetTestRunDetails(releaseId);
            var results = from r in resultDetails.resultsForGroup
                          where (categoryNames == null) || (categoryNames.Where(c => string.Equals(c, r.groupByValue.name, StringComparison.OrdinalIgnoreCase)).Any())
                          select r.groupByValue;

            foreach (var result in results)
            {
                if (!runDict.Keys.Contains(result.name))
                {
                    runDict.Add(result.name, result.id);
                }
            }
            return runDict;
        }

        public static void GetTestResultUsingClient()
        {
            var project = ConfigurationManager.AppSettings["Vsts.Project"];
            var teamCollectionUrl = "https://devdiv.visualstudio.com/";
            //var buildUrl = "vstfs:///Build/Build/665723";
            var buildUrl = "vstfs:///Build/Build/551173";
            var pat = HttpHelper.GetPatFromKeyVault(VstsResource.TestManagement);
            var connection = new VssConnection(new Uri(teamCollectionUrl), new VssBasicCredential(string.Empty, pat));
            var client = connection.GetClient<TestManagementHttpClient>();
            var testRuns = client.GetTestRunsAsync(project, buildUri: buildUrl).Result;
            
            //var otestRuns = testRuns.OrderByDescending(x => x.Id);
            var rtests = testRuns.Where(x => !x.Name.Trim(' ').Equals("vstest test run", StringComparison.OrdinalIgnoreCase)).OrderByDescending(t => t.Id).ToList();

            Console.WriteLine("Done");
            //var tc = client.GetTestRunByIdAsync(project, 448953).Result;
            //var tc = client.GetTestCaseResultsAsync(project, 449207).Result;
            var tc = client.GetTestResultsAsync(project, 449207).Result;

            var tcn = tc.Where(x => x.Build.Id.Equals(665723));
            Console.WriteLine("");


            
        }

        public static void GetVariables()
        {
            var project = ConfigurationManager.AppSettings["Vsts.Project"];
            var teamCollectionUrl = "https://devdiv.visualstudio.com/";
            //var buildUrl = "vstfs:///Build/Build/665723";
            var buildUrl = "vstfs:///Build/Build/551173";
            var pat = HttpHelper.GetPatFromKeyVault(VstsResource.TestManagement);
            var connection = new VssConnection(new Uri(teamCollectionUrl), new VssBasicCredential(string.Empty, pat));
            //var client = connection.GetClient<>();
        }

        private static async Task<TestRunResultDetailsResponse> GetTestRunDetails(int releaseId)
        {
            var account = ConfigurationManager.AppSettings["Vsts.Account"];
            var project = ConfigurationManager.AppSettings["Vsts.Project"];
            var requestUrl = $"https://{account}.visualstudio.com/{project}/_apis/test/ResultDetailsByRelease?releaseId={releaseId}&releaseEnvId=0&publishContext=CD&groupBy=TestRun&%24filter=Outcome+eq+Failed&%24orderby=TestCaseTitle+asc";

            var jsonClient = HttpHelper.GetJsonClient(VstsResource.TestManagement);
            var response = await jsonClient.GetAsync(requestUrl);
            //var result = await response.Content.ReadAsStringAsync();
            //var jsonObj = JObject.Parse(result);
            var result = await response.Content.ReadAsAsync<TestRunResultDetailsResponse>();
            return result;
        }
    }
}

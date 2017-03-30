using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using ReleaseManagment.TestRun;

namespace ReleaseManagment
{
    public class TestManagementApi
    {

        //public static async Task<IEnumerable<TestRunResult>> GetTestRunResults(string branchName, string buildNumber, RunType runType)
        //{
        //    var resultSummary = await GetTestResultSummary(releaseId);
        //    var testRunResults = new List<TestRunResult>();
        //    return testRunResults;
        //}
        public static async Task<TestRunResult> GetTestRunResult(int releaseId)
        {
            var resultSummary = await GetTestResultSummary(releaseId);
            var testRunResult = new TestRunResult
            {
                ReleaseId = releaseId,
                TotalCount = resultSummary.aggregatedResultsAnalysis.totalTests,
                PassedCount = (resultSummary.aggregatedResultsAnalysis.resultsByOutcome.Passed != null)
                ? resultSummary.aggregatedResultsAnalysis.resultsByOutcome.Passed.count : 0,
                FailedCount = (resultSummary.aggregatedResultsAnalysis.resultsByOutcome.Failed != null) 
                ? resultSummary.aggregatedResultsAnalysis.resultsByOutcome.Failed.count : 0

            };

            var categories = await GetTestCategories(releaseId);
            Build buildData = null;
            foreach (var category in categories)
            {
                var testCaseResults = await GetTestCaseResults(category);
                category.TestCaseResults = testCaseResults.Item1;
                if(buildData == null)
                {
                    buildData = testCaseResults.Item2;
                }
            }

            testRunResult.Category = categories;

            if (buildData == null)
            {
                return testRunResult;
            }

            testRunResult.BuildUrl = buildData.url;
            var tokens = buildData.name.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            testRunResult.BranchName = tokens[0];
            testRunResult.BuildNumber = tokens[1];
            return testRunResult;
        }

        private static async Task<IEnumerable<TestCategory>> GetTestCategories(int releaseId)
        {
            var resultDetails = await GetTestRunDetails(releaseId);
            var testCategories = new List<TestCategory>();
            foreach (var rg in resultDetails.resultsForGroup)
            {
                var tc = new TestCategory
                {
                    Name = rg.groupByValue.name,
                    RunId = rg.groupByValue.id,
                    PassCount = rg.resultsCountByOutcome.Passed != null ? rg.resultsCountByOutcome.Passed.count : 0,
                    FailCount = rg.resultsCountByOutcome.Failed != null ? rg.resultsCountByOutcome.Failed.count : 0
                };

                testCategories.Add(tc);
            }

            return testCategories;
        }
        private static async Task<Tuple<IEnumerable<TestCaseResult>, Build>> GetTestCaseResults(TestCategory testCategory)
        {
            var testCaseResult = new List<TestCaseResult>();
            var result = await GetTestResult(testCategory.RunId);
            Build buildData = null;
            foreach (var item in result.value)
            {
                TestCaseOutcome outcome;
                OverallState state;
                Enum.TryParse<TestCaseOutcome>(item.outcome, out outcome);
                Enum.TryParse<OverallState>(item.state, out state);
                testCaseResult.Add(new TestCaseResult
                {
                    TestName = item.testCaseTitle,
                    TestType = item.automatedTestType,
                    StartDate = item.startedDate,
                    CompletedDate = item.completedDate,
                    Outcome = outcome,
                });

                if(buildData == null)
                {
                    buildData = item.build;
                }
            }

            return new Tuple<IEnumerable<TestCaseResult>, Build>(testCaseResult, buildData);
        }

        
        //private static async Task<IEnumerable<TestCaseResult>> GetTestCaseResults(TestCategory testCategory)
        //{
        //    var testCaseResult = new List<TestCaseResult>();
        //    var result = await GetTestResult(testCategory.RunId);
        //    foreach (var item in result.value)
        //    {
        //        Outcome outcome;
        //        Enum.TryParse<Outcome>(item.outcome, out outcome);
        //        testCaseResult.Add(new TestCaseResult
        //        {
        //            Name = item.testCase.name,
        //            StartDate = item.startedDate,
        //            CompletedDate = item.completedDate,
        //            Outcome = outcome
        //        });
        //    }

        //    return testCaseResult;
        //}

        private static async Task<ResultSummary> GetTestResultSummary(int releaseId)
        {
            var account = ConfigurationManager.AppSettings["Vsts.Account"];
            var project = ConfigurationManager.AppSettings["Vsts.Project"];
            var requestUrl = $"https://{account}.visualstudio.com/{project}/_apis/test/ResultSummaryByRelease?releaseId={releaseId}&releaseEnvId=0&publishContext=CD&includeFailureDetails=true";
            var jsonClient = HttpHelper.GetJsonClient(VstsResource.TestManagement);
            var response = await jsonClient.GetAsync(requestUrl);
            //var result = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadAsAsync<ResultSummary>();
            return result;
        }

        public static async Task<TestResult> GetTestResult(int runId)
        {
            var account = ConfigurationManager.AppSettings["Vsts.Account"];
            var project = ConfigurationManager.AppSettings["Vsts.Project"];
            var requestUrl = $"https://{account}.visualstudio.com/{project}/_apis/test/runs/{runId}/results";

            var jsonClient = HttpHelper.GetJsonClient(VstsResource.TestManagement);
            var response = await jsonClient.GetAsync(requestUrl);
            var result = await response.Content.ReadAsAsync<TestResult>();
            return result;
        }

        private static async Task<ResultDetails> GetTestRunDetails(int releaseId)
        {
            var account = ConfigurationManager.AppSettings["Vsts.Account"];
            var project = ConfigurationManager.AppSettings["Vsts.Project"];
            var requestUrl = $"https://{account}.visualstudio.com/{project}/_apis/test/ResultDetailsByRelease?releaseId={releaseId}&releaseEnvId=0&publishContext=CD&groupBy=TestRun&%24filter=Outcome+eq+Failed&%24orderby=TestCaseTitle+asc";

            var jsonClient = HttpHelper.GetJsonClient(VstsResource.TestManagement);
            var response = await jsonClient.GetAsync(requestUrl);
            //var result = await response.Content.ReadAsStringAsync();
            //var jsonObj = JObject.Parse(result);
            var result = await response.Content.ReadAsAsync<ResultDetails>();
            return result;
        }

        public static async Task<TestRunResponse> GetTestRuns(string buildUri)
        {
            var jsonClient = HttpHelper.GetJsonClient(VstsResource.TestManagement);
            var account = ConfigurationManager.AppSettings["Vsts.Account"];
            var project = ConfigurationManager.AppSettings["Vsts.Project"];
            var requestUrl = $"https://{account}.visualstudio.com/DefaultCollection/{project}/_apis/test/runs?api-version=1.0&buildUri={buildUri}";
            var response = await jsonClient.GetAsync(requestUrl);
            var result = await response.Content.ReadAsAsync<TestRunResponse>();
            return result;
        }

        //public static async Task<IEnumerable<TestResult>> GetTestResults(int releaseId)
        //{
        //    var testResults = new List<TestResult>();
        //    var runIds = await GetTestRunIds(releaseId);
        //    foreach (var runId in runIds)
        //    {
        //        var result = await GetTestResult(runId);
        //        testResults.Add(result);
        //    }

        //    return testResults;
        //}

        //private static async Task<IEnumerable<Resultsforgroup>> GetResultsForGroup(int releaseId)
        //{
        //    var resultDetails = await GetTestRunDetails(releaseId);
        //    return resultDetails.resultsForGroup;
        //}

        //private static async Task<IEnumerable<int>> GetTestRunIds(int releaseId)
        //{
        //    var runIds = new List<int>();
        //    var testGroups = await GetResultsForGroup(releaseId);

        //    foreach (var rg in testGroups)
        //    {
        //        runIds.Add(rg.groupByValue.id);
        //    }

        //    return runIds;
        //}

        public static async Task<string> GetBuildTestResults()
        {
            var jsonClient = HttpHelper.GetJsonClient(VstsResource.TestManagement);
            var account = ConfigurationManager.AppSettings["Vsts.Account"];
            var project = ConfigurationManager.AppSettings["Vsts.Project"];
            var buildUrl = "https://devdiv.visualstudio.com/_apis/build/Builds/551173";

            var requestUrl = $"https://{account}.visualstudio.com/{project}/_apis/test/runs?api-version=1.0&buildUri={buildUrl}";
            var response = await jsonClient.GetAsync(requestUrl);
            var result = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(result);
            return result;
        }

        public static void GetTestResultUsingClient()
        {
            var project = ConfigurationManager.AppSettings["Vsts.Project"];
            var buildUrl = "https://devdiv.visualstudio.com/_apis/build/Builds/551173";
            var teamCollectionUrl = "https://devdiv.visualstudio.com/";
            var connection = new VssConnection(new Uri(teamCollectionUrl), new VssCredentials());
            var client = connection.GetClient<TestManagementHttpClient>();
            var testRuns = client.GetTestRunsAsync(projectId: project, buildUri: buildUrl).Result;
            Console.WriteLine("Done" );
        }
    }
}

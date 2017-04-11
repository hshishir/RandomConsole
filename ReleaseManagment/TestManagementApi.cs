using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Newtonsoft.Json.Linq;
using ReleaseManagment.TestRun;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections;

namespace ReleaseManagment
{
    public class TestManagementApi
    {

        //public static async Task<TestRunResult> GetTestRunResult(int releaseId)
        //{
        //    var resultSummary = await GetTestResultSummary(releaseId);
        //    var testRunResult = new TestRunResult
        //    {
        //        ReleaseId = releaseId,
        //        TotalCount = resultSummary.AggregatedResultsAnalysis.TotalTests,
        //        PassCount = (resultSummary.AggregatedResultsAnalysis.ResultsByOutcome.Passed != null)
        //        ? resultSummary.AggregatedResultsAnalysis.ResultsByOutcome.Passed.Count : 0,
        //        FailCount = (resultSummary.AggregatedResultsAnalysis.ResultsByOutcome.Failed != null) 
        //        ? resultSummary.AggregatedResultsAnalysis.ResultsByOutcome.Failed.Count : 0

        //    };

        //    var testCategoryResults = await GetTestCategoryResults(releaseId);
        //    Build buildData = null;
        //    foreach (var category in testCategoryResults)
        //    {
        //        var testCaseResults = await GetTestCaseResults(category);
        //        category.TestCaseResults = testCaseResults.Item1;
        //        if(buildData == null)
        //        {
        //            buildData = testCaseResults.Item2;
        //        }
        //    }

        //    testRunResult.TestCategoryResults = testCategoryResults;

        //    if (buildData == null)
        //    {
        //        return testRunResult;
        //    }

        //    testRunResult.BuildUrl = buildData.Url;
        //    var tokens = buildData.Name.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
        //    testRunResult.BranchName = tokens[0];
        //    testRunResult.BuildNumber = tokens[1];
        //    return testRunResult;
        //}

        public static async Task<TestRunResult> GetTestRunResult(int releaseId, string categoryName)
        {
            var testCategoryResults = await GetTestCategoryResults(releaseId);
            var testCategory = testCategoryResults.Where(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (testCategory == null)
            {
                return null;
            }
            var testRunResult = new TestRunResult
            {
                ReleaseId = releaseId,
                PassCount = testCategory.PassCount,
                FailCount = testCategory.FailCount,
                TotalCount = testCategory.PassCount + testCategory.FailCount
            };

            var testCaseResults = await GetTestCaseResults(testCategory);
            testCategory.TestCaseResults = testCaseResults.Item1;
            var buildData = testCaseResults.Item2;
            testRunResult.TestCategoryResults = new List<TestCategoryResult>
            {
                testCategory
            };

            if (buildData != null)
            {
                testRunResult.BuildUrl = buildData.Url;
                var tokens = buildData.Name.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                testRunResult.BranchName = tokens[0];
                testRunResult.BuildNumber = tokens[1];
            }

            return testRunResult;
        }

        public static async Task<TestRunResult> GetTestRunResult(int releaseId, IEnumerable<string> categoryNames = null)
        {
            var testCategoryResults = await GetTestCategoryResults(releaseId);

            //var testCategories =
            //    from s in testCategoryResults
            //    where (categoryNames.Where(c => string.Equals(s.Name, c, StringComparison.OrdinalIgnoreCase)).Any())
            //    select s;

            var testCategories =
                from s in testCategoryResults
                where categoryNames == null || (categoryNames.Where(c => string.Equals(s.Name, c, StringComparison.OrdinalIgnoreCase)).Any())
                select s;

            var runPassCount = 0;
            var runFailCount = 0;
            var runTotalCount = 0;

            Build buildData = null;

            foreach (var testCategory in testCategories)
            {
                var testCaseResults = await GetTestCaseResults(testCategory);
                testCategory.TestCaseResults = testCaseResults.Item1;

                // Update test category overall state and pass and fail count
                var dict = GetTestCaseRunStates(testCaseResults.Item1);
                var overallCategoryState = GetCategoryOverallState(dict);

                testCategory.RunState = overallCategoryState;

                testCategory.PassCount = dict.ContainsKey(TestCaseOutcome.Passed) ? dict[TestCaseOutcome.Passed] : 0;
                testCategory.FailCount = (dict.ContainsKey(TestCaseOutcome.Failed) ? dict[TestCaseOutcome.Failed] : 0) +
                    (dict.ContainsKey(TestCaseOutcome.Aborted) ? dict[TestCaseOutcome.Aborted] : 0) +
                    (dict.ContainsKey(TestCaseOutcome.NotExecuted) ? dict[TestCaseOutcome.NotExecuted] : 0) +
                    (dict.ContainsKey(TestCaseOutcome.Timeout) ? dict[TestCaseOutcome.Timeout] : 0) +
                    (dict.ContainsKey(TestCaseOutcome.Inconclusive) ? dict[TestCaseOutcome.Inconclusive] : 0);

                testCategory.RunningCount = dict.ContainsKey(TestCaseOutcome.Error) ? dict[TestCaseOutcome.Error] : 0;

                runPassCount += testCategory.PassCount;
                runFailCount += testCategory.FailCount;
                runTotalCount += testCategory.TestCaseResults.Count();

                if (buildData == null)
                {
                    buildData = testCaseResults.Item2;
                }
            }

            var testRunResult = new TestRunResult
            {
                ReleaseId = releaseId,
                PassCount = runPassCount,
                FailCount = runFailCount,
                TotalCount = runTotalCount
            };

            testRunResult.TestCategoryResults = testCategories.OrderBy(x => x.Name).ToList();

            if (buildData != null)
            {
                testRunResult.BuildUrl = buildData.Url;
                var tokens = buildData.Name.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                testRunResult.BranchName = tokens[0];
                testRunResult.BuildNumber = tokens[1];
            }

            return testRunResult;
        }


        private static OverallState GetCategoryOverallState(Dictionary<TestCaseOutcome, int> categoryTestCaseOutcomes)
        {
            if (categoryTestCaseOutcomes.Keys.Contains(TestCaseOutcome.Error))
            {
                return OverallState.Running;
            }
            else if (categoryTestCaseOutcomes.Keys.Contains(TestCaseOutcome.Aborted) ||
                    categoryTestCaseOutcomes.Keys.Contains(TestCaseOutcome.Failed) ||
                    categoryTestCaseOutcomes.Keys.Contains(TestCaseOutcome.Inconclusive) ||
                    categoryTestCaseOutcomes.Keys.Contains(TestCaseOutcome.NotExecuted) ||
                    categoryTestCaseOutcomes.Keys.Contains(TestCaseOutcome.Timeout))
            {
                return OverallState.Failed;
            }
            else
            {
                return OverallState.Passed;
            }
        }

        private static Dictionary<TestCaseOutcome, int> GetTestCaseRunStates(IEnumerable<TestCaseResult> testCaseResults)
        {
            var dict = new Dictionary<TestCaseOutcome, int>();
            foreach (var testCase in testCaseResults)
            {
                if (dict.Keys.Contains(testCase.Outcome))
                {
                    dict[testCase.Outcome]++;
                }
                else
                {
                    dict.Add(testCase.Outcome, 1);
                }
            }

            return dict;
        }

        

        private static async Task<IEnumerable<TestCategoryResult>> GetTestCategoryResults(int releaseId)
        {
            var resultDetails = await GetTestRunDetails(releaseId);
            var testCategoryResults = new List<TestCategoryResult>();
            foreach (var rg in resultDetails.resultsForGroup)
            {
                var tc = new TestCategoryResult
                {
                    Name = rg.groupByValue.name,
                    RunId = rg.groupByValue.id,
                    PassCount = rg.resultsCountByOutcome.Passed != null ? rg.resultsCountByOutcome.Passed.Count : 0,
                    FailCount = rg.resultsCountByOutcome.Failed != null ? rg.resultsCountByOutcome.Failed.Count : 0
                };

                testCategoryResults.Add(tc);
            }

            return testCategoryResults;
        }

        private static async Task<Tuple<IEnumerable<TestCaseResult>, Build>> GetTestCaseResults(TestCategoryResult testCategory)
        {
            var testCaseResult = new List<TestCaseResult>();
            var result = await GetTestCaseResult(testCategory.RunId);
            Build buildData = null;
            foreach (var item in result.Value)
            {
                TestCaseOutcome outcome;
                OverallState state;
                Enum.TryParse<TestCaseOutcome>(item.Outcome, out outcome);
                Enum.TryParse<OverallState>(item.State, out state);
                testCaseResult.Add(new TestCaseResult
                {
                    TestName = item.TestCaseTitle,
                    TestType = item.AutomatedTestType,
                    StartDate = item.StartedDate,
                    CompletedDate = item.CompletedDate,
                    Outcome = outcome,
                });

                if (buildData == null)
                {
                    buildData = item.Build;
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

        private static async Task<TestRunResultSummaryResponse> GetTestResultSummary(int releaseId)
        {
            var account = ConfigurationManager.AppSettings["Vsts.Account"];
            var project = ConfigurationManager.AppSettings["Vsts.Project"];
            var requestUrl = $"https://{account}.visualstudio.com/{project}/_apis/test/ResultSummaryByRelease?releaseId={releaseId}&releaseEnvId=0&publishContext=CD&includeFailureDetails=true";
            var jsonClient = HttpHelper.GetJsonClient(VstsResource.TestManagement);
            var response = await jsonClient.GetAsync(requestUrl);
            var result = await response.Content.ReadAsAsync<TestRunResultSummaryResponse>();
            return result;
        }

        public static async Task<TestCaseResultsResponse> GetTestCaseResult(int runId)
        {
            var account = ConfigurationManager.AppSettings["Vsts.Account"];
            var project = ConfigurationManager.AppSettings["Vsts.Project"];
            var requestUrl = $"https://{account}.visualstudio.com/{project}/_apis/test/runs/{runId}/results";

            var jsonClient = HttpHelper.GetJsonClient(VstsResource.TestManagement);
            var response = await jsonClient.GetAsync(requestUrl);
            var result = await response.Content.ReadAsAsync<TestCaseResultsResponse>();
            return result;
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

        //public static void GetTestResultUsingClient()
        //{
        //    var project = ConfigurationManager.AppSettings["Vsts.Project"];
        //    var buildUrl = "https://devdiv.visualstudio.com/_apis/build/Builds/551173";
        //    var teamCollectionUrl = "https://devdiv.visualstudio.com/";
        //    var connection = new VssConnection(new Uri(teamCollectionUrl), new VssCredentials());
        //    var client = connection.GetClient<TestManagementHttpClient>();
        //    var testRuns = client.GetTestRunsAsync(projectId: project, buildUri: buildUrl).Result;
        //    Console.WriteLine("Done");
        //}
    }
}

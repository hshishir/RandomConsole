using Microsoft.TeamFoundation.TestManagement.WebApi;
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
    public class TestResult
    {
        public static async Task<Dictionary<string,int>> GetTestRunData(int releaseId, IEnumerable<string> categoryNames = null)
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

        public static async Task<TestRunResult> GetTestRunResult(int releaseId, Dictionary<string,int> testRunData)
        {
            var testCategorResults = new List<TestCategoryResult>();
            var runPassCount = 0;
            var runFailCount = 0;
            var runTotalCount = 0;
            Build buildData = null;
            var runState = OverallState.Passed;

            foreach (var runData in testRunData)
            {
                var testCategoryResult = new TestCategoryResult
                {
                    Name = runData.Key,
                    RunId = runData.Value
                };

                var testCaseResults = await GetTestCaseResults(testCategoryResult);

                testCategoryResult.TestCaseResults = testCaseResults;

                // Update test category overall state and pass and fail count
                var dict = GetTestCaseRunStates(testCaseResults);
                var overallCategoryState = GetCategoryOverallState(dict);

                testCategoryResult.RunState = overallCategoryState;
                if (testCategoryResult.RunState.Equals(OverallState.Failed))
                {
                    runState = OverallState.Failed;
                }

                testCategoryResult.PassCount = dict.ContainsKey(TestCaseOutcome.Passed) ? dict[TestCaseOutcome.Passed] : 0;
                testCategoryResult.FailCount = (dict.ContainsKey(TestCaseOutcome.Failed) ? dict[TestCaseOutcome.Failed] : 0) +
                    (dict.ContainsKey(TestCaseOutcome.Aborted) ? dict[TestCaseOutcome.Aborted] : 0) +
                    (dict.ContainsKey(TestCaseOutcome.NotExecuted) ? dict[TestCaseOutcome.NotExecuted] : 0) +
                    (dict.ContainsKey(TestCaseOutcome.Timeout) ? dict[TestCaseOutcome.Timeout] : 0) +
                    (dict.ContainsKey(TestCaseOutcome.Inconclusive) ? dict[TestCaseOutcome.Inconclusive] : 0);

                testCategoryResult.RunningCount = dict.ContainsKey(TestCaseOutcome.Error) ? dict[TestCaseOutcome.Error] : 0;

                runPassCount += testCategoryResult.PassCount;
                runFailCount += testCategoryResult.FailCount;
                runTotalCount += testCategoryResult.TestCaseResults.Count();

                if (buildData == null)
                {
                    buildData = testCategoryResult.TestCaseResults.ElementAt(0).Build;
                }

                var projectName = testCategoryResult.TestCaseResults.ElementAt(0).Project.Name;
                testCategoryResult.ResultUrl = $"https://{projectName}.visualstudio.com/{projectName}/_TestManagement/Runs?runId={runData.Value}&_a=runCharts";

                testCategorResults.Add(testCategoryResult);
            }

            var testRunResult = new TestRunResult
            {
                ReleaseId = releaseId,
                PassCount = runPassCount,
                FailCount = runFailCount,
                TotalCount = runTotalCount,
                RunState = runState
            };

            testRunResult.TestCategoryResults = testCategorResults.OrderBy(x => x.Name).ToList();

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
            if (categoryTestCaseOutcomes.Keys.Contains(TestCaseOutcome.Aborted) ||
                    categoryTestCaseOutcomes.Keys.Contains(TestCaseOutcome.Failed) ||
                    categoryTestCaseOutcomes.Keys.Contains(TestCaseOutcome.Inconclusive) ||
                    categoryTestCaseOutcomes.Keys.Contains(TestCaseOutcome.NotExecuted) ||
                    categoryTestCaseOutcomes.Keys.Contains(TestCaseOutcome.Timeout) ||
                    categoryTestCaseOutcomes.Keys.Contains(TestCaseOutcome.Error))
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

        //private static async Task<Tuple<IEnumerable<TestCaseResult>, Build>> GetTestCaseResults(TestCategoryResult testCategory)
        private static async Task<IEnumerable<TestCaseResult>> GetTestCaseResults(TestCategoryResult testCategory)
        {
            var testCaseResults = new List<TestCaseResult>();
            var result = await GetTestCaseResult(testCategory.RunId);
            Build buildData = null;
            foreach (var item in result.Value)
            {
                TestCaseOutcome outcome;
                OverallState state;
                Enum.TryParse<TestCaseOutcome>(item.Outcome, out outcome);
                Enum.TryParse<OverallState>(item.State, out state);
                testCaseResults.Add(new TestCaseResult
                {
                    TestName = item.TestCaseTitle,
                    TestType = item.AutomatedTestType,
                    StartDate = item.StartedDate,
                    CompletedDate = item.CompletedDate,
                    Build = item.Build,
                    Project = item.Project,
                    Outcome = outcome,
                });

                if (buildData == null)
                {
                    buildData = item.Build;
                }
            }

            return testCaseResults;
            //return new Tuple<IEnumerable<TestCaseResult>, Build>(testCaseResult, buildData);
        }

        private static async Task<TestCaseResultsResponse> GetTestCaseResult(int runId)
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

        //public static void GetTestResultUsingClient()
        //{
        //    var project = ConfigurationManager.AppSettings["Vsts.Project"];
        //    var buildUrl = "https://devdiv.visualstudio.com/_apis/build/Builds/551173";
        //    var teamCollectionUrl = "https://devdiv.visualstudio.com/";
        //    var connection = new VssConnection(new Uri(teamCollectionUrl), new VssCredentials());
        //    var client = connection.GetClient<TestManagementHttpClient>();
        //    var testRuns = client.GetTestRunsAsync(projectId: project, buildUri: buildUrl).Result;
        //   // var testResults = client.GetTestResultsAsync(project, )
        //    Console.WriteLine("Done");
        //}
    }
}

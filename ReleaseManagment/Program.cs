using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseManagment
{
    class Program
    {
        static void Main(string[] args)
        {
            //var rel = ReleaseApi.GetRelease(28526).Result;
            //Console.WriteLine("Complete");
            //var releaseId = 25572;
            //var dtlReleaseIdWithFailures = 26113;
            //var dtlReleaseIdAllPasses = 26375;
            //var releaseId = 28591;
            ////var testRunResult = TestManagementApi.GetTestRunResult(releaseId).Result;
            //var ddritResult = TestManagementApi.GetTestRunResult(dtlReleaseIdAllPasses).Result;

            //var runData = TestResult.GetTestRunData(dtlReleaseIdAllPasses, new[] {"ddrits", "setup - all"}).Result;
            //var runResult = TestResult.GetTestRunResult(dtlReleaseIdAllPasses, runData).Result;

            //var testResults = TestManagementApi.GetTestRunResult(releaseId, new[] { "Setup - Minimal" }).Result;


            //var buildUri = "vstfs:///Build/Build/702767";
            //var testRuns = TestManagementApi.GetTestRuns(buildUri).Result;
            //var testRunList = testRuns.value.OrderByDescending(x => x.id).ToList();

            //TestResultNew.GetTestResultUsingClient();

            var runId = 597166;
            //var response = TestManagementApi.GetTestCaseResult(runId).Result;

            //var testRun = TestManagementApi.GetTestRun(runId).Result;

            var sbuilder = new StringBuilder();
            sbuilder.AppendLine("## Append Another Link ");
            sbuilder.AppendLine("#### [Link to MSDN](http://www.msdn.com/)");

            var returnCode = TestManagementApi.UpdateTestRun(runId, sbuilder.ToString(), true).Result;
            Console.WriteLine("Completed!!");

            
        }
    }
}

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
            //var releaseId = 25572;
            var dtlReleaseId = 26113;
            var releaseId = 25887;
            //var testRunResult = TestManagementApi.GetTestRunResult(releaseId).Result;
            var ddritResult = TestManagementApi.GetTestRunResult(releaseId).Result;

            var testResults = TestManagementApi.GetTestRunResult(releaseId, new[] { "ddrits","setup - all", "setup - minimal"}).Result;


            var buildUri = "vstfs:///Build/Build/639868";
            var testRuns = TestManagementApi.GetTestRuns(buildUri).Result;
            var testRunList = testRuns.value.OrderByDescending(x => x.id).ToList();

            
            Console.WriteLine("Completed!!");

            
        }
    }
}

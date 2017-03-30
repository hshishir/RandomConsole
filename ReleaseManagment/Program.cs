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
            var releaseId = 25956;
            var testResult = TestManagementApi.GetTestRunResult(releaseId).Result;

            //var releaseObject = ReleaseApi.GetRelease(releaseId).Result;

            var buildUri = "vstfs:///Build/Build/641677";
            var testRuns = TestManagementApi.GetTestRuns(buildUri).Result;
            var testRunList = testRuns.value.OrderByDescending(x => x.id).ToList();

            
            Console.WriteLine("Completed!!");

            
        }
    }
}

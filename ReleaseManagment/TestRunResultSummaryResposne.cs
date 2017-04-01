using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseManagment
{

    public class TestRunResultSummaryResponse
    {
        public Aggregatedresultsanalysis AggregatedResultsAnalysis { get; set; }
        public Testresultscontext TestResultsContext { get; set; }
        public Teamproject TeamProject { get; set; }
    }

    public class Aggregatedresultsanalysis
    {
        public Previouscontext PreviousContext { get; set; }
        public int TotalTests { get; set; }
        public string Duration { get; set; }
        public Resultsbyoutcome ResultsByOutcome { get; set; }
    }

    public class Previouscontext
    {
        public int ContextType { get; set; }
        public object Build { get; set; }
        public object Release { get; set; }
    }

    public class Resultsbyoutcome
    {
        public Total Total { get; set; }
        public Executed Executed { get; set; }
        public Passed Passed { get; set; }
        public Failed Failed { get; set; }
    }

    public class Passed
    {
        public string Outcome { get; set; }
        public int Count { get; set; }
        public string Duration { get; set; }
    }

    public class Testresultscontext
    {
        public string ContextType { get; set; }
        public object Build { get; set; }
        public Release Release { get; set; }
    }

    public class Release
    {
        public int Id { get; set; }
        public object Name { get; set; }
        public int EnvironmentId { get; set; }
        public object EnvironmentName { get; set; }
        public int DefinitionId { get; set; }
        public int EnvironmentDefinitionId { get; set; }
        public object EnvironmentDefinitionName { get; set; }
    }

    public class Teamproject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string State { get; set; }
    }

}

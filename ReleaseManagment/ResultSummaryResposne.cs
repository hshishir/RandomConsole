using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseManagment
{

    public class ResultSummary
    {
        public Aggregatedresultsanalysis aggregatedResultsAnalysis { get; set; }
        public Testresultscontext testResultsContext { get; set; }
        public Teamproject teamProject { get; set; }
    }

    public class Aggregatedresultsanalysis
    {
        public Previouscontext previousContext { get; set; }
        public int totalTests { get; set; }
        public string duration { get; set; }
        public Resultsbyoutcome resultsByOutcome { get; set; }
    }

    public class Previouscontext
    {
        public int contextType { get; set; }
        public object build { get; set; }
        public object release { get; set; }
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
        public string outcome { get; set; }
        public int count { get; set; }
        public string duration { get; set; }
    }

    public class Testresultscontext
    {
        public string contextType { get; set; }
        public object build { get; set; }
        public Release release { get; set; }
    }

    public class Release
    {
        public int id { get; set; }
        public object name { get; set; }
        public int environmentId { get; set; }
        public object environmentName { get; set; }
        public int definitionId { get; set; }
        public int environmentDefinitionId { get; set; }
        public object environmentDefinitionName { get; set; }
    }

    public class Teamproject
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string state { get; set; }
    }

}

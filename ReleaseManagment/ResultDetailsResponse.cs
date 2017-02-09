using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseManagment
{
    public class ResultDetails
    {
        public string groupByField { get; set; }
        public Resultsforgroup[] resultsForGroup { get; set; }
    }

    public class Resultsforgroup
    {
        public Groupbyvalue groupByValue { get; set; }
        public Resultscountbyoutcome resultsCountByOutcome { get; set; }
        public object[] results { get; set; }
    }

    public class Groupbyvalue
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool isAutomated { get; set; }
        public DateTime completedDate { get; set; }
        public string state { get; set; }
        public int totalTests { get; set; }
        public int incompleteTests { get; set; }
        public int notApplicableTests { get; set; }
        public int passedTests { get; set; }
        public int unanalyzedTests { get; set; }
        public int revision { get; set; }
        public string releaseEnvironmentUri { get; set; }
    }

    public class Resultscountbyoutcome
    {

        public Total Total { get; set; }
        public Executed Executed { get; set; }
        public Passed Passed { get; set; }
        public Failed Failed { get; set; }
    }

    public class Failed
    {
        public string outcome { get; set; }
        public int count { get; set; }
        public string duration { get; set; }
    }

    public class Total
    {
        public string outcome { get; set; }
        public int count { get; set; }
        public string duration { get; set; }
    }

    public class Executed
    {
        public string outcome { get; set; }
        public int count { get; set; }
        public string duration { get; set; }
    }

}

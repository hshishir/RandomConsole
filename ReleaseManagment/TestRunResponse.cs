using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseManagment.TestRun
{

    public class TestRunResponse
    {
        public Value[] value { get; set; }
        public int count { get; set; }
    }

    public class Value
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public bool isAutomated { get; set; }
        public string iteration { get; set; }
        public Owner owner { get; set; }
        public DateTime startedDate { get; set; }
        public DateTime completedDate { get; set; }
        public string state { get; set; }
        public Plan plan { get; set; }
        public int revision { get; set; }
    }

    public class Owner
    {
        public string id { get; set; }
        public string displayName { get; set; }
    }

    public class Plan
    {
        public string id { get; set; }
    }

}

using System;

namespace ReleaseManagment
{
    public class TestCaseResultsResponse
    {
        public Value[] Value { get; set; }
        public int Count { get; set; }
    }

    public class Value
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public DateTime StartedDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public string Outcome { get; set; }
        public int Revision { get; set; }
        public Runby RunBy { get; set; }
        public string State { get; set; }
        public Testcase TestCase { get; set; }
        public Testrun TestRun { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Lastupdatedby LastUpdatedBy { get; set; }
        public int Priority { get; set; }
        public Build Build { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Url { get; set; }
        public string FailureType { get; set; }
        public string AutomatedTestType { get; set; }
        public string AutomatedTestId { get; set; }
        public Area Area { get; set; }
        public string TestCaseTitle { get; set; }
        public object[] CustomFields { get; set; }
        public string AutomatedTestName { get; set; }
    }

    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Runby
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string UniqueName { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
    }

    public class Testcase
    {
        public string Name { get; set; }
    }

    public class Testrun
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Lastupdatedby
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string UniqueName { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
    }

    public class Build
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Area
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}

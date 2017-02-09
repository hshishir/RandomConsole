using System;
using System.Collections.Generic;

namespace ReleaseManagment
{
    public class TestRunResult
    {
        public int ReleaseId {get; set;}
        public int TotalCount {get; set;}
        public int PassedCount {get; set;}
        public int FailedCount {get; set;}
        public string BuildNumber {get; set;}
        public string BranchName {get; set;}
        public string BuildUrl {get; set;}
        public string BuildId { get; set; }
        public IEnumerable<TestCategory> Category {get; set;}
    }

    public class TestCategory
    {
        public string Name{get; set;}
        public int RunId {get; set;}

        public int PassCount {get; set;}
        public int FailCount {get; set;}
        public OverallState RunState {get; set;}

        public IEnumerable<TestCaseResult> TestCaseResults {get; set;}
    }

    public class TestCaseResult
    {
        public string TestName { get; set; }
        public string TestType { get; set; }
        public Outcome Outcome { get; set; }
        public OverallState State { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompletedDate { get; set; }
    }

    public enum OverallState
    {
        Completed,
        Running
    }

    public enum Outcome
    {
        Passed,
        Failed
    }
}
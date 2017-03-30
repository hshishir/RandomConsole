using System;
using System.Collections.Generic;

namespace ReleaseManagment
{
    public class TestRunResult
    {
        public int ReleaseId { get; set; }
        public int TotalCount { get; set; }
        public int PassedCount { get; set; }
        public int FailedCount { get; set; }
        public string BuildNumber { get; set; }
        public string BranchName { get; set; }
        public string BuildId { get; set; }
        public string BuildUrl { get; set; }
        public RunType RunType { get; set; }
        public OverallState RunState { get; set; }
        public IEnumerable<TestCategory> Category { get; set; }
    }

    public class TestCategory
    {
        public string Name { get; set; }
        public int RunId { get; set; }

        public int PassCount { get; set; }
        public int FailCount { get; set; }
        public OverallState RunState { get; set; }
        public IEnumerable<TestCaseResult> TestCaseResults { get; set; }
    }

    public class TestCaseResult
    {
        public string TestName { get; set; }
        public string TestType { get; set; }
        public TestCaseOutcome Outcome { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public IEnumerable<TestErrors> Errors { get; set; }
    }

    public class TestErrors
    {
        public string Description { get; set; }
    }

    public enum OverallState
    {
        Failed,
        Passed,
        Running
    }

    public enum TestCaseOutcome
    {
        Error,
        Failed,
        Timeout,
        Aborted,
        Inconclusive,
        NotExecuted,
        Passed
    }

    public enum RunType
    {
        Official,
        Private,
        Validation
    }
}
using System;
using System.Collections.Generic;

namespace ReleaseManagment
{
    /// <summary>
    /// Class containing cumulative test run result information.
    /// Multiple categories of tests might run as part of a single test run.
    /// </summary>
    public class TestRunResult
    {
        /// <summary>
        /// Release Id that kicked off the run
        /// </summary>
        public int ReleaseId { get; set; }

        /// <summary>
        /// Total number of tests
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Number of tests passed
        /// </summary>
        public int PassCount { get; set; }

        /// <summary>
        /// Number of tests failed
        /// </summary>
        public int FailCount { get; set; }

        /// <summary>
        /// Build that triggered the release
        /// </summary>
        public string BuildNumber { get; set; }

        /// <summary>
        /// Branch for which tests were run
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Build for which tests were run
        /// </summary>
        public string BuildId { get; set; }

        /// <summary>
        /// Build url
        /// </summary>
        public string BuildUrl { get; set; }

        /// <summary>
        /// Run type (Official, Private, etc.)
        /// </summary>
        public string RunType { get; set; }

        /// <summary>
        /// Overall run state
        /// </summary>
        public OverallState RunState { get; set; }

        /// <summary>
        /// Results for test categories (DDRITs, OptProf, Setup etc.)
        /// </summary>
        public IEnumerable<TestCategoryResult> TestCategoryResults { get; set; }
    }

    /// <summary>
    /// Class containing result for a category of test.
    /// </summary>
    public class TestCategoryResult
    {
        /// <summary>
        /// Category name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Run Id for the test category
        /// </summary>
        public int RunId { get; set; }

        /// <summary>
        /// Number of tests passed
        /// </summary>
        public int PassCount { get; set; }

        /// <summary>
        /// Number of tests failed
        /// </summary>
        public int FailCount { get; set; }

        /// <summary>
        /// Overall run state
        /// </summary>
        public OverallState RunState { get; set; }

        /// <summary>
        /// List of tests that ran as part of a given category
        /// </summary>
        public IEnumerable<TestCaseResult> TestCaseResults { get; set; }
    }

    /// <summary>
    /// Class containing result for a test case
    /// </summary>
    public class TestCaseResult
    {
        /// <summary>
        /// Name of the test
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Test type (UnitTest etc.)
        /// </summary>
        public string TestType { get; set; }

        /// <summary>
        /// Test case run outcome
        /// </summary>
        public TestCaseOutcome Outcome { get; set; }

        /// <summary>
        /// Start time of the test
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Finish time of the test
        /// </summary>
        public DateTime CompletedDate { get; set; }

        /// <summary>
        /// Errors encountered during test
        /// </summary>
        public IEnumerable<TestCaseError> TestCaseErrors { get; set; }
    }

    /// <summary>
    /// Class containing test case error information
    /// </summary>
    public class TestCaseError
    {
        /// <summary>
        /// Description of error
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// Overall state of test run (for cumulative test or a category of test)
    /// </summary>
    public enum OverallState
    {
        Failed,
        Passed,
        Running
    }

    /// <summary>
    /// Outcome of a single test run
    /// </summary>
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
}
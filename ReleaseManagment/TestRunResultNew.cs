using System;
using System.Collections.Generic;
using System.Linq;

namespace ReleaseManagment.TestRunResultNew
{
    public class TestRunResult<T,P> : RunStateCollection<T> 
    where T : RunStateCollection<P>
    where P : RunState
    {
        /// <summary>
        /// Branch for which tests were run
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Build for which tests were run
        /// </summary>
        public string BuildId { get; set; }

        /// <summary>
        /// Build that triggered the release
        /// </summary>
        public string BuildNumber { get; set; }

        /// <summary>
        /// Build url
        /// </summary>
        public string BuildUrl { get; set; }

        /// <summary>
        /// Release Id that kicked off the run
        /// </summary>
        public int ReleaseId { get; set; }

        /// <summary>
        /// Run type (Official, Private, etc.)
        /// </summary>
        public string RunType { get; set; }
    }

    public class TestCategoryResult<T> : RunStateCollection<T> where T : TestCaseResult
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
        /// Run url for the test category
        /// </summary>
        public string RunUrl { get; set; }
    }

    public class TestCaseResult : RunState
    {
        /// <summary>
        /// Build number
        /// </summary>
        public Build Build { get; set; }
        /// <summary>
        /// Finish time of the test
        /// </summary>
        public DateTime CompletedDate { get; set; }

        /// <summary>
        /// Test case run outcome
        /// </summary>
        public TestCaseOutcome Outcome { get; set; }

        /// <summary>
        /// Start time of the test
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Errors encountered during test
        /// </summary>
        public IEnumerable<TestCaseError> TestCaseErrors { get; set; }

        /// <summary>
        /// Name of the test
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Test type (UnitTest etc.)
        /// </summary>
        public string TestType { get; set; }

        public override OverallState OverallState
        {
            get
            {
                if (Outcome.Equals(TestCaseOutcome.Passed))
                {
                    return OverallState.Passed;
                }
                else
                {
                    return OverallState.Failed;
                }
            }
        }
    }

    /// <summary>
    /// Class containing test case error information
    /// </summary>
    public class TestCaseError
    {
        public string Description { get; set; }
    }

    public abstract class RunState
    {
        public abstract OverallState OverallState { get; }
    }
    
    public class RunStateCollection<T> : RunState where T : RunState
    {
        public IEnumerable<T> RunStates { get; set; }

        public int PassCount => RunStates.Count( r => r.OverallState.Equals(OverallState.Passed));

        public int FailCount => RunStates.Count( r => r.OverallState.Equals(OverallState.Failed));

        public int TotalCount => RunStates.Count( r => r.OverallState.Equals(OverallState.Passed)) +
                                 RunStates.Count( r => r.OverallState.Equals(OverallState.Failed)) +
                                 RunStates.Count( r => r.OverallState.Equals(OverallState.Running));

        public override OverallState OverallState 
        { 
            get
            {
                if (RunStates.Any( r => r.OverallState.Equals(OverallState.Failed)))
                {
                    return OverallState.Failed;
                }
                else if (RunStates.Any( r => r.OverallState.Equals(OverallState.Running)))
                {
                    return OverallState.Running;
                }
                else if (RunStates.Any( r => r.OverallState.Equals(OverallState.NotStarted)))
                {
                    return OverallState.NotStarted;
                }

                return OverallState.Passed;
            }
        }
    }

    public enum OverallState
    {
        Failed,
        NotStarted,
        Passed,
        Running
    }

    /// <summary>
    /// Outcome of a single test run
    /// </summary>
    public enum TestCaseOutcome
    {
        Aborted,
        Error,
        Failed,
        Inconclusive,
        NotExecuted,
        Passed,
        Timeout
    }
}
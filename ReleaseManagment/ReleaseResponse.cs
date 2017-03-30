using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseManagment
{

    public class ReleaseResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime modifiedOn { get; set; }
        public Modifiedby modifiedBy { get; set; }
        public Createdby createdBy { get; set; }
        public Environment[] environments { get; set; }
        public Variables variables { get; set; }
        //public Artifact[] artifacts { get; set; }
        public Releasedefinition releaseDefinition { get; set; }
        public string description { get; set; }
        public string reason { get; set; }
        public string releaseNameFormat { get; set; }
        public bool keepForever { get; set; }
        public string logsContainerUrl { get; set; }
    }

    public class Modifiedby
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

    public class Createdby
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

    public class Variables
    {
        public Webappname webAppName { get; set; }
    }

    public class Webappname
    {
        public string value { get; set; }
    }

    public class Releasedefinition
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Environment
    {
        public int id { get; set; }
        public int releaseId { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public Variables1 variables { get; set; }
        public Predeployapproval[] preDeployApprovals { get; set; }
        public object[] postDeployApprovals { get; set; }
        public Preapprovalssnapshot preApprovalsSnapshot { get; set; }
        public Postapprovalssnapshot postApprovalsSnapshot { get; set; }
        public Deploystep[] deploySteps { get; set; }
        public int rank { get; set; }
        public int definitionEnvironmentId { get; set; }
        public int queueId { get; set; }
        public Runoptions runOptions { get; set; }
        public Environmentoptions environmentOptions { get; set; }
        public string[] demands { get; set; }
        public Condition[] conditions { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime modifiedOn { get; set; }
        public Workflowtask[] workflowTasks { get; set; }
        public Owner owner { get; set; }
    }

    public class Variables1
    {
        public Webappname1 webAppName { get; set; }
    }

    public class Webappname1
    {
        public string value { get; set; }
    }

    public class Preapprovalssnapshot
    {
        public Approval[] approvals { get; set; }
    }

    public class Approval
    {
        public int rank { get; set; }
        public bool isAutomated { get; set; }
        public bool isNotificationOn { get; set; }
        public int id { get; set; }
    }

    public class Postapprovalssnapshot
    {
        public Approval1[] approvals { get; set; }
    }

    public class Approval1
    {
        public int rank { get; set; }
        public bool isAutomated { get; set; }
        public bool isNotificationOn { get; set; }
        public int id { get; set; }
    }

    public class Runoptions
    {
        public string EnvironmentOwnerEmailNotificationType { get; set; }
        public string skipArtifactsDownload { get; set; }
        public string TimeoutInMinutes { get; set; }
    }

    public class Environmentoptions
    {
        public string emailNotificationType { get; set; }
        public bool skipArtifactsDownload { get; set; }
        public int timeoutInMinutes { get; set; }
    }

    public class Owner
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

    public class Predeployapproval
    {
        public int id { get; set; }
        public int revision { get; set; }
        public string approvalType { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime modifiedOn { get; set; }
        public string status { get; set; }
        public string comments { get; set; }
        public bool isAutomated { get; set; }
        public bool isNotificationOn { get; set; }
        public int trialNumber { get; set; }
        public int attempt { get; set; }
        public int rank { get; set; }
        //public Release release { get; set; }
        public Releasedefinition1 releaseDefinition { get; set; }
        public Releaseenvironment releaseEnvironment { get; set; }
    }

    //public class Release
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public string url { get; set; }
    //}

    public class Releasedefinition1
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Releaseenvironment
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Deploystep
    {
        public int id { get; set; }
        public int attempt { get; set; }
        public object[] tasks { get; set; }
        public string runPlanId { get; set; }
    }

    public class Condition
    {
        public string name { get; set; }
        public string conditionType { get; set; }
        public string value { get; set; }
    }

    public class Workflowtask
    {
        public string taskId { get; set; }
        public string version { get; set; }
        public string name { get; set; }
        public bool enabled { get; set; }
        public bool alwaysRun { get; set; }
        public bool continueOnError { get; set; }
        public string definitionType { get; set; }
        public Inputs inputs { get; set; }
    }

    public class Inputs
    {
        public string ConnectedServiceName { get; set; }
        public string WebSiteName { get; set; }
        public string WebSiteLocation { get; set; }
        public string Slot { get; set; }
        public string Package { get; set; }
        public string doNotDelete { get; set; }
        public string AdditionalArguments { get; set; }
    }

    public class Artifact
    {
        public int id { get; set; }
        public string type { get; set; }
        public string alias { get; set; }
        public Definitionreference definitionReference { get; set; }
        public bool isPrimary { get; set; }
    }

    public class Definitionreference
    {
        public Definition definition { get; set; }
        //public Project project { get; set; }
        public Version version { get; set; }
        public Branch branch { get; set; }
    }

    public class Definition
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    //public class Project
    //{
    //    public string id { get; set; }
    //    public string name { get; set; }
    //}

    public class Version
    {
        public string id { get; set; }
        public object name { get; set; }
    }

    public class Branch
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}


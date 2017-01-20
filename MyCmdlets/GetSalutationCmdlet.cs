using System.Management.Automation;

namespace MyCmdlets
{
    /// <summary>
    /// Refer http://www.powershellmagazine.com/2014/03/18/writing-a-powershell-module-in-c-part-1-the-basics/
    /// Usage:
    ///     Import-Module C:\GitReposPersonal\RandomConsole\MyCmdlets\bin\Debug\MyCmdlets.dll -Verbose
    ///     Get-Salutation -Name Hemang,Neha
    ///     Get-Salutation -Person Hemang,Neha
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Salutation")]
    public class GetSalutationCmdlet : PSCmdlet
    {
        private string[] nameCollection;

        [Parameter(
            Mandatory = true,
            HelpMessage = "Name to get salutation for"
        )]
        [Alias("Person", "FirstName")]
        public string[] Name
        {
            get { return nameCollection; }
            set { nameCollection = value; }
        }

        protected override void ProcessRecord()
        {
            foreach (var name in nameCollection)
            {
                WriteVerbose($"Creating salutation for {name}");
                var salutation = $"Hello {name}";
                WriteObject(salutation);
            }
        }
    }
}

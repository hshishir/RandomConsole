using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReleaseManagment
{
    public class TestSuites
    {
        //public static List<TestContainer> GetData(string testSuitesFilePath)
        //{
        //    dynamic containers = null;
        //    if (!File.Exists(testSuitesFilePath))
        //    {
        //        throw new FileNotFoundException($"File {testSuitesFilePath} not found");
        //    }

        //    try
        //    {
        //        using (var reader = new StreamReader(testSuitesFilePath))
        //        {
        //            var json = reader.ReadToEnd();
        //            var jsonParsed = JsonConvert.DeserializeObject<dynamic>(json);
        //            containers = JsonConvert.DeserializeObject<List<TestContainer>>(jsonParsed.testcontainer.ToString());
        //        }

        //        return containers;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
    }
}

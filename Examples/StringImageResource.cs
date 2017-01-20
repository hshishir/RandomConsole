using System;

namespace RandomConsole.Examples
{
    // http://www.codeproject.com/Articles/252879/NET-String-Resources
    public class StringImageResource
    {
        public void TestMe()
        {
            Console.WriteLine(Resources.FailedToFindDrop);
            Console.WriteLine(Resources.ServerDown, "http://foo");

            var bitmap = Resources.DayOfCaring;
            Console.WriteLine("Image Height={0}, Width={1}", bitmap.Height, bitmap.Width);
        }
    }
}

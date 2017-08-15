using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomConsole.Examples
{
    public class TaskWithRetry
    {
        public static Task<T> CreateTaskWithRetry<T>(Func<T> action, int retryCount, TimeSpan retryDelay)
        {
            Func<T> retryAction = () =>
            {
                int attempts = 0;
                do
                {
                    try
                    {
                        attempts++;
                        return action();
                    }
                    catch (Exception e)
                    {

                    }
                }while (attempts < retryCount);
                return default(T);
            };

           return new Task<T>(retryAction);
        }
    }
}

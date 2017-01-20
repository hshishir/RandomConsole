using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomConsole.Examples
{
    class DelegateExample
    {
        private ActionExample _action;
        private FuncExample _func;

        public DelegateExample()
        {
            _action = new ActionExample();
            _func = new FuncExample();
        }

        public void TestMe()
        {
            _action.TestMe();
            Console.WriteLine(); Console.WriteLine();
            _func.TestMe();
        }
    }

    class ActionExample
    {
        public void TestMe()
        {
            NoParameters(ActionNoParam);
            WithParameter(ActionWithParam);

            // Using lambda
            NoParameters(() =>
            {
                Console.WriteLine("Action delegate no parameter (lambda)");
            });

            WithParameter((fn, ln) =>
            {
                Console.WriteLine("Action delegate with parameter (lambda) {0} {1}", fn, ln);
            });
        }

        public void NoParameters(Action action)
        {
            action();
        }

        public void WithParameter(Action<string,string> action)
        {
            action("Hemang", "Shihsir");
        }

        public void ActionNoParam()
        {
            Console.WriteLine("Action delegate no parameter");
        }

        public void ActionWithParam(string fname, string lname)
        {
            Console.WriteLine("Action delegate two parameters {0} {1}", fname, lname);
        }
    }

    class FuncExample
    {
        public void TestMe()
        {
            NoParameter(FuncNoParam);
            WithParameter(FuncWithParam);

            // Using lambda
            NoParameter(() =>
            {
                return "Func delegate no parameter (lambda)";
            });

            WithParameter((i) =>
            {
                return "Func delegate with parameter (lambda) " + i.ToString();
            });
        }

        public void NoParameter(Func<string> function)
        {
            var str = function();
            Console.WriteLine(str);
        }

        public void WithParameter(Func<int, string> function)
        {
            Console.WriteLine(function(10));
        }

        public string FuncNoParam()
        {
            return "Func delegate no parameter";
        }

        public string FuncWithParam(int age)
        {
            return string.Format("Func delegate one parameter: Age {0}", age.ToString());
        }
    }
}

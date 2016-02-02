using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RandomConsole.Examples
{
    // http://www.codeproject.com/Tips/133941/Anonymous-types
    class AnonymousType
    {
        public AnonymousType()
        {
        }

        public void TestMe()
        {
            Console.WriteLine("***Type 1***");
            CreateType1();
            Console.WriteLine("***Type 2***");
            CreateType2();
            Console.WriteLine("***Type 3***");
            CreateType3();
        }
        private void CreateType1()
        {
            var first = new {id = 1, Name = "First"};
            var second = new {id = 2, Name = "Second"};

            Console.WriteLine("Person: {0} {1}", first.id, first.Name);
            Console.WriteLine("Second: {0} {1}", second.id, second.Name);
        }

        private void CreateType2()
        {
            var persons = new[]
            {
                new {id = 1, Name = "First"},
                new {id = 2, Name = "Second"}
            };

            foreach (var person in persons)
            {
                Console.WriteLine("Person: {0} {1}", person.id, person.Name);
            }
        }

        private void CreateType3()
        {
            var employees = new []
            {
                new Employees(1, "First1", "Last1"),
                new Employees(2, "First2", "Last2"),
                new Employees(3, "First3", "Last3"),
                new Employees(4, "First4", "Last4"),
            };

            // Cannot set this property since setter is private
            //employees[0].FirstName = "Hello";

            var subset = from emp in employees
                where emp.Id > 2
                select new {emp.Id, emp.LastName};

            foreach (var sub in subset)
            {
                Console.WriteLine("Employee: {0} {1}", sub.Id, sub.LastName);
            }
        }
    }

    public class Employees
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Employees(int id, string fname, string lname)
        {
            Id = id;
            FirstName = fname;
            LastName = lname;
        }
    }
}

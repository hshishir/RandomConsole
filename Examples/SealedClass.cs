using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomConsole.Examples
{
    // Sealed modifier prevents other classes from inheriting.
    sealed class SealedClass
    {
        public int x;
        public int y;
    }

    // class MyDerivedC : SealedClass { } // Error


    // Use the sealed modifier on a method or property that overrides a virtual method or property in a base class. 
    //This enables you to allow classes to derive from your class and prevent them from overriding specific virtual methods or properties. 
    class X
    {
        protected virtual void F() { Console.WriteLine("X.F"); }
        protected virtual void F2() { Console.WriteLine("X.F2"); }
    }
    class Y : X
    {
        sealed protected override void F() { Console.WriteLine("Y.F"); }
        protected override void F2() { Console.WriteLine("Y.F2"); }
    }
    class Z : Y
    {
        // Attempting to override F causes compiler error CS0239.
        // protected override void F() { Console.WriteLine("C.F"); }

        // Overriding F2 is allowed.
        protected override void F2() { Console.WriteLine("Z.F2"); }
    }
}

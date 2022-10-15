using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleClassesLibrary
{
    internal class Class2
    {
        public void FirstMethod(Class1 class1)
        {
            Console.WriteLine("First method");
        }

        public void SecondMethod()
        {
            Console.WriteLine("Second method");
        }

        public void ThirdMethod(int a)
        {
            Console.WriteLine("Third method (int)");
        }

        public void ThirdMethod(double a)
        {
            Console.WriteLine("Third method (double)");
        }
    }
}

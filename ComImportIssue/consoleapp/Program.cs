using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using nslib;

namespace consoleapp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Class1 class1= new Class1();
            Test1();
        }

        public static void Test1()
        {
            foreach( var t in  GetAllTypesOf<ClassFactory>()){
                System.Console.WriteLine(t.ToString());
            }
        }

        private static IEnumerable<Type> GetAllTypesOf<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.DefinedTypes)
                .Where(type => typeof(T).GetTypeInfo().IsAssignableFrom(type.AsType()))
                .Select(type => type.AsType());
        }
    }
}

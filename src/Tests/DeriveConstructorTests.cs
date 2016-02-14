using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsDerive;

namespace Tests
{
    public class DeriveConstructorTests
    {
        [DeriveConstructor]
        class NoParams {
        }

        [DeriveConstructor]
        class OnlyReadonlyProperties {
            public string name { get; }
        }

        [DeriveConstructor]
        class OnlyReadonlyFields {
            public readonly string name;
        }

        [DeriveConstructor]
        class ReadOnlyAndNonReadOnlyFields {
            public readonly string name;
            public string description;
        }

        [DeriveConstructor]
        class ReadOnlyAndNonReadOnlyProperties {
            public string name { get; }
            public string description { get; private set; }
        }

        // TODO detect early instead of generating duplicate constructor
        //[DeriveConstructor]
        //class DuplicateConstructor {
        //    public string name { get; }

        //    public DuplicateConstructor(string name) { }
        //}


        public static void Tests() {
            var a1 = new NoParams();
            var a2 = new OnlyReadonlyProperties(name: "John property");
            Console.WriteLine(a2.name);
            var a3 = new OnlyReadonlyFields("John field");
            Console.WriteLine(a3.name);
            var a4 = new ReadOnlyAndNonReadOnlyFields(name: "John field");
        }
    }
}

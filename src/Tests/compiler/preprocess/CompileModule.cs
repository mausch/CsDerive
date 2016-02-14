using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Dnx.Compilation.CSharp;
using CsDerive;

namespace Tests.compiler.preprocess
{

    public class CompileModule: AggregateCompileModule 
    {
        public CompileModule() : base(new[] {
            new DeriveConstructorProcessor()
        }) { }
    }
}

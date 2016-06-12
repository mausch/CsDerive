using StackExchange.Precompilation;
using System;
using System.Collections.Generic;

namespace CsDerive
{
    public class AggregateCompileModule: ICompileModule {
        private readonly IReadOnlyCollection<ICompileModule> modules;

        public AggregateCompileModule(IReadOnlyCollection<ICompileModule> modules) {
            this.modules = modules;
        }

        public void BeforeCompile(BeforeCompileContext context) {
            foreach (var module in modules)
                module.BeforeCompile(context);
        }

        public void AfterCompile(AfterCompileContext context) {
            foreach (var module in modules)
                module.AfterCompile(context);
        }
    }
}

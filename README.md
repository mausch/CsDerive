# CsDerive
Compile-time derivation of code for C#

The goal of this project is to generate code at compile-time using [DNX's `ICompileModule`](https://github.com/aspnet/dnx/issues/39) to reduce the amount of common boilerplate when writing everyday C#.<br/>
It can already generate constructors for immutable types (see [examples](https://github.com/mausch/CsDerive/blob/master/src/Tests/DeriveConstructorTests.cs)).<br/>
See the [issues](https://github.com/mausch/CsDerive/issues) for other features planned to reduce boilerplate.

**Current status**: on hold because [Microsoft seems to have killed `ICompileModule`](https://github.com/dotnet/cli/issues/1812). This is to be replaced with a similar Roslyn feature called "source generators" which is currently under development targeting C# 7. Latest update I've seen about this is https://github.com/dotnet/roslyn/issues/10882#issuecomment-214864671

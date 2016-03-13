# CsDerive
Compile-time derivation of code for C#

The goal of this project is to generate code at compile-time using [DNX's `ICompileModule`](https://github.com/aspnet/dnx/issues/39) to reduce the amount of common boilerplate when doing functional programming in C#.<br/>
It can already generate constructors for immutable types (see [examples](https://github.com/mausch/CsDerive/blob/master/src/Tests/DeriveConstructorTests.cs)).<br/>
See the [issues](https://github.com/mausch/CsDerive/issues) for other features planned to reduce boilerplate.

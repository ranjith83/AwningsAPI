// Exposes the compiler-generated Program class (internal by default in top-level-statement
// projects) to the test assembly so WebApplicationFactory<Program> can reference it.
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("AwningsAPI.Tests")]

dotnet restore EasyConstructor.sln
msbuild build src/EasyConstructor.csproj
dotnet test test/test.csproj
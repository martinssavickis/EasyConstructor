export FrameworkPathOverride=$(dirname $(which mono))/../lib/mono/4.5/
dotnet restore EasyConstructor.sln
dotnet build src/EasyConstructor.csproj
dotnet test test/test.csproj
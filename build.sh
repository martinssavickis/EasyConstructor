dotnet restore EasyConstructor.sln
#I haven't figured out how to get msbuild on my machine, so override full framework logation with mono for now
#use this to run locally
#export FrameworkPathOverride=/usr/lib/mono/4.5/
msbuild build src/EasyConstructor.csproj
dotnet test test/test.csproj
cd Example
dotnet restore
dotnet publish -c Release -r win-x64 -p:PublishNativeAot=True
bin/Release/net6.0/win-x64/publish/Example

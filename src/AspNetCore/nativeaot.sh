#!/bin/bash

cd Example
dotnet restore
dotnet publish -c Release -r linux-x64 -p:PublishNativeAot=True
# Comment line below if you want preserve debug symbols in the executable
strip bin/Release/net6.0/linux-x64/publish/Example
bin/Release/net6.0/linux-x64/publish/Example

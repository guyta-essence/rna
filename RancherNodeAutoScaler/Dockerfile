FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /WorkDir
COPY . ./
ENTRYPOINT ["dotnet", "RancherNodeAutoScaler.dll", "--console"]

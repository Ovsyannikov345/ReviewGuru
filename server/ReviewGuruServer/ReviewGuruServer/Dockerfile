FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
EXPOSE 8080

COPY *.sln .
COPY ReviewGuruServer/*.csproj ./ReviewGuruServer/
COPY ReviewGuru.BAL/*.csproj ./ReviewGuru.BAL/
COPY ReviewGuru.DAL/*.csproj ./ReviewGuru.DAL/
COPY Tests/*.csproj ./Tests/ 

RUN dotnet restore 

COPY ReviewGuruServer/. ./ReviewGuruServer/
COPY ReviewGuru.BAL/. ./ReviewGuru.BAL/
COPY ReviewGuru.DAL/. ./ReviewGuru.DAL/
COPY Tests/. ./Tests/ 

WORKDIR /app/ReviewGuruServer
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/ReviewGuruServer/out ./
ENTRYPOINT ["dotnet", "ReviewGuru.API.dll"]
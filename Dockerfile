FROM            mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR         /app
EXPOSE          80

ENV		ConnectionStrings__TopazContext=%CONNECTIONSTRING%

FROM            mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR         /src
ADD		.	.
RUN		dotnet restore TopazWebApp/Topaz.csproj
RUN             dotnet publish TopazWebApp/Topaz.csproj -c Release -o /app --no-restore

FROM            base
WORKDIR         /app
COPY --from=build /app ./
ENTRYPOINT      ["dotnet", "Topaz.dll"]

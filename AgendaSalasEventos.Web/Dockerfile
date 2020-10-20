#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AgendaSalasEventos.Web/AgendaSalasEventos.Web.csproj", "AgendaSalasEventos.Web/"]
RUN dotnet restore "AgendaSalasEventos.Web/AgendaSalasEventos.Web.csproj"
COPY . .
WORKDIR "/src/AgendaSalasEventos.Web"
RUN dotnet build "AgendaSalasEventos.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AgendaSalasEventos.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AgendaSalasEventos.Web.dll"]


# builds our image using dotnet's sdk
#FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
#WORKDIR /source
#COPY . ./webapp/
#WORKDIR /source/webapp
#RUN dotnet restore
#RUN dotnet publish -c release -o /app --no-restore
#
## runs it using aspnet runtime
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
#WORKDIR /app
#COPY --from=build /app ./
#ENTRYPOINT ["dotnet", "webapp.dll"]
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Faces.WebMVC/Faces.WebMVC.csproj", "Faces.WebMVC/"]
COPY ["Faces.SharedLib/Messaging.InterfacesConstant/Messaging.InterfacesConstant.csproj", "Faces.SharedLib/Messaging.InterfacesConstant/"]
RUN dotnet restore "Faces.WebMVC/Faces.WebMVC.csproj"
COPY . .
WORKDIR "/src/Faces.WebMVC"
RUN dotnet build "Faces.WebMVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Faces.WebMVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Faces.WebMVC.dll"]

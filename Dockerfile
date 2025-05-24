# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source/

# copy everything else and build app
COPY ./ ./

RUN dotnet restore

RUN dotnet publish -c release -o ./release --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0

ARG TZ="Asia/Ho_Chi_Minh"
ENV TZ ${TZ}
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

WORKDIR /app
COPY --from=build /source/release ./
ENTRYPOINT ["dotnet", "backend.dll"]

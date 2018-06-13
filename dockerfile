FROM microsoft/dotnet:2.1.300-sdk-stretch AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY src/*.csproj ./src/
RUN dotnet restore

# copy everything else and build app
COPY . .
WORKDIR /app/src
RUN dotnet build

FROM build AS publish
WORKDIR /app/src
RUN dotnet publish -c Release -o out

# Copy konon folder to release folder
WORKDIR /app/src/out
RUN rm -rf ./konon
COPY src/konon/*.mp3 ./konon/

FROM microsoft/dotnet:2.1.0-runtime-stretch-slim AS runtime
RUN apt-get update -y && apt-get install -y build-essential
RUN apt-get install -y libopus0 libopus-dev libsodium-dev
RUN apt-get install -y ffmpeg
WORKDIR /app
COPY --from=publish /app/src/out ./
ENTRYPOINT ["dotnet", "MayorBot.dll"]

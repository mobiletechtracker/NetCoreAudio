FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

# copy the solution and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.1-runtime
WORKDIR /app

# copy build output
COPY --from=build-env /app/DemoHarness/out ./

# copy audio files
COPY --from=build-env /app/*.mp3 ./

# set main entry point for the app
ENTRYPOINT ["dotnet", "DemoHarness.dll"]
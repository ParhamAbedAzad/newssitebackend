FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY . .
# Build runtime image
ENTRYPOINT ["dotnet", "NewsSiteBackEnd.dll"]

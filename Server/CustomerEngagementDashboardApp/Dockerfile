FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# העתק את קובץ ה-csproj של הפרויקט הראשי בלבד
COPY CustomerEngagementDashboardApp.csproj ./
RUN dotnet restore

# עכשיו העתק את שאר הקבצים
COPY . . 
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=build /out .

ENV ASPNETCORE_URLS=http://+:5000
ENV DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 5000

ENTRYPOINT ["dotnet", "CustomerEngagementDashboardApp.dll"]

# Use the official .NET image to build the .NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-dotnet
WORKDIR /app
COPY LightFeatherSupervisorManagementNotificationApi/ ./
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish -r linux-musl-x64 --self-contained true

# Use the official Node image to build the Angular application
FROM node:20 AS build-angular
WORKDIR /web
COPY LightFeatherSupervisorManagementNotificationFrontEnd/ ./
RUN npm install
RUN npm run build

# Final stage/image
FROM --platform=linux/amd64 nginx:alpine
RUN apk upgrade --no-cache
RUN apk add --no-cache bash openssl libgcc libstdc++ icu-libs

COPY --from=build-angular /web/dist/light-feather-supervisor-management-notification-front-end/browser/ /usr/share/nginx/html
COPY --from=build-dotnet /app/publish /app
COPY nginx/nginx.conf /etc/nginx/nginx.conf

ENV ASPNETCORE_URLS=http://localhost:5032

EXPOSE 80

WORKDIR /app

# Start both Nginx and the .NET application
CMD sh -c '/app/LightFeatherSupervisorManagementNotificationApi & nginx -g "daemon off;"'

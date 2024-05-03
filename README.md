# Deployment and Running Instructions

This document provides the necessary steps to build and run the LightFeather Supervisor Management Notification application, which consists of a .NET backend and an Angular frontend, served via Nginx within a Docker container.

Prerequisites:

    Docker installed on your machine. If not already installed, follow the instructions for your respective operating system here.

# Building the Docker Image

To build the Docker image, navigate to the root directory of the project where the Dockerfile is located and run the following command:

```bash

docker build -t lightfeather-app .

```

# Running the Docker Container

Once the Docker image has been built, you can run the container using the following command:

```bash

docker run -p 80:80 lightfeather-app

```

This command maps port 80 of the Docker container to port 80 on your host machine, allowing you to access the application via [http://localhost](http://localhost) in your web browser.

> Note: The Dockerfile uses the `--platform=linux/amd64` flag internally to ensure compatibility with Apple Silicon (ARM64) processors by explicitly defining the platform as AMD64. This means that, if you are running this on an Apple Silicon Mac, it will show up in Docker Desktop as using AMD64 emulation. This is normal.
>
> The image can be tweaked to run on ARM64 natively, but this has been omitted for demo purposes.

# Accessing the Application

- Frontend (Angular)
  - Open a web browser and go to [http://localhost](http://localhost). This will serve the Angular application.

- Backend (.NET API)
  - The .NET application can be accessed directly via the `/api` endpoint (e.g. [http://localhost/api/supervisors](http://localhost/api/supervisors)).

# Additional Information

The Docker container utilizes Nginx to serve the Angular application's static files and to proxy requests to the .NET application through the /api prefix.

Ensure no other applications are running on port 80 on your host machine to avoid port conflicts, or choose a different port.

# Running without Docker

The .NET application is configured to run on port 5032 by default. The Angular frontend has a `proxy.config.json` file which allows it to seamlessly communicate with the backend (without resorting to CORS workarounds).

To run the applications without Docker, make sure you have .NET 8 and node.js 20, as well as the Angular CLI installed on your machine. Then:

1) Run the .NET app:

```bash

cd LightFeatherSupervisorManagementNotificationApi/
dotnet run

```

2) In a separate terminal, run the Angular app along with the built-in proxy:

```bash

cd LightFeatherSupervisorManagementNotificationFrontEnd/
npm install
ng serve

```

The site should now be running locally on [http://localhost:4200](http://localhost:4200).

# Troubleshooting

If you encounter any issues with the application not running as expected, ensure that Docker is correctly installed and running on your system. Additionally, check the Docker container logs for any error messages or warnings that may provide more insight into any issues.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS migrator

WORKDIR /app

RUN dotnet tool install --global dotnet-ef --version 6.0.10

ENV PATH="$PATH:/root/.dotnet/tools"

COPY . .

RUN dotnet restore "./Smart Electric Metering System BackEnd.csproj"
RUN dotnet build "./Smart Electric Metering System BackEnd.csproj"
CMD ["/bin/sh", "-c", "sleep 360 && dotnet ef database update --no-build --project 'Smart Electric Metering System BackEnd.csproj'"]

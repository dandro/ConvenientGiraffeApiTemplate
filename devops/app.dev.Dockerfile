FROM mcr.microsoft.com/dotnet/sdk:5.0

WORKDIR /app 

ENV DOCKER="true"

EXPOSE 5000

COPY ../../ .

RUN dotnet tool restore

CMD ["dotnet", "fake", "run", "dev.fsx"]

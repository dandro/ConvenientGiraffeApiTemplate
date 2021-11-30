FROM postgres:latest

ENV POSTGRES_PASSWORD="insecuredbpassword"
ENV POSTGRES_DB="localdb"

EXPOSE 5432

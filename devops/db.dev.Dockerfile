FROM postgres:latest

ENV POSTGRES_PASSWORD="insecuredbpassword"
ENV POSTGRES_DB="localuser"

EXPOSE 5432

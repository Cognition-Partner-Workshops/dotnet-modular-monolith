FROM mcr.microsoft.com/mssql/server:2022-latest

COPY ./CompanyName.MyMeetings.Database/Scripts/CreateDatabase_Linux.sql /scripts/

ENV ACCEPT_EULA=Y
ENV MSSQL_PID=Express
ENV MSSQL_TCP_PORT=1433

ADD entrypoint.sh /
ENTRYPOINT ["/bin/bash", "/entrypoint.sh"]

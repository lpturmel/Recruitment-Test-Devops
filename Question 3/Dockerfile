# NOTE:
#
# *The image did not contain sqlcmd at the path the script expected so I opted to create /opt/mssql-tools/bin/sqlcmd as a script
# that forwards all arguments to the real bin and trusts the server's certificates*
#
# The database is restored during the builder stage of the Docker build.
# The SQL Server is started temporarily and polled until it can run the restore_db.sh script.
#
#
# The final layer only copies the data of the database and the entrypoint.sh script without keeping the restore_db.sh script.

ARG BUILD_FROM=mcr.microsoft.com/mssql/server:2019-latest
# The reason I put a default password is to satisfy the build command requirement of the question 3 README.md while being able
# in the pipeline to pass the SA_PASSWORD from a variable.
ARG SA_PASSWORD=SuperStrong@Passw0rd

FROM ${BUILD_FROM} AS builder
ARG SA_PASSWORD

ENV ACCEPT_EULA=Y

COPY --chmod=755 entrypoint.sh /usr/local/bin/entrypoint.sh
COPY --chmod=755 restore_db.sh /var/opt/mssql/docker/restore_db.sh
COPY data.sql /var/opt/mssql/docker/data.sql

RUN mkdir -p /opt/mssql-tools/bin
RUN printf '%s\n' '#!/bin/sh' 'exec /opt/mssql-tools18/bin/sqlcmd -C "$@"' > /opt/mssql-tools/bin/sqlcmd
RUN chmod 755 /opt/mssql-tools/bin/sqlcmd

RUN /opt/mssql/bin/sqlservr & \
  pid="$!" && \
  ready=0 && \
  for i in $(seq 1 30); do \
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "${SA_PASSWORD}" -Q "SELECT 1" >/dev/null 2>&1 && ready=1 && break || sleep 5; \
  done && \
  test "$ready" = "1" && \
  /var/opt/mssql/docker/restore_db.sh && \
  kill "$pid" && \
  wait "$pid"



FROM ${BUILD_FROM}
ARG SA_PASSWORD

ENV ACCEPT_EULA=Y

RUN mkdir -p /opt/mssql-tools/bin
RUN printf '%s\n' '#!/bin/sh' 'exec /opt/mssql-tools18/bin/sqlcmd -C "$@"' > /opt/mssql-tools/bin/sqlcmd
RUN chmod 755 /opt/mssql-tools/bin/sqlcmd

COPY --from=builder /var/opt/mssql/data /var/opt/mssql/data
COPY --chmod=755 entrypoint.sh /usr/local/bin/entrypoint.sh

HEALTHCHECK CMD /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "${SA_PASSWORD}" -Q "SELECT 1" || exit 1

ENTRYPOINT ["/usr/local/bin/entrypoint.sh"]

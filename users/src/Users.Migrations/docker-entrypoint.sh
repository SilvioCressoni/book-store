#!/bin/sh
dotnet /app/Users.Migrations.dll -cs "$ConnectionString" && echo 'schema' > /var/output/migrator.txt
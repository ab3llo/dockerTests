#!/bin/bash
set -eu -o pipefail
dotnet restore /src/Docker.Spring.Rest.Api.Tests -nowarn:msb3202,nu1503
dotnet test  /src/Docker.Spring.Rest.Api.Tests/Docker.Spring.Rest.Api.Tests.csproj

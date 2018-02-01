#!/usr/bin/env bash

# stop on first error
set -e

# arguments

CONFIGURATION="Release"

# validations

if ! [ -x "$(command -v dotnet)" ]; then
    echo "dotnet cli is not installed."
    exit 1
fi

# tasks

echo
echo dotnet-restore
echo ----------------------

dotnet restore

echo
echo dotnet-build
echo ----------------------

dotnet build -c $CONFIGURATION --no-incremental /p:NonWindowsBuild=true

echo
echo dotnet-test
echo ----------------------

dotnet test test/OpenTracing.Tests/OpenTracing.Tests.csproj -c $CONFIGURATION --no-build /p:NonWindowsBuild=true

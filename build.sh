#!/usr/bin/env bash

# stop on first error
set -e

# arguments

CONFIGURATION="Release"

# validations

if ! [ -x "$(command -v dotnet)" ]; then
    echo ".NET Core SDK is not installed."
    exit 1
fi

# tasks

echo
echo dotnet-build
echo ----------------------

dotnet build -c $CONFIGURATION --no-incremental

echo
echo dotnet-test
echo ----------------------

for d in test/*/*.csproj; do
    dotnet test $d -c $CONFIGURATION --no-build
done

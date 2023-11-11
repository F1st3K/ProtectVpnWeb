#!/bin/bash

cd ../src/ProtectVpnWeb.Api

if [[ "$1" == "-i" ]]; then
    echo "run incremental build"
    ~/.dotnet/dotnet build --no-restore --incremental
else
    echo "run build"
    ~/.dotnet/dotnet restore
    ~/.dotnet/dotnet build --no-restore
fi

~/.dotnet/dotnet publish -o ../../deploy

#!/bin/bash

cd ../src/WireguardWeb.Api

if [[ "$1" == "-i" ]]; then
    echo "run incremental build"
    dotnet build --no-restore --incremental
else
    echo "run build"
    dotnet restore
    dotnet build --no-restore
fi

dotnet publish -o ../../deploy

#!/bin/bash

if [[ "$1" == "-i" ]]; then
    bash build.sh -i
else
    bash build.sh
fi

cd ..

if [ -z "$1" ] || [ "$1" == "-i" ]; then
    a="/home/f1st3k/repos/WireguardWeb/.idea/.idea.WireguardWeb/Docker/docker-compose.generated.override.yml"
else 
    a="$1"
fi
echo $a
docker-compose -f docker-compose.yml -f $a -p wireguardweb stop wireguardweb.api
docker-compose -f docker-compose.yml -f $a -p wireguardweb up -d

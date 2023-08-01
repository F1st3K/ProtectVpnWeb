#!/bin/bash

if [[ "$1" == "-i" ]]; then
    bash build.sh -i
else
    bash build.sh
fi


/usr/bin/docker-compose -f /home/f1st3k/repos/WireguardWeb/docker-compose.yml -f /home/f1st3k/repos/WireguardWeb/.idea/.idea.WireguardWeb/Docker/docker-compose.generated.override.yml -p wireguardweb stop wireguardweb.api
/usr/bin/docker-compose -f /home/f1st3k/repos/WireguardWeb/docker-compose.yml -f /home/f1st3k/repos/WireguardWeb/.idea/.idea.WireguardWeb/Docker/docker-compose.generated.override.yml -p wireguardweb up -d

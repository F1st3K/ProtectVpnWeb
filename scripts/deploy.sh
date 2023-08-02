#!/bin/bash

if [[ "$1" == "-i" ]]; then
    bash build.sh -i
else
    bash build.sh
fi

cd ..
defdir_override_uml=".idea/.idea.WireguardWeb/Docker/docker-compose.generated.override.yml"

if [ -e "$defdir_override_uml" ]; then
    dir_override_uml="$defdir_override_uml"
    echo "default uml is find!"
elif [ -n "$1" ] && [ "$1" != "-i" ]; then
    dir_override_uml="$1"
elif [ -n "$2" ]; then
    dir_override_uml="$2"
else
  docker-compose -f docker-compose.yml -p wireguardweb stop wireguardweb.api
  docker-compose -f docker-compose.yml -p wireguardweb up -d
  echo "override uml is not found!"
  exit
fi

echo $dir_override_uml
docker-compose -f docker-compose.yml -f $dir_override_uml -p wireguardweb stop wireguardweb.api
docker-compose -f docker-compose.yml -f $dir_override_uml -p wireguardweb up -d
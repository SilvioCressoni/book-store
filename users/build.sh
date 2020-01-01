#!/usr/bin/env bash

# Define directories.
SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
SCRIPT="./build/main.cake"
TOOLS_DIR=$SCRIPT_DIR/tools
CAKE_VERSION=0.35.0
CAKE_ARGUMENTS=()

# Parse arguments.
for i in "$@"; do
    case $1 in
        -s|--script) SCRIPT="$2"; shift ;;
        --cake-version) CAKE_VERSION="--version=$2"; shift ;;
        --) shift; CAKE_ARGUMENTS+=("$@"); break ;;
        *) CAKE_ARGUMENTS+=("$1") ;;
    esac
    shift
done

# Make sure the tools folder exist.
if [ ! -d "$TOOLS_DIR" ]; then
  mkdir "$TOOLS_DIR"
fi

#Install lib
CAKE_PATH="$TOOLS_DIR/dotnet-cake"
CAKE_INSTALLED_VERSION=$($CAKE_PATH --version 2>&1)

if [ "$CAKE_VERSION" != "$CAKE_INSTALLED_VERSION" ]; then
    if [ -f "$CAKE_PATH" ]; then
        dotnet tool uninstall Cake.Tool --tool-path "$TOOLS_DIR" 
    fi

    echo "Installing Cake $CAKE_VERSION..."
    dotnet tool install Cake.Tool --tool-path "$TOOLS_DIR" --version $CAKE_VERSION

    if [ $? -ne 0 ]; then
        echo "An error occured while installing Cake."
        exit 1
    fi
fi

# Start Cake
exec "$CAKE_PATH" "$SCRIPT" "${CAKE_ARGUMENTS[@]}"
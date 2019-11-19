#!/bin/sh

# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

# Stop script if command returns non-zero exit code.
# Prevents hidden errors caused by missing error code propagation.
set -e


usage() {
  echo "Common settings:"
  echo "  --configuration <value>  Build configuration: 'Debug' or 'Release' (short: -c)"
  echo "  --verbosity <value>      Msbuild verbosity: q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic] (short: -v)"
  echo "  --help                   Print help and exit"
  echo ""

  echo "Actions:"
  echo " --restore                Restore dependencies (short: -r)"
  echo " --build                  Build solution (short: -b)"
  echo " --clean                  Clean the solution"
  echo " --test                   Run all unit tests in the solution (short: -t)"
  echo " --integrationTest        Run all integration tests in the solution"
  echo " --publish                Publish artifacts (e.g. symbols)"
  echo " --migrations             Run migrations(short: -m)"
  echo " --connectionString       Connection string (short: -cs)"
  echo ""

  echo "Command line arguments not listed above are passed thru to msbuild."
  echo "The above arguments can be shortened as much as to be unambiguous (e.g. -co for configuration, -t for test, etc.)."
}

verbosity='minimal'
configuration='Debug'
properties=''
connectionString='Server=localhost;Port=5432;Database=bookstoreuser;User Id=postgres;Password=BookStore@123;'

restore=false
build=false
clean=false
test=false
integrationTest=false
publish=false
migrations=false

while [ $# -gt 0 ]
do
  opt=$1
  #"$(echo "${1/#--/-})" | awk '{print tolower($0)}')"
  case $opt in
    -help|-h)
      usage
      exit 0
      ;;
    --clean)
      clean=true
      ;;
    --configuration|-c)
      configuration=$2
      shift
      ;;
    --verbosity|-v)
      verbosity=$2
      shift
      ;;
    --restore|-r)
      restore=true
      ;;
    --build|-b)
      build=true
      ;;
    --test|-t)
      test=true
      ;;
    --integrationtest)
      integration_test=true
      ;;
    --publish)
      publish=true
      ;;
    --migrations|-m)
      migrations=true
      build=true
      ;;
    --connectionString|-cs)
      connectionString=$2
      shift
      ;;
    *)
      properties="$properties $1"
      ;;
  esac

  shift
done

if [ "$clean" = true ]; then 
   dotnet clean ../Users.sln
fi

if [ "$restore" = true ]; then 
  dotnet restore ../Users.sln
fi

if [ "$build" = true ]; then 
  dotnet build ../Users.sln -c $configuration
fi

if [ "$migrations" = true ]; then 
  dotnet tool restore
  dotnet fm migrate -p Postgres -c "$connectionString" -a ../src/Users.Migrations/bin/$configuration/netstandard2.0/Users.Migrations.dll
fi

exit 0
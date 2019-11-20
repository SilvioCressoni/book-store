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
  echo "  --workDir <value>        Work directory (short: -w)"
  echo "  --artifacts <value>      Artifacts output (short: -a)"
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
  echo " --docker                 Generate docker image (short: -d)"
  echo ""

  echo "Command line arguments not listed above are passed thru to msbuild."
  echo "The above arguments can be shortened as much as to be unambiguous (e.g. -co for configuration, -t for test, etc.)."
}

verbosity='minimal'
configuration='Debug'
properties=''
workDir='..'
artifacts='../.artifacts'
connectionString='Server=localhost;Port=5432;Database=bookstoreuser;User Id=postgres;Password=BookStore@123;'

restore=false
build=false
clean=false
test=false
integrationTest=false
publish=false
migrations=false
docker=false

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
      configuration="Release"
      ;;
    --migrations|-m)
      migrations=true
      build=true
      ;;
    --connectionString|-cs)
      connectionString=$2
      shift
      ;;
    --workDir|-w)
      workDir=$2
      shift
      ;;
    --artifacts|-a)
      artifacts=$2
      shift
      ;;
    --docker|-d)
      docker=true
      publish=true
      configuration="Release"
      ;;
    *)
      properties="$properties $1"
      ;;
  esac

  shift
done

if [ "$clean" = true ]; then 
  echo "======Clean solution==========="
   dotnet clean "$workDir/Users.sln"
fi

if [ "$restore" = true ]; then 
  echo "======Restore solution==========="
  dotnet restore "$workDir/Users.sln"
fi

if [ "$build" = true ]; then 
  echo "======Build solution==========="
  dotnet build "$workDir/Users.sln" -c $configuration
fi

if [ "$publish" = true ]; then
  echo "======Publish solution==========="
  dotnet publish "$workDir/src/Users.Web" -c $configuration -o $artifacts
fi

if [ "$migrations" = true ]; then 
  echo "======Run migration==========="
  dotnet tool restore
  dotnet fm migrate -p Postgres -c "$connectionString" -a ../src/Users.Migrations/bin/$configuration/netstandard2.0/Users.Migrations.dll
fi

if [ "$docker" = true ]; then 
  echo "======Docker project==========="
  dotnet tool restore
  dotnet fm migrate -p Postgres -c "$connectionString" -a ../src/Users.Migrations/bin/$configuration/netstandard2.0/Users.Migrations.dll
fi

exit 0
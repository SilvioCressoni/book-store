[CmdletBinding(PositionalBinding=$false)]
Param(
  [string][Alias('c')]$configuration = "Debug",
  [string][Alias('v')]$verbosity = "minimal",
  [string][Alias('w')]$workDir = ".",
  [string][Alias('a')]$artifacts = ".\.artifacts",
  [string][Alias('cs')]$connectionString = "Server=localhost;Port=5432;Database=bookstoreuser;User Id=postgres;Password=BookStore@123;",
  [switch][Alias('r')]$restore,
  [switch][Alias('b')]$build,
  [switch] $clean,
  [switch][Alias('t')]$test,
  [switch] $integrationTest,
  [switch] $publish,
  [switch] $migrations,
  [switch] $docker,
  [switch] $help,
  [Parameter(ValueFromRemainingArguments=$true)][String[]]$properties
)

function Print-Usage() {
  Write-Host "Common settings"
  Write-Host "  -configuration <value>  Build configuration: 'Debug' or 'Release' (short: -c)"
  Write-Host "  -verbosity <value>      Msbuild verbosity: q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic] (short: -v)"
  Write-Host "  -help                   Print help and exit"
  Write-Host ""

  Write-Host "Actions:"
  Write-Host " -restore                Restore dependencies (short: -r)"
  Write-Host " -build                  Build solution (short: -b)"
  Write-Host " -clean                  Clean the solution"
  Write-Host " -test                   Run all unit tests in the solution (short: -t)"
  Write-Host " -integrationTest        Run all integration tests in the solution"
  Write-Host " -publish                Publish artifacts (e.g. symbols)"
  Write-Host " --migrations            Run migrations(short: -m)"
  Write-Host " --connectionString      Connection string (short: -cs)"
  Write-Host " --docker                Generate docker image (short: -d)"
  Write-Host ""

  Write-Host "Command line arguments not listed above are passed thru to msbuild."
  Write-Host "The above arguments can be shortened as much as to be unambiguous (e.g. -co for configuration, -t for test, etc.)."
}

function Build {
  dotnet build -c $configuration "$workDir\Users.sln"
}


try {
  if($help -or (($null -ne $properties) -and ($properties.Contains("/help") -or $properties.Contains("/?")))) {
    Print-Usage
    exit 0
  }

  $configuration = if (Test-Path variable:configuration) { $configuration } else { if($publish) { "Release" } else {  "Debug" } }
  $restore = if(Test-Path variable:restore) { $restore } else { $true }
  $artifacts = if(Test-Path variable:artifacts) { $artifacts } else { "..\.artifacts" }
  $workDir = if(Test-Path variable:workDir) { $workDir } else { "." }

  if($clean) {
    dotnet clean "$workDir\Users.sln"
  }

  if($restore) {
    dotnet restore "$workDir\Users.sln"
  }

  if($build) {
    Build
  }

  if($publish) {
    dotnet publish -c $configuration "$workDir\src\Users.Web" -o $artifacts -r linux-x64
  }

  if($migrations) {
    Build
    dotnet tool restore
    dotnet fm migrate -p Postgres -c "$connectionString" -a "$workDir/src/Users.Migrations/bin/$configuration/netstandard2.0/Users.Migrations.dll"
  }

  if($docker) {
    docker build -t book-store-user .
  }

} catch {
  Write-Host $_.ScriptStackTrace
  exit 1
}

exit 0
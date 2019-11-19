[CmdletBinding(PositionalBinding=$false)]
Param(
  [string][Alias('c')]$configuration = "Debug",
  [string][Alias('v')]$verbosity = "minimal",
  [switch][Alias('r')]$restore,
  [switch][Alias('b')]$build,
  [switch] $clean,
  [switch][Alias('t')]$test,
  [switch] $integrationTest,
  [switch] $publish,
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
  Write-Host ""

  Write-Host "Command line arguments not listed above are passed thru to msbuild."
  Write-Host "The above arguments can be shortened as much as to be unambiguous (e.g. -co for configuration, -t for test, etc.)."
}

function Build {

}

try {
  if($help -or (($null -ne $properties) -and ($properties.Contains("/help") -or $properties.Contains("/?")))) {
    Print-Usage
    exit 0
  }

  $configuration = if (Test-Path variable:configuration) { 
    $configuration
  } else {
    "Debug"
  }

  $restore = if(Test-Path variable:restore) {
    $restore
  } else {
    $true
  }

  if($clean) {
    dotnet clean ../Users.sln
  }

  if($restore) {
    dotnet restore ../Users.sln
  }

  if($build) {
    dotnet build -c $configuration ../Users.sln
  }

} catch {
  Write-Host $_.ScriptStackTrace
  exit 1
}

exit 0
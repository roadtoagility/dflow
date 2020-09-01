# Taken from https://github.com/connellw/Firestorm/blob/master/build.ps1

<#
.SYNOPSIS
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
function Exec
{
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,
        [Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0) {
        throw ("Exec: " + $errorMessage)
    }
}

$root = (Get-Item -Path ".\").FullName
$artifactsPath = $root + "\artifacts"
if(Test-Path $artifactsPath) { Remove-Item $artifactsPath -Force -Recurse }

$branch = @{ $true = $env:APPVEYOR_PULL_REQUEST_HEAD_REPO_BRANCH; $false = @{ $true = $env:APPVEYOR_REPO_BRANCH; $false = $(git symbolic-ref --short -q HEAD) }[$env:APPVEYOR_REPO_BRANCH -ne $NULL] }[$env:APPVEYOR_PULL_REQUEST_HEAD_REPO_BRANCH -ne $NULL];
$suffix = @{ $true = ""; $false = "build$($env:APPVEYOR_BUILD_NUMBER)"}[$branch -eq "master"];
$commitHash = $(git rev-parse --short HEAD);
$buildSuffix = @{$true="$($env:APPVEYOR_REPO_TAG_NAME)";$false=@{ $true = "$($suffix)-$($commitHash)"; $false = "$($branch)" }[$suffix -ne ""]}[$env:APPVEYOR_REPO_TAG -eq "true"];
$versionSuffix = @{$true="--version-suffix=$($buildSuffix)";$false=@{ $true = "--version-suffix=$($suffix)"; $false = ""}[$suffix -ne ""]}[$env:APPVEYOR_REPO_TAG -eq "true"];

# $suffix = @{ $true = "$($env:APPVEYOR_REPO_TAG_NAME)"; $false = $suffix}[$env:APPVEYOR_REPO_TAG -eq "true"]

echo "Build: Package version suffix is $suffix"
echo "Build: Build version suffix is $buildSuffix"

# Update Appveyor version
if (Test-Path env:APPVEYOR) {
   
    $avSuffix = @{ $true = $($suffix); $false = $buildSuffix }[$suffix -ne ""]
    $full = @{ $true = "$($avSuffix)"; $false = $($prefix) }[-not ([string]::IsNullOrEmpty($avSuffix))]
    
    echo "Build: Full version is $full"
    Update-AppveyorBuild -Version $full
}

# Build
echo "`n`n----- BUILD -----`n"

exec { & dotnet build DFlow.sln -c Release --version-suffix=$buildSuffix }

# Test
echo "`n`n----- TEST -----`n"

exec { & dotnet tool install --global coverlet.console }

$testDirs  = @(Get-ChildItem -Path tests -Include "*.Tests" -Directory -Recurse)

$i = 0
ForEach ($folder in $testDirs) { 
    echo "Testing $folder"

    $i++
    $format = @{ $true = "/p:CoverletOutputFormat=opencover"; $false = ""}[$i -eq $testDirs.Length ]

    exec { & dotnet test $folder.FullName -c Release --no-build --no-restore /p:CollectCoverage=true /p:CoverletOutput=$root\coverage /p:MergeWith=$root\coverage.json /p:Include="[*]DFlow.*" /p:Exclude="[*]DFlow.Tests.*%2c[*]DFlow.Example.*" $format }
}

choco install codecov --no-progress
exec { & codecov -f "$root\coverage.opencover.xml" }

# Pack
echo "`n`n----- PACK -----`n"

exec { & dotnet pack -c Release -o $artifactsPath --include-symbols --no-build $versionSuffix }
# Originally taken from https://github.com/jbogard/MediatR and https://github.com/psake/psake
# taken from https://connellw.github.io/open-source-ci-pipeline-net-appveyor/

function Exec {
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

# $branch = @{ $true = $env:APPVEYOR_PULL_REQUEST_HEAD_REPO_BRANCH; $false = @{ $true = $env:APPVEYOR_REPO_BRANCH; $false = $(git symbolic-ref --short -q HEAD) }[$env:APPVEYOR_REPO_BRANCH -ne $NULL] }[$env:APPVEYOR_PULL_REQUEST_HEAD_REPO_BRANCH -ne $NULL];
# $revision = @{ $true = "{0:00000}" -f [convert]::ToInt32("0" + $env:APPVEYOR_BUILD_NUMBER, 10); $false = "local" }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
# $suffix = @{ $true = ""; $false = "$($branch.Substring(0, [math]::Min(10,$branch.Length)))-$revision"}[$branch -eq "master" -and $revision -ne "local"]
# $commitHash = $(git rev-parse --short HEAD)
# $buildSuffix = @{ $true = "$($suffix)-$($commitHash)"; $false = "$($branch)-$($commitHash)" }[$suffix -ne ""]
# $versionSuffix = @{ $true = "--version-suffix=$($suffix)"; $false = ""}[$suffix -ne ""]

# echo "Build: Package version suffix is $suffix"
# echo "Build: Build version suffix is $buildSuffix"

# # Update Appveyor version
# if (Test-Path env:APPVEYOR) {
#     $props = [xml](Get-Content "src\Directory.Build.props")
#     $prefix = $props.Project.PropertyGroup.VersionPrefix
    
#     $avSuffix = @{ $true = $($suffix); $false = $props.Project.PropertyGroup.VersionSuffix }[$suffix -ne ""]
#     $full = @{ $true = "$($prefix)-$($avSuffix)"; $false = $($prefix) }[-not ([string]::IsNullOrEmpty($avSuffix))]
    
#     echo "Build: Full version is $full"
#     Update-AppveyorBuild -Version $full
# }

# Build
echo "`n`n----- BUILD -----`n"

exec { & dotnet build DFlow.sln -c Release --version-suffix=$buildSuffix }

# Test
echo "`n`n----- TEST -----`n"

exec { & dotnet tool install --global coverlet.console }

$testDirs  = @(Get-ChildItem -Path tests -Include "*.Tests" -Directory -Recurse)
# $testDirs += @(Get-ChildItem -Path tests -Include "*.IntegrationTests" -Directory -Recurse)
# $testDirs += @(Get-ChildItem -Path tests -Include "*FunctionalTests" -Directory -Recurse)

$i = 0
ForEach ($folder in $testDirs) { 
    echo "Testing $folder"

    $i++
    $format = @{ $true = "/p:CoverletOutputFormat=opencover"; $false = ""}[$i -eq $testDirs.Length ]

    exec { & dotnet test $folder.FullName -c Release --no-build --no-restore /p:CollectCoverage=true /p:CoverletOutput=$root\coverage /p:MergeWith=$root\coverage.json /p:Include="[*]DFlow.*" /p:Exclude="[*]DFlow.Tests.*" $format }
}

choco install codecov --no-progress
exec { & codecov -f "$root\coverage.opencover.xml" }

# Pack
echo "`n`n----- PACK -----`n"

exec { & dotnet pack -c Release -o $artifactsPath --include-symbols --no-build $versionSuffix }
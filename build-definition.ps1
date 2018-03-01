Properties {

    # This number will be appended to all nuget package versions
    # This should be overwritten by a CI system like VSTS, AppVeyor, TeamCity, ...
    $VersionSuffix = "loc" + ((Get-Date).ToUniversalTime().ToString("yyyyMMddHHmm"))

    # The build configuration used for compilation
    $BuildConfiguration = "Release"

    # The folder in which all output packages should be placed
    $ArtifactsPath = Join-Path $PWD "artifacts"

    # Artifacts-subfolder in which test results will be placed
    $ArtifactsPathTests = "tests"

    # Artifacts-subfolder in which NuGet packages will be placed
    $ArtifactsPathNuGet = "nuget"

    # A list of projects for which NuGet packages should be created
    $NugetLibraries = @(
        "src/OpenTracing"
    )
}

FormatTaskName ("`n" + ("-"*25) + "[{0}]" + ("-"*25) + "`n")

Task Default -depends init, clean, dotnet-install, dotnet-build, dotnet-test, dotnet-pack

Task init {

    Write-Host "VersionSuffix: $VersionSuffix"
    Write-Host "BuildConfiguration: $BuildConfiguration"
    Write-Host "ArtifactsPath: $ArtifactsPath"
    Write-Host "ArtifactsPathTests: $ArtifactsPathTests"
    Write-Host "ArtifactsPathNuGet: $ArtifactsPathNuGet"

    Assert ($BuildConfiguration -ne $null) "Property 'BuildConfiguration' may not be null."
    Assert ($ArtifactsPath -ne $null) "Property 'ArtifactsPath' may not be null."
    Assert ($ArtifactsPathTests -ne $null) "Property 'ArtifactsPathTests' may not be null."
    Assert ($ArtifactsPathNuGet -ne $null) "Property 'ArtifactsPathNuGet' may not be null."
}

Task clean {

    if (Test-Path $ArtifactsPath) { Remove-Item -Path $ArtifactsPath -Recurse -Force -ErrorAction Ignore }
    New-Item $ArtifactsPath -ItemType Directory -ErrorAction Ignore | Out-Null

    Write-Host "Created artifacts folder '$ArtifactsPath'"
}

Task dotnet-install {

    if (Get-Command "dotnet.exe" -ErrorAction SilentlyContinue) {
        Write-Host "dotnet SDK already installed"
        exec { dotnet --version }
    } else {
        Write-Host "Installing dotnet SDK"

        $installScript = Join-Path $ArtifactsPath "dotnet-install.ps1"

        Invoke-WebRequest "https://raw.githubusercontent.com/dotnet/cli/rel/1.0.0/scripts/obtain/dotnet-install.ps1" `
            -OutFile $installScript

        & $installScript
    }
}

Task dotnet-build {

    # --no-incremental to ensure that CI builds always result in a clean build
    exec { dotnet build -c $BuildConfiguration --no-incremental /p:CI=true /p:VersionSuffix=$VersionSuffix }
}

Task dotnet-test {

    $testOutput = Join-Path $ArtifactsPath $ArtifactsPathTests
    New-Item $testOutput -ItemType Directory -ErrorAction Ignore | Out-Null

    $testsFailed = $false

    Get-ChildItem .\test -Filter *.csproj -Recurse | ForEach-Object {

        $library = Split-Path $_.DirectoryName -Leaf

        Write-Host ""
        Write-Host "Testing $library"
        Write-Host ""

        dotnet test $_.FullName -c $BuildConfiguration --no-build
        if ($LASTEXITCODE -ne 0) {
            $testsFailed = $true
        }
    }

    if ($testsFailed) {
        throw "at least one test failed"
    }
}

Task dotnet-pack {

    if ($NugetLibraries -eq $null -or $NugetLibraries.Count -eq 0) {
        Write-Host "No NugetLibraries configured"
        return
    }

    $NugetLibraries | ForEach-Object {

        $library = $_
        $libraryOutput = Join-Path $ArtifactsPath $ArtifactsPathNuGet

        Write-Host ""
        Write-Host "Packaging $library to $libraryOutput"
        Write-Host ""

        exec { dotnet pack $library -c $BuildConfiguration --no-restore --no-build -o $libraryOutput /p:CI=true /p:VersionSuffix=$VersionSuffix }
    }
}

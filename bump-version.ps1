param (
    [switch]$Major,
    [switch]$Minor,
    [switch]$Patch,

    # A fixed string - may not be used in combination with version switches.
    [string]$Version
)

$ErrorActionPreference = "Stop"

#########################
# Settings

# The current version will be read from this file
$projectFile = "src\OpenTracing\project.json"

# All projects and dependencies starting with this name will be updated.
$packagePrefix = "OpenTracing"


#########################
# Validate arguments

$selectedSwitches = 0
if ($Major -eq $true) { $selectedSwitches++ }
if ($Minor -eq $true) { $selectedSwitches++ }
if ($Patch -eq $true) { $selectedSwitches++ }

if ([String]::IsNullOrWhiteSpace($Version) -and $selectedSwitches -eq 0) {
    throw "There must be either a version switch (-Major, -Minor, -Patch) or a fixed version string (-Version)"
}

if (![String]::IsNullOrWhiteSpace($Version) -and $selectedSwitches -gt 0) {
    throw "'-Version' can not be used in combination with version switches"
}

if ($selectedSwitches -gt 1) {
    throw "Only one version switch can be used"
}


#########################
# Get current version from main project

$projectFileJson = Get-Content -Path $projectFile -Raw | ConvertFrom-Json -ErrorAction Ignore

$currentVersion = $projectFileJson.version

$versionParts = $currentVersion.TrimEnd("*-").Split(".")
$currentMajor = [Convert]::ToInt32($versionParts[0])
$currentMinor = [Convert]::ToInt32($versionParts[1])
$currentPatch = [Convert]::ToInt32($versionParts[2])

Write-Output "Current version: $currentVersion (Major: $currentMajor Minor: $currentMinor Patch: $currentPatch)"


#########################
# Bump version

if (![String]::IsNullOrWhiteSpace($Version)) {
    $Version = $Version.Trim().TrimEnd("*-")

    if (!($Version -match '([0-9]+)\.([0-9]+)\.([0-9]+)(?:(\-[0-9A-Za-z-]+(?:\.[0-9A-Za-z-]+)*))?(?:\+[0-9A-Za-z-\-\.]+)?')) {
        throw "$Version is not a valid version number"
    }

    $newVersion = "$Version-*"
} else {
    if ($Major -eq $true) {
        $currentMajor++
        $currentMinor = 0
        $currentPatch = 0
    }
    if ($Minor -eq $true) {
        $currentMinor++
        $currentPatch = 0
    }
    if ($Patch -eq $true) {
        $currentPatch++
    }

    $newVersion = "$currentMajor.$currentMinor.$currentPatch-*"
}

Write-Output "New version: $newVersion"


#########################
# Update all library versions and dependencies in all projects

# The PowerShell JSON indentation is incredibly ugly and it reformats the whole document.
# That's why we use simple string replace for the actual file update.
# There's a higher chance for this to go wrong, but meeh...

Get-ChildItem -Filter project.json -Recurse -Depth 5 | ForEach-Object {
    Write-Output ("Processing " + $_.DirectoryName)

    $fileChanged = $false
    $fileContent = [IO.File]::ReadAllText($_.FullName)
    $json = $fileContent | ConvertFrom-Json -ErrorAction Ignore

    # Should we update the library version?
    if ($_.Directory.Name.StartsWith($packagePrefix) -eq $true -and $json.version -ne $null) {

        $oldFileContent = $fileContent
        $fileContent = $fileContent.Replace("""version"": ""$currentVersion""", """version"": ""$newVersion""")
        $fileChanged = $true

        if ($oldFileContent -eq $fileContent) { throw "string.Replace for version failed!" }

        Write-Output " - Changed version"
    }

    if ($json.dependencies -ne $null) {
        $json.dependencies `
            | Get-Member -MemberType NoteProperty `
            | Where-Object { $_.Name.StartsWith($packagePrefix) } `
            | Foreach-Object {
                $name = $_.Name

                $oldFileContent = $fileContent
                $fileContent = $fileContent.Replace("""$name"": ""$currentVersion""", """$name"": ""$newVersion""")
                $fileChanged = $true

                if ($oldFileContent -eq $fileContent) { throw "string.Replace for dependency '$name' failed!" }

                Write-Output "  - Updated dependency '$name'"
            }
    }

    if ($fileChanged -eq $true) {
        # Out-File adds the BOM to the encoding so we have to use a different method
        # http://stackoverflow.com/questions/5596982/using-powershell-to-write-a-file-in-utf-8-without-the-bom
        [IO.File]::WriteAllText($_.FullName, $fileContent)
    }
}

Write-Output "Finished!"
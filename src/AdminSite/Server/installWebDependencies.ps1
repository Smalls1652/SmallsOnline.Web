[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [switch]$ForceHighlightJsInstall
)

$scriptRoot = $PSScriptRoot

$writeInfoSplat = @{
    "InformationAction" = "Continue";
}

$nodeModulesPath = Join-Path -Path $scriptRoot -ChildPath "node_modules"

if (!(Test-Path -Path $nodeModulesPath)) {
    Write-Information @writeInfoSplat -MessageData "`t| Running 'npm install'"
    npm install
}

$tempPath = [System.IO.Path]::GetTempPath()

$bootstrapCssPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap\dist\css\bootstrap.min.css"
$bootstrapCssMapPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap\dist\css\bootstrap.min.css.map"
$bootstrapOutPath = Join-Path -Path $scriptRoot -ChildPath "wwwroot\css\bootstrap\"

$bootstrapIconsCssPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap-icons\font\bootstrap-icons.css"
$bootstrapIconsFontDirPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap-icons\font\fonts\"
$bootstrapIconsOutPath = Join-Path -Path $scriptRoot -ChildPath "wwwroot\css\bootstrap-icons\"

$highlightJsRepoTag = "11.9.0"
$highlightJsSrcPath = Join-Path -Path $tempPath -ChildPath "highlight.js\"
$highlightJsBuildToolPath = Join-Path -Path $highlightJsSrcPath -ChildPath "tools\build.js"
$highlightJsPath = Join-Path -Path $highlightJsSrcPath -ChildPath "build\highlight.min.js"
$highlightJsCssPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\@highlightjs\cdn-assets\styles\github.min.css"
$highlightJsOutPath = Join-Path -Path $scriptRoot -ChildPath "wwwroot\highlight.js\"
$highlightJsCssOutPath = Join-Path -Path $scriptRoot -ChildPath "wwwroot\highlight.js\styles\"

if (!(Test-Path -Path $bootstrapOutPath)) {
    $null = New-Item -Path $bootstrapOutPath -ItemType "Directory"
}

foreach ($fileItem in (Get-ChildItem -Path $bootstrapOutPath)) {
    Write-Information @writeInfoSplat -MessageData "`t| Removing '$($fileItem.Name)'"
    Remove-Item -Path $fileItem.FullName -Force
}

Write-Information @writeInfoSplat -MessageData "`t| bootstrap.min.css-> $($bootstrapOutPath)"
Copy-Item -Path $bootstrapCssPath -Destination $bootstrapOutPath -ErrorAction "Stop"

Write-Information @writeInfoSplat -MessageData "`t| bootstrap.min.css.map-> $($bootstrapOutPath)"
Copy-Item -Path $bootstrapCssMapPath -Destination $bootstrapOutPath -ErrorAction "Stop"

if (!(Test-Path -Path $bootstrapIconsOutPath)) {
    $null = New-Item -Path $bootstrapIconsOutPath -ItemType "Directory"
}

foreach ($fileItem in (Get-ChildItem -Path $bootstrapIconsOutPath)) {
    Write-Information @writeInfoSplat -MessageData "`t| Removing '$($fileItem.Name)'"
    Remove-Item -Path $fileItem.FullName -Force -Recurse
}

Write-Information @writeInfoSplat -MessageData "`t| bootstrap-icons.css-> $($bootstrapIconsOutPath)"
Copy-Item -Path $bootstrapIconsCssPath -Destination $bootstrapIconsOutPath -ErrorAction "Stop"

Write-Information @writeInfoSplat -MessageData "`t| fonts\-> $($bootstrapIconsOutPath)"
Copy-Item -Path $bootstrapIconsFontDirPath -Destination $bootstrapIconsOutPath -Recurse -ErrorAction "Stop"

# ---- popper.js ----

$popperJsPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\@popperjs\core\dist\cjs\popper.js"
$popperJsOutPath = Join-Path -Path $scriptRoot -ChildPath "wwwroot\js\popper\"

# Create output directory if it doesn't exist
if (!(Test-Path -Path $popperJsOutPath)) {
    $null = New-Item -Path $popperJsOutPath -ItemType "Directory" -Force
}

# Remove any existing item in the directory
foreach ($fileItem in (Get-ChildItem -Path $popperJsOutPath)) {
    Write-Information @writeInfoSplat -MessageData "`t| Removing '$($fileItem.Name)'"
    Remove-Item -Path $fileItem.FullName -Force
}

# Copy the files
Write-Information @writeInfoSplat -MessageData "`t| popper.js-> $($popperJsOutPath)"
Copy-Item -Path $popperJsPath -Destination $popperJsOutPath -ErrorAction "Stop"

# ---- Bootstrap JS ----

# Bootstrap JS file paths
$bootstrapJsPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap\dist\js\bootstrap.bundle.min.js"
$bootstrapJsMapPath = Join-Path -Path $scriptRoot -ChildPath "node_modules\bootstrap\dist\js\bootstrap.bundle.min.js.map"
$bootstrapJsOutPath = Join-Path -Path $scriptRoot -ChildPath "wwwroot\js\bootstrap\"

# Create output directory if it doesn't exist
if (!(Test-Path -Path $bootstrapJsOutPath)) {
    $null = New-Item -Path $bootstrapJsOutPath -ItemType "Directory" -Force
}

# Remove any existing item in the directory
foreach ($fileItem in (Get-ChildItem -Path $bootstrapJsOutPath)) {
    Write-Information @writeInfoSplat -MessageData "`t| Removing '$($fileItem.Name)'"
    Remove-Item -Path $fileItem.FullName -Force
}

# Copy the files
Write-Information @writeInfoSplat -MessageData "`t| bootstrap.bundle.min.js-> $($bootstrapJsOutPath)"
Copy-Item -Path $bootstrapJsPath -Destination $bootstrapJsOutPath -ErrorAction "Stop"

Write-Information @writeInfoSplat -MessageData "`t| bootstrap.bundle.min.js.map-> $($bootstrapJsOutPath)"
Copy-Item -Path $bootstrapJsMapPath -Destination $bootstrapJsOutPath -ErrorAction "Stop"

if ((!(Test-Path -Path $highlightJsOutPath) -or !(Test-Path -Path $highlightJsCssOutPath)) -or $ForceHighlightJsInstall) {
    
    Write-Information @writeInfoSplat -MessageData "`t| Customizing highlight.js"
    Write-Information @writeInfoSplat -MessageData "`t`t| Pulling highlight.js repo"
    Start-Process -FilePath "git" -Wait -WorkingDirectory $tempPath -ArgumentList @(
        "clone",
        "https://github.com/highlightjs/highlight.js.git"
    )

    Write-Information @writeInfoSplat -MessageData "`t`t| Changing highlight.js to tag '$($highlightJsRepoTag)'"
    Start-Process -FilePath "git" -Wait -WorkingDirectory $highlightJsSrcPath -ArgumentList @(
        "switch",
        "--detach",
        "$($highlightJsRepoTag)"
    )

    Write-Information @writeInfoSplat -MessageData "`t`t| Building highlight.js from source"
    $highlightJsBuildProcSplat = @{
        "Wait"             = $true;
        "WorkingDirectory" = $highlightJsSrcPath;
    }
    Start-Process @highlightJsBuildProcSplat -FilePath "npm" -ArgumentList @(
        "install"
    )

    Start-Process @highlightJsBuildProcSplat -FilePath "node" -ArgumentList @(
        "`"$($highlightJsBuildToolPath)`"",
        ":common",
        "powershell",
        "dockerfile"
    )

    if (Test-Path -Path $highlightJsOutPath) {
        Remove-Item -Path $highlightJsOutPath -Recurse -Force
    }

    $null = New-Item -Path $highlightJsOutPath -ItemType "Directory"
    $null = New-Item -Path $highlightJsCssOutPath -ItemType "Directory"

    Write-Information @writeInfoSplat -MessageData "`t| highlight.min.js-> $($highlightJsOutPath)"
    Copy-Item -Path $highlightJsPath -Destination $highlightJsOutPath -ErrorAction "Stop"

    Write-Information @writeInfoSplat -MessageData "`t| styles\github.min.css-> $($highlightJsCssOutPath)"
    Copy-Item -Path $highlightJsCssPath -Destination $highlightJsCssOutPath -Recurse -ErrorAction "Stop"

    Write-Information @writeInfoSplat -MessageData "`t| Cleaning up build files"
    Remove-Item -Path $highlightJsSrcPath -Recurse -Force
}

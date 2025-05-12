# Build script for Fanex.NAP.SignIn.Framework NuGet package

param(
    [string]$Configuration = "Release",
    [string]$Version = "1.0.0"
)

Write-Host "Building Fanex.NAP.SignIn.Framework package version $Version" -ForegroundColor Green

# Clean previous builds
Write-Host "Cleaning previous builds..." -ForegroundColor Yellow
if (Test-Path "bin") {
    Remove-Item -Recurse -Force "bin"
}
if (Test-Path "obj") {
    Remove-Item -Recurse -Force "obj"
}

# Restore NuGet packages
Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
nuget restore Fanex.NAP.SignIn.Framework.csproj

# Build the project
Write-Host "Building project in $Configuration configuration..." -ForegroundColor Yellow
msbuild Fanex.NAP.SignIn.Framework.csproj /p:Configuration=$Configuration /p:Platform="Any CPU" /verbosity:minimal

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

# Create NuGet package
Write-Host "Creating NuGet package..." -ForegroundColor Yellow
nuget pack Fanex.NAP.SignIn.Framework.nuspec -Version $Version -OutputDirectory "bin\$Configuration"

if ($LASTEXITCODE -ne 0) {
    Write-Host "Package creation failed!" -ForegroundColor Red
    exit 1
}

Write-Host "Package created successfully!" -ForegroundColor Green
Write-Host "Package location: bin\$Configuration\Fanex.NAP.SignIn.Framework.$Version.nupkg" -ForegroundColor Cyan

# Optional: Push to NuGet (uncomment if you have API key)
# Write-Host "Pushing to NuGet..." -ForegroundColor Yellow
# nuget push "bin\$Configuration\Fanex.NAP.SignIn.Framework.$Version.nupkg" -Source "https://api.nuget.org/v3/index.json" -ApiKey "YOUR-API-KEY" 
# Test build script for Fanex.NAP.SignIn.Framework

Write-Host "Testing build with security transparency fix..." -ForegroundColor Green

# Clean previous builds
Write-Host "Cleaning previous builds..." -ForegroundColor Yellow
if (Test-Path "bin") {
    Remove-Item -Recurse -Force "bin"
}
if (Test-Path "obj") {
    Remove-Item -Recurse -Force "obj"
}

# Build the project
Write-Host "Building project..." -ForegroundColor Yellow
msbuild Fanex.NAP.SignIn.Framework.csproj /p:Configuration=Release /p:Platform="Any CPU" /verbosity:minimal

if ($LASTEXITCODE -eq 0) {
    Write-Host "Build completed successfully!" -ForegroundColor Green
    Write-Host "Assembly location: bin\Release\Fanex.NAP.SignIn.dll" -ForegroundColor Cyan
    
    # Check if the assembly was created
    if (Test-Path "bin\Release\Fanex.NAP.SignIn.dll") {
        Write-Host "Assembly file exists!" -ForegroundColor Green
    } else {
        Write-Host "Assembly file not found!" -ForegroundColor Red
    }
} else {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
} 
# publish-all.ps1
Write-Host "Starting multi-platform publish..." -ForegroundColor Green

Write-Host "`nPublishing for Windows (win-x64)..." -ForegroundColor Cyan
dotnet publish -c Release -r win-x64 --self-contained

if ($LASTEXITCODE -ne 0) {
    Write-Host "Windows publish failed!" -ForegroundColor Red
    exit 1
}

# Executable umbenennen
Rename-Item "bin/Release/net9.0/win-x64/publish/OfficeTracker.exe" "OfficeTracker-win-x64.exe" -ErrorAction SilentlyContinue

Write-Host "`nPublishing for Linux (linux-x64)..." -ForegroundColor Cyan
dotnet publish -c Release -r linux-x64 --self-contained

if ($LASTEXITCODE -ne 0) {
    Write-Host "Linux publish failed!" -ForegroundColor Red
    exit 1
}

# Executable umbenennen
Rename-Item "bin/Release/net9.0/linux-x64/publish/OfficeTracker" "OfficeTracker-linux-x64" -ErrorAction SilentlyContinue

Write-Host "`nAll platforms published successfully!" -ForegroundColor Green

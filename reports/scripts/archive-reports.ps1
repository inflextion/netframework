# Archive Reports Script
# Archives current reports with timestamp and optional build number

param(
    [string]$BuildNumber = "manual",
    [string]$Branch = "unknown",
    [switch]$KeepCurrent
)

$reportsDir = "reports"
$timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
$archiveName = "$timestamp-build-$BuildNumber-$Branch"
$archivePath = "$reportsDir\archive\$archiveName"

Write-Host "Archiving reports..." -ForegroundColor Blue
Write-Host "Archive name: $archiveName" -ForegroundColor Cyan

# Check if there are reports to archive
if (-not (Test-Path "$reportsDir\allure-report") -and -not (Test-Path "$reportsDir\allure-results")) {
    Write-Host "❌ No reports found to archive" -ForegroundColor Red
    exit 1
}

# Create archive directory
New-Item -ItemType Directory -Path $archivePath -Force | Out-Null

# Archive allure-report
if (Test-Path "$reportsDir\allure-report") {
    Copy-Item -Recurse -Force "$reportsDir\allure-report" "$archivePath\"
    Write-Host "✓ Archived allure-report" -ForegroundColor Green
}

# Archive allure-results
if (Test-Path "$reportsDir\allure-results") {
    Copy-Item -Recurse -Force "$reportsDir\allure-results" "$archivePath\"
    Write-Host "✓ Archived allure-results" -ForegroundColor Green
}

# Create metadata file
$metadata = @{
    Timestamp = $timestamp
    BuildNumber = $BuildNumber
    Branch = $Branch
    ArchiveName = $archiveName
    CreatedBy = $env:USERNAME
    Machine = $env:COMPUTERNAME
} | ConvertTo-Json -Depth 2

$metadata | Out-File -FilePath "$archivePath\metadata.json" -Encoding UTF8
Write-Host "✓ Created metadata.json" -ForegroundColor Green

# Clean current reports unless -KeepCurrent is specified
if (-not $KeepCurrent) {
    if (Test-Path "$reportsDir\allure-report") {
        Remove-Item -Recurse -Force "$reportsDir\allure-report"
        Write-Host "✓ Cleaned current allure-report" -ForegroundColor Yellow
    }
    
    if (Test-Path "$reportsDir\allure-results") {
        Remove-Item -Recurse -Force "$reportsDir\allure-results"
        Write-Host "✓ Cleaned current allure-results" -ForegroundColor Yellow
    }
}

Write-Host "✅ Reports archived to: $archivePath" -ForegroundColor Green

# Show archive size
$archiveSize = (Get-ChildItem -Recurse $archivePath | Measure-Object -Property Length -Sum).Sum
$archiveSizeMB = [math]::Round($archiveSize / 1MB, 2)
Write-Host "Archive size: $archiveSizeMB MB" -ForegroundColor Cyan
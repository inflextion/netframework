# Clean Reports Script
# Removes all generated reports and results

param(
    [switch]$KeepHistory,
    [switch]$Force
)

$reportsDir = "reports"

if (-not $Force) {
    $confirmation = Read-Host "This will delete all reports and results. Continue? (y/N)"
    if ($confirmation -ne 'y' -and $confirmation -ne 'Y') {
        Write-Host "Operation cancelled." -ForegroundColor Yellow
        exit
    }
}

Write-Host "Cleaning reports directory..." -ForegroundColor Blue

# Remove allure-results
if (Test-Path "$reportsDir/allure-results") {
    Remove-Item -Recurse -Force "$reportsDir/allure-results"
    Write-Host "✓ Removed allure-results" -ForegroundColor Green
}

# Remove allure-report
if (Test-Path "$reportsDir/allure-report") {
    if ($KeepHistory -and (Test-Path "$reportsDir/allure-report/history")) {
        # Backup history
        $tempHistory = "$reportsDir/history_backup_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
        Copy-Item -Recurse -Force "$reportsDir/allure-report/history" $tempHistory
        Write-Host "✓ Backed up history to $tempHistory" -ForegroundColor Yellow
    }
    
    Remove-Item -Recurse -Force "$reportsDir/allure-report"
    Write-Host "✓ Removed allure-report" -ForegroundColor Green
}

# Remove temp history
if (Test-Path "$reportsDir/history_temp") {
    Remove-Item -Recurse -Force "$reportsDir/history_temp"
    Write-Host "✓ Removed temp history" -ForegroundColor Green
}

# Optionally clean archive (with confirmation)
if (Test-Path "$reportsDir/archive") {
    if (-not $Force) {
        $archiveConfirm = Read-Host "Also clean archived reports? (y/N)"
        if ($archiveConfirm -eq 'y' -or $archiveConfirm -eq 'Y') {
            Remove-Item -Recurse -Force "$reportsDir/archive"
            New-Item -ItemType Directory -Path "$reportsDir/archive" | Out-Null
            Write-Host "✓ Cleaned archive" -ForegroundColor Green
        }
    } else {
        Remove-Item -Recurse -Force "$reportsDir/archive"
        New-Item -ItemType Directory -Path "$reportsDir/archive" | Out-Null
        Write-Host "✓ Cleaned archive" -ForegroundColor Green
    }
}

Write-Host "Reports cleanup completed!" -ForegroundColor Green
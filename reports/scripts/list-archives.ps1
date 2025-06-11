# List Archives Script
# Shows all archived reports with metadata

param(
    [switch]$Detailed,
    [int]$Last = 0
)

$reportsDir = "reports"
$archiveDir = "$reportsDir\archive"

if (-not (Test-Path $archiveDir)) {
    Write-Host "❌ No archive directory found" -ForegroundColor Red
    exit 1
}

$archives = Get-ChildItem -Directory $archiveDir | Sort-Object Name -Descending

if ($archives.Count -eq 0) {
    Write-Host "📁 No archived reports found" -ForegroundColor Yellow
    exit 0
}

# Limit results if -Last parameter is specified
if ($Last -gt 0) {
    $archives = $archives | Select-Object -First $Last
}

Write-Host "📊 Archived Reports ($($archives.Count) found)" -ForegroundColor Blue
Write-Host "=" * 60

foreach ($archive in $archives) {
    $metadataPath = Join-Path $archive.FullName "metadata.json"
    
    if ($Detailed -and (Test-Path $metadataPath)) {
        $metadata = Get-Content $metadataPath | ConvertFrom-Json
        
        Write-Host "📁 $($archive.Name)" -ForegroundColor Cyan
        Write-Host "   Timestamp: $($metadata.Timestamp)" -ForegroundColor Gray
        Write-Host "   Build: $($metadata.BuildNumber)" -ForegroundColor Gray
        Write-Host "   Branch: $($metadata.Branch)" -ForegroundColor Gray
        Write-Host "   Created by: $($metadata.CreatedBy)" -ForegroundColor Gray
        
        # Calculate size
        $size = (Get-ChildItem -Recurse $archive.FullName | Measure-Object -Property Length -Sum).Sum
        $sizeMB = [math]::Round($size / 1MB, 2)
        Write-Host "   Size: $sizeMB MB" -ForegroundColor Gray
        
        # Check contents
        $hasReport = Test-Path (Join-Path $archive.FullName "allure-report")
        $hasResults = Test-Path (Join-Path $archive.FullName "allure-results")
        $contents = @()
        if ($hasReport) { $contents += "report" }
        if ($hasResults) { $contents += "results" }
        Write-Host "   Contents: $($contents -join ', ')" -ForegroundColor Gray
        
    } else {
        # Simple listing
        $size = (Get-ChildItem -Recurse $archive.FullName | Measure-Object -Property Length -Sum).Sum
        $sizeMB = [math]::Round($size / 1MB, 2)
        
        Write-Host "📁 $($archive.Name) ($sizeMB MB)" -ForegroundColor Cyan
    }
    
    Write-Host ""
}

# Show total archive size
$totalSize = (Get-ChildItem -Recurse $archiveDir | Measure-Object -Property Length -Sum).Sum
$totalSizeMB = [math]::Round($totalSize / 1MB, 2)
Write-Host "💾 Total archive size: $totalSizeMB MB" -ForegroundColor Blue
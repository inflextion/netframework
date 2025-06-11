# Open Report Script
# Opens Allure reports in browser with a simple web server

param(
    [string]$Archive = "",
    [int]$Port = 5000
)

$reportsDir = "reports"

# Determine which report to open
if ($Archive) {
    # Open archived report
    $reportPath = "$reportsDir\archive\$Archive\allure-report"
    if (-not (Test-Path $reportPath)) {
        Write-Host "❌ Archive '$Archive' not found or has no report" -ForegroundColor Red
        Write-Host "Available archives:" -ForegroundColor Yellow
        Get-ChildItem -Directory "$reportsDir\archive" | ForEach-Object { 
            Write-Host "  - $($_.Name)" -ForegroundColor Cyan
        }
        exit 1
    }
    Write-Host "🗂️  Opening archived report: $Archive" -ForegroundColor Blue
} else {
    # Open current report
    $reportPath = "$reportsDir\allure-report"
    if (-not (Test-Path $reportPath)) {
        Write-Host "❌ No current report found. Run tests first or specify an archive." -ForegroundColor Red
        exit 1
    }
    Write-Host "📊 Opening current report" -ForegroundColor Blue
}

# Check if port is available
$portInUse = Get-NetTCPConnection -LocalPort $Port -ErrorAction SilentlyContinue
if ($portInUse) {
    Write-Host "⚠️  Port $Port is already in use. Trying port $($Port + 1)..." -ForegroundColor Yellow
    $Port = $Port + 1
}

Write-Host "🌐 Starting web server on http://localhost:$Port" -ForegroundColor Green
Write-Host "📂 Serving from: $reportPath" -ForegroundColor Gray
Write-Host "Press Ctrl+C to stop the server" -ForegroundColor Yellow

# Change to report directory and start server
Push-Location $reportPath

try {
    # Try to open browser automatically
    Start-Process "http://localhost:$Port"
    
    # Start the server
    npx serve -s . -l $Port
} finally {
    Pop-Location
}
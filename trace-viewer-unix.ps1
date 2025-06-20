# Playwright Trace Viewer Script (Unix/Mac/Linux)
# Opens Playwright trace files for debugging test execution

param(
    [string]$TraceName,
    [string]$TracesDir = "bin/Debug/net9.0/Traces",
    [switch]$List,
    [switch]$Help
)

# Show help information
if ($Help) {
    Write-Host "Playwright Trace Viewer Script" -ForegroundColor Blue
    Write-Host "==============================" -ForegroundColor Blue
    Write-Host ""
    Write-Host "Usage:" -ForegroundColor Green
    Write-Host "  ./trace-viewer-unix.ps1 [TRACE_NAME] [OPTIONS]" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Parameters:" -ForegroundColor Green
    Write-Host "  TraceName      Name of trace file (without .zip extension)" -ForegroundColor White
    Write-Host "  -TracesDir     Directory containing trace files (default: bin/Debug/net9.0/Traces)" -ForegroundColor White
    Write-Host "  -List          List all available trace files" -ForegroundColor White
    Write-Host "  -Help          Show this help message" -ForegroundColor White
    Write-Host ""
    Write-Host "Examples:" -ForegroundColor Green
    Write-Host "  powershell ./trace-viewer-unix.ps1 BaseUrlLaunchTest-trace" -ForegroundColor Cyan
    Write-Host "  powershell ./trace-viewer-unix.ps1 -List" -ForegroundColor Cyan
    Write-Host "  powershell ./trace-viewer-unix.ps1 MyTest-trace -TracesDir custom/traces" -ForegroundColor Cyan
    Write-Host ""
    exit 0
}

# Check if traces directory exists
if (-not (Test-Path $TracesDir)) {
    Write-Host "ERROR: Traces directory not found: $TracesDir" -ForegroundColor Red
    Write-Host ""
    Write-Host "Tips:" -ForegroundColor Yellow
    Write-Host "  1. Make sure you've run some UI tests with tracing enabled" -ForegroundColor White
    Write-Host "  2. Check that EnableTracing is set to true in appsettings.json" -ForegroundColor White
    Write-Host "  3. Or enable tracing per test: LaunchBrowserAsync(browser, enableTracing: true)" -ForegroundColor White
    exit 1
}

# List available trace files
if ($List) {
    Write-Host "Available trace files in: $TracesDir" -ForegroundColor Blue
    Write-Host ""
    
    $traceFiles = Get-ChildItem -Path $TracesDir -Filter "*.zip" | Sort-Object LastWriteTime -Descending
    
    if ($traceFiles.Count -eq 0) {
        Write-Host "No trace files found." -ForegroundColor Yellow
        Write-Host ""
        Write-Host "Tip: Run some UI tests with tracing enabled to generate trace files." -ForegroundColor Gray
    } else {
        $traceFiles | ForEach-Object {
            $size = [math]::Round($_.Length / 1MB, 2)
            $name = $_.BaseName
            Write-Host "  * $name" -ForegroundColor Green
            Write-Host "     Size: ${size}MB, Modified: $($_.LastWriteTime)" -ForegroundColor Gray
            Write-Host ""
        }
        
        Write-Host "Usage: powershell ./trace-viewer-unix.ps1 <trace-name>" -ForegroundColor Cyan
    }
    exit 0
}

# If no trace name provided, show available files
if (-not $TraceName) {
    Write-Host "ERROR: Please specify a trace name or use -List to see available traces" -ForegroundColor Red
    Write-Host ""
    Write-Host "Usage: powershell ./trace-viewer-unix.ps1 <trace-name>" -ForegroundColor Cyan
    Write-Host "       powershell ./trace-viewer-unix.ps1 -List" -ForegroundColor Cyan
    exit 1
}

# Ensure .zip extension
if (-not $TraceName.EndsWith("-trace.zip")) {
    if (-not $TraceName.EndsWith("-trace")) {
        $TraceName += "-trace"
    }
    $TraceName += ".zip"
}

# Build full trace path
$tracePath = Join-Path $TracesDir $TraceName

# Check if trace file exists
if (-not (Test-Path $tracePath)) {
    Write-Host "ERROR: Trace file not found: $tracePath" -ForegroundColor Red
    Write-Host ""
    Write-Host "Available trace files:" -ForegroundColor Yellow
    Get-ChildItem -Path $TracesDir -Filter "*.zip" | ForEach-Object {
        Write-Host "  * $($_.BaseName)" -ForegroundColor Green
    }
    exit 1
}

# Check if Playwright is available
$playwrightPath = "bin/Debug/net9.0/playwright.ps1"
if (-not (Test-Path $playwrightPath)) {
    Write-Host "ERROR: Playwright not found at: $playwrightPath" -ForegroundColor Red
    Write-Host ""
    Write-Host "Try building the project first:" -ForegroundColor Yellow
    Write-Host "   dotnet build" -ForegroundColor Cyan
    exit 1
}

# Launch trace viewer
Write-Host "Opening Playwright Trace Viewer..." -ForegroundColor Blue
Write-Host "Trace file: $tracePath" -ForegroundColor Gray
Write-Host ""

try {
    & powershell $playwrightPath show-trace $tracePath
} catch {
    Write-Host "ERROR: Failed to open trace viewer: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Troubleshooting:" -ForegroundColor Yellow
    Write-Host "  1. Ensure Playwright browsers are installed: powershell $playwrightPath install" -ForegroundColor White
    Write-Host "  2. Try running: dotnet build" -ForegroundColor White
    Write-Host "  3. Check if PowerShell is available: powershell --version" -ForegroundColor White
    exit 1
}

Write-Host ""
Write-Host "SUCCESS: Trace viewer opened successfully!" -ForegroundColor Green
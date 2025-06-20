# Playwright Codegen Script
# Launches Playwright UI recorder for test generation

param(
    [string]$Url = "http://localhost:3000/form",
    [string]$Browser = "chromium",
    [string]$Device = "",
    [string]$ViewportSize = "",
    [string]$LoadStorage = "",
    [string]$SaveStorage = "",
    [switch]$Mobile,
    [switch]$Help
)

# Show help information
if ($Help) {
    Write-Host "Playwright Codegen Script" -ForegroundColor Blue
    Write-Host "=========================" -ForegroundColor Blue
    Write-Host ""
    Write-Host "Usage:" -ForegroundColor Green
    Write-Host "  ./codegen.ps1 [URL] [OPTIONS]" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Parameters:" -ForegroundColor Green
    Write-Host "  -Url           Target URL to start recording (optional)" -ForegroundColor White
    Write-Host "  -Browser       Browser to use (chromium, firefox, webkit)" -ForegroundColor White
    Write-Host "  -Device        Device to emulate (e.g., 'iPhone 12', 'Pixel 5')" -ForegroundColor White
    Write-Host "  -ViewportSize  Viewport size (e.g., '1920,1080')" -ForegroundColor White
    Write-Host "  -LoadStorage   Load authentication from file" -ForegroundColor White
    Write-Host "  -SaveStorage   Save authentication to file" -ForegroundColor White
    Write-Host "  -Mobile        Use mobile viewport (375x667)" -ForegroundColor White
    Write-Host "  -Help          Show this help message" -ForegroundColor White
    Write-Host ""
    Write-Host "Examples:" -ForegroundColor Green
    Write-Host "  ./codegen.ps1" -ForegroundColor Cyan
    Write-Host "  ./codegen.ps1 -Url https://example.com" -ForegroundColor Cyan
    Write-Host "  ./codegen.ps1 -Url https://example.com -Browser firefox" -ForegroundColor Cyan
    Write-Host "  ./codegen.ps1 -Device 'iPhone 12'" -ForegroundColor Cyan
    Write-Host "  ./codegen.ps1 -Mobile" -ForegroundColor Cyan
    Write-Host "  ./codegen.ps1 -LoadStorage auth.json" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Common devices:" -ForegroundColor Green
    Write-Host "  'iPhone 12', 'iPhone 13', 'Pixel 5', 'Galaxy S21'" -ForegroundColor Gray
    Write-Host "  'iPad', 'Desktop Chrome', 'Desktop Firefox'" -ForegroundColor Gray
    exit 0
}

# Ensure project is built
Write-Host "🔧 Building project..." -ForegroundColor Blue
dotnet build --configuration Debug

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed. Please fix build errors before running codegen." -ForegroundColor Red
    exit 1
}

# Check if Playwright is installed
$playwrightPath = "bin/Debug/net9.0/playwright.ps1"
if (-not (Test-Path $playwrightPath)) {
    Write-Host "❌ Playwright not found. Installing browsers..." -ForegroundColor Red
    pwsh bin/Debug/net9.0/playwright.ps1 install
}

# Build the codegen command
$command = @("pwsh", $playwrightPath, "codegen")

# Add browser parameter
if ($Browser -ne "chromium") {
    $command += "--browser=$Browser"
}

# Add device emulation
if ($Device) {
    $command += "--device=$Device"
    Write-Host "📱 Using device: $Device" -ForegroundColor Cyan
} elseif ($Mobile) {
    $command += "--viewport-size=375,667"
    Write-Host "📱 Using mobile viewport: 375x667" -ForegroundColor Cyan
} elseif ($ViewportSize) {
    $command += "--viewport-size=$ViewportSize"
    Write-Host "🖥️  Using viewport: $ViewportSize" -ForegroundColor Cyan
}

# Add storage options
if ($LoadStorage) {
    if (Test-Path $LoadStorage) {
        $command += "--load-storage=$LoadStorage"
        Write-Host "🔑 Loading authentication from: $LoadStorage" -ForegroundColor Yellow
    } else {
        Write-Host "⚠️  Storage file not found: $LoadStorage" -ForegroundColor Yellow
    }
}

if ($SaveStorage) {
    $command += "--save-storage=$SaveStorage"
    Write-Host "💾 Will save authentication to: $SaveStorage" -ForegroundColor Yellow
}

# Add target language
$command += "--target=csharp"

# Add URL if provided
if ($Url) {
    $command += $Url
    Write-Host "🌐 Starting with URL: $Url" -ForegroundColor Green
}

# Display launch information
Write-Host ""
Write-Host "🎭 Launching Playwright Codegen..." -ForegroundColor Blue
Write-Host "Browser: $Browser" -ForegroundColor Gray
if ($Url) { Write-Host "URL: $Url" -ForegroundColor Gray }
Write-Host ""
Write-Host "📝 Instructions:" -ForegroundColor Green
Write-Host "  1. Record your interactions in the browser" -ForegroundColor White
Write-Host "  2. Copy generated C# code from the recorder window" -ForegroundColor White
Write-Host "  3. Adapt code to use your BaseUiTest and page objects" -ForegroundColor White
Write-Host "  4. Add assertions and test structure" -ForegroundColor White
Write-Host ""
Write-Host "💡 Tip: Use meaningful selectors and avoid generated data-* attributes when possible" -ForegroundColor Yellow
Write-Host ""

# Execute the command
try {
    & $command[0] $command[1..($command.Length-1)]
} catch {
    Write-Host "❌ Failed to launch codegen: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "🔧 Troubleshooting:" -ForegroundColor Yellow
    Write-Host "  1. Ensure Playwright browsers are installed: pwsh bin/Debug/net9.0/playwright.ps1 install" -ForegroundColor White
    Write-Host "  2. Try running: dotnet build" -ForegroundColor White
    Write-Host "  3. Check if PowerShell is available: pwsh --version" -ForegroundColor White
    exit 1
}

Write-Host ""
Write-Host "✅ Codegen session completed!" -ForegroundColor Green
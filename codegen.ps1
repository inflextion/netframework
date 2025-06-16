# Playwright Codegen Script for Windows
# Launches Playwright UI recorder for test generation

param(
    [string]$Url = "",
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
    Write-Host "  .\codegen.ps1 [URL] [OPTIONS]" -ForegroundColor Cyan
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
    Write-Host "  .\codegen.ps1" -ForegroundColor Cyan
    Write-Host "  .\codegen.ps1 -Url https://example.com" -ForegroundColor Cyan
    Write-Host "  .\codegen.ps1 -Url https://example.com -Browser firefox" -ForegroundColor Cyan
    Write-Host "  .\codegen.ps1 -Device 'iPhone 12'" -ForegroundColor Cyan
    Write-Host "  .\codegen.ps1 -Mobile" -ForegroundColor Cyan
    Write-Host "  .\codegen.ps1 -LoadStorage auth.json" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Common devices:" -ForegroundColor Green
    Write-Host "  'iPhone 12', 'iPhone 13', 'Pixel 5', 'Galaxy S21'" -ForegroundColor Gray
    Write-Host "  'iPad', 'Desktop Chrome', 'Desktop Firefox'" -ForegroundColor Gray
    exit 0
}

# Ensure project is built
Write-Host "[BUILD] Building project..." -ForegroundColor Blue
dotnet build --configuration Debug

if ($LASTEXITCODE -ne 0) {
    Write-Host "[ERROR] Build failed. Please fix build errors before running codegen." -ForegroundColor Red
    exit 1
}

# Check if Playwright is installed
$playwrightPath = "bin\Debug\net9.0\playwright.ps1"
if (-not (Test-Path $playwrightPath)) {
    Write-Host "[WARNING] Playwright script not found at: $playwrightPath" -ForegroundColor Yellow
    Write-Host "[INFO] Looking for Playwright in other locations..." -ForegroundColor Yellow
    
    # Try to find playwright.ps1 in common locations
    $alternativePaths = @(
        "bin\Debug\net8.0\playwright.ps1",
        "bin\Debug\net7.0\playwright.ps1",
        "bin\Debug\net6.0\playwright.ps1"
    )
    
    foreach ($path in $alternativePaths) {
        if (Test-Path $path) {
            $playwrightPath = $path
            Write-Host "[OK] Found Playwright at: $playwrightPath" -ForegroundColor Green
            break
        }
    }
    
    if (-not (Test-Path $playwrightPath)) {
        Write-Host "[ERROR] Could not find playwright.ps1 in any expected location" -ForegroundColor Red
        Write-Host "Please ensure Microsoft.Playwright NuGet package is installed" -ForegroundColor Yellow
        exit 1
    }
}

# Install browsers if needed
try {
    # Convert to absolute path for reliability
    $fullPlaywrightPath = Resolve-Path $playwrightPath
    
    # Check if PowerShell Core is available
    $pwshAvailable = Get-Command pwsh -ErrorAction SilentlyContinue
    
    if ($pwshAvailable) {
        Write-Host "[INFO] Using PowerShell Core (pwsh)" -ForegroundColor Cyan
        $powerShellExe = "pwsh"
    } else {
        Write-Host "[INFO] Using Windows PowerShell" -ForegroundColor Cyan
        $powerShellExe = "powershell"
    }
    
    # Build the codegen command
    $arguments = @($fullPlaywrightPath, "codegen")
    
    # Add browser parameter
    if ($Browser -ne "chromium") {
        $arguments += "--browser=$Browser"
    }
    
    # Add device emulation
    if ($Device) {
        $arguments += "--device=$Device"
        Write-Host "[DEVICE] Using device: $Device" -ForegroundColor Cyan
    } elseif ($Mobile) {
        $arguments += "--viewport-size=375,667"
        Write-Host "[MOBILE] Using mobile viewport: 375x667" -ForegroundColor Cyan
    } elseif ($ViewportSize) {
        $arguments += "--viewport-size=$ViewportSize"
        Write-Host "[VIEWPORT] Using viewport: $ViewportSize" -ForegroundColor Cyan
    }
    
    # Add storage options
    if ($LoadStorage) {
        if (Test-Path $LoadStorage) {
            $fullStoragePath = Resolve-Path $LoadStorage
            $arguments += "--load-storage=$fullStoragePath"
            Write-Host "[AUTH] Loading authentication from: $LoadStorage" -ForegroundColor Yellow
        } else {
            Write-Host "[WARNING] Storage file not found: $LoadStorage" -ForegroundColor Yellow
        }
    }
    
    if ($SaveStorage) {
        $arguments += "--save-storage=$SaveStorage"
        Write-Host "[SAVE] Will save authentication to: $SaveStorage" -ForegroundColor Yellow
    }
    
    # Add target language
    $arguments += "--target=csharp"
    
    # Add URL if provided
    if ($Url) {
        $arguments += $Url
        Write-Host "[URL] Starting with URL: $Url" -ForegroundColor Green
    }
    
    # Display launch information
    Write-Host ""
    Write-Host "[LAUNCH] Launching Playwright Codegen..." -ForegroundColor Blue
    Write-Host "Browser: $Browser" -ForegroundColor Gray
    if ($Url) { Write-Host "URL: $Url" -ForegroundColor Gray }
    Write-Host ""
    Write-Host "[INSTRUCTIONS]" -ForegroundColor Green
    Write-Host "  1. Record your interactions in the browser" -ForegroundColor White
    Write-Host "  2. Copy generated C# code from the recorder window" -ForegroundColor White
    Write-Host "  3. Adapt code to use your BaseUiTest and page objects" -ForegroundColor White
    Write-Host "  4. Add assertions and test structure" -ForegroundColor White
    Write-Host ""
    Write-Host "[TIP] Use meaningful selectors and avoid generated data-* attributes when possible" -ForegroundColor Yellow
    Write-Host ""
    
    # Execute the command
    & $powerShellExe -ExecutionPolicy Bypass -File $arguments[0] $arguments[1..($arguments.Length-1)]
    
} catch {
    Write-Host "[ERROR] Failed to launch codegen: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "[TROUBLESHOOTING]" -ForegroundColor Yellow
    Write-Host "  1. Ensure Playwright browsers are installed:" -ForegroundColor White
    Write-Host "     powershell -ExecutionPolicy Bypass -File $playwrightPath install" -ForegroundColor Gray
    Write-Host "  2. Try running: dotnet build" -ForegroundColor White
    Write-Host "  3. Check .NET version: dotnet --version" -ForegroundColor White
    Write-Host "  4. Ensure Microsoft.Playwright NuGet package is installed" -ForegroundColor White
    Write-Host "  5. Try running with administrator privileges" -ForegroundColor White
    exit 1
}

Write-Host ""
Write-Host "[SUCCESS] Codegen session completed!" -ForegroundColor Green
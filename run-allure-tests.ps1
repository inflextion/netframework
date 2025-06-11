# Variables for paths
$reportsDir = "reports"
$reportHistory = "$reportsDir\allure-report\history"
$tempHistory = "$reportsDir\history_temp"
$rootResults = "$reportsDir\allure-results"
$allureReport = "$reportsDir\allure-report"

# Step 0: Create reports directory if it doesn't exist
if (-not (Test-Path $reportsDir)) {
    New-Item -ItemType Directory -Path $reportsDir | Out-Null
}

# Step 1: (Before cleaning) Backup history if it exists
if (Test-Path $reportHistory) {
    Remove-Item -Recurse -Force $tempHistory -ErrorAction SilentlyContinue
    Copy-Item -Recurse -Force $reportHistory $tempHistory
}

# Step 2: Clean old results and report
Remove-Item -Recurse -Force $rootResults -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force $allureReport -ErrorAction SilentlyContinue

# Step 3: Run your xUnit tests (no logger for Allure)
dotnet test atf.csproj --filter Category=Smoke

# Step 4: Move allure-results from build output to root
$buildResults = "bin\Debug\net9.0\allure-results"
if (Test-Path $buildResults) {
    Move-Item -Force -Path $buildResults -Destination $rootResults
}

# Step 5: Restore history from backup to new results
$destHistory = "$rootResults\history"
if (Test-Path $tempHistory) {
    if (-not (Test-Path $destHistory)) {
        New-Item -ItemType Directory -Path $destHistory | Out-Null
    }
    Copy-Item -Recurse -Force "$tempHistory\*" $destHistory
    Remove-Item -Recurse -Force $tempHistory -ErrorAction SilentlyContinue
}

# Step 6: Generate the Allure report in reports folder
allure generate $rootResults -o $allureReport --clean

# Step 7: Launch the webserver with npx from the allure-report folder (then manually open the browser on http://localhost:5000)
cd ".\$allureReport\"
npx serve -s . -l 5000


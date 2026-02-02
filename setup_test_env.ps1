$docPath = [Environment]::GetFolderPath("MyDocuments")
$appFolder = Join-Path $docPath "Feedback-Flow"

# 1. Create App Folder
if (!(Test-Path $appFolder)) {
    New-Item -ItemType Directory -Force -Path $appFolder | Out-Null
    Write-Host "Created folder: $appFolder"
}

# 2. Create Dummy CSV
$csvPath = Join-Path $appFolder "students.csv"
$csvContent = @"
Full Name,Email,Assigned Material
Jane Doe,jane.doe@example.com,
John Smith,john.smith@data.com,
Maria Garcia,maria.garcia@test.org,
"@

if (!(Test-Path $csvPath)) {
    $csvContent | Out-File -FilePath $csvPath -Encoding utf8
    Write-Host "Created dummy CSV: $csvPath"
} else {
    Write-Host "CSV already exists: $csvPath"
}

# 3. Create Daily Folder & Dummy Notes
$today = Get-Date -Format "yyyyMMdd"
$todayFolder = Join-Path $appFolder $today
if (!(Test-Path $todayFolder)) {
    New-Item -ItemType Directory -Force -Path $todayFolder | Out-Null
}

# Setup dummy note for Jane Doe
$janeFolder = Join-Path $todayFolder "Jane-Doe"
if (!(Test-Path $janeFolder)) {
    New-Item -ItemType Directory -Force -Path $janeFolder | Out-Null
}
$janeNote = Join-Path $janeFolder "feedback-jane-doe-$today.txt"
"Jane has shown excellent progress in C# specific topics." | Out-File -FilePath $janeNote -Encoding utf8
Write-Host "Created test note for Jane Doe at: $janeNote"

# Setup dummy note for John Smith
$johnFolder = Join-Path $todayFolder "John-Smith"
if (!(Test-Path $johnFolder)) {
    New-Item -ItemType Directory -Force -Path $johnFolder | Out-Null
}
$johnNote = Join-Path $johnFolder "feedback-john-smith-$today.txt"
"John needs to work on SOLID principles a bit more." | Out-File -FilePath $johnNote -Encoding utf8
Write-Host "Created test note for John Smith at: $johnNote"

# Setup dummy note for Maria Garcia
$mariaFolder = Join-Path $todayFolder "Maria-Garcia"
if (!(Test-Path $mariaFolder)) {
    New-Item -ItemType Directory -Force -Path $mariaFolder | Out-Null
}
$mariaNote = Join-Path $mariaFolder "feedback-maria-garcia-$today.txt"
"Maria demonstrates strong understanding of async/await patterns and dependency injection." | Out-File -FilePath $mariaNote -Encoding utf8
Write-Host "Created test note for Maria Garcia at: $mariaNote"

Write-Host "Setup Complete. You can now run the Feedback Flow app."

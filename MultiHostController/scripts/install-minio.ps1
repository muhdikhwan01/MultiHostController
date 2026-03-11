Write-Host "Downloading MinIO..."

Invoke-WebRequest `
 https://dl.min.io/server/minio/release/windows-amd64/minio.exe `
 -OutFile minio.exe

Write-Host "MinIO download completed."
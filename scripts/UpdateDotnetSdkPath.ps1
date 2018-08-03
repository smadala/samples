dotnet --version; 
$path = Get-ChildItem Env:path; 
Write-Host $path.Value; 
$pathValue = $path.Value -Replace "C:\\Program Files\\dotnet","C:\dotnet"; 
Write-Host $pathValue; 
$env:Path = $pathValue; 
dotnet --version
dotnet NETCoreSampleUnitTests\MSTestV2Tests\MSTestV2Tests.csproj --logger trx --collect "Code coverage"

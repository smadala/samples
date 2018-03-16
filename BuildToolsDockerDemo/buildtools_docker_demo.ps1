

#$ContainerId = "fb5ab8ed56a9"
$ContainerId = "bb51fe78250cdab37b28dc5084c878d29e9fefc7079af45a98bf3fa729cf24fa"
$VSTestConsolePath = "C:\BuildTools\Common7\IDE\Extensions\TestPlatform\vstest.console.exe"
$TestAssembly = "c:\Docker\data\samples\BuildToolsDockerDemo\MSTestv2Project\bin\Debug\MSTestv2Project.dll"
$HostResultsDirectory = "D:\docker\data\TestResults-3\"
$ResultsDirectoryArg = "/ResultsDirectory:C:\docker\data\TestResults-3\"

if(Test-Path $HostResultsDirectory){
    Remove-Item -Force -Recurse -Verbose $HostResultsDirectory
}

$numberOfIterations = 3
$Containers = @()


for($i=1; $i -le $numberOfIterations; $i++){
    Write-Host "Creating container from image buildtools2017:only-managed-15.7-27514.01-v2"
    $ContainerId = docker create -v "D:\Docker\data:C:\Docker\data" buildtools2017:only-managed-15.7-27514.01-v2 

    $Containers += $ContainerId

    Write-Host "Starting container $ContainerId"
    docker start $ContainerId

    Write-Host "Number of docker containers started: $i"

    Write-Host "Executing: docker exec -d $ContainerId $VSTestConsolePath $TestAssembly $ResultsDirectoryArg /logger:trx /Testcasefilter:priority=$i"
    docker exec -d $ContainerId $VSTestConsolePath $TestAssembly $ResultsDirectoryArg /logger:trx /Testcasefilter:priority=$i
}


#for($i=1; $i -le $numberOfIterations; $i++){
  
#    $ContainerId = $Containers[$i-1]
#    $priority = $i
#    Write-Host "Executing: docker exec -d $ContainerId $VSTestConsolePath $TestAssembly $ResultsDirectoryArg /logger:trx /Testcasefilter:priority=$priority"
#    docker exec -d $ContainerId $VSTestConsolePath $TestAssembly $ResultsDirectoryArg /logger:trx /Testcasefilter:priority=$priority
#}

$CompletedJobs = 0
while($numberOfIterations -ne $CompletedJobs)
{
    Start-Sleep -s 2
    if(Test-Path $HostResultsDirectory)
    {
        $CompletedJobs = Get-ChildItem -Filter *trx $HostResultsDirectory | Measure-Object| %{$_.Count}
        Write-Host "Total Jobs: $numberOfIterations Completed jobs: $CompletedJobs"
    }
    else
    {
        Write-Host "Results directory yet to create."
    }
}

Write-Host "All Jobs completed..."
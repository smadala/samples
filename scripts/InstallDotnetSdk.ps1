[reflection.assembly]::LoadWithPartialName("System.Net.Http") | Out-Null
[reflection.assembly]::LoadWithPartialName("System.Threading") | Out-Null
[reflection.assembly]::LoadWithPartialName("System.IO.Compression.FileSystem") | Out-Null

$SourcePath = "https://dotnetcli.blob.core.windows.net/dotnet/Sdk/2.1.400-preview-008975/dotnet-sdk-2.1.400-preview-008975-win-x64.zip";
$DestinationPath = "C:\dotnet"
$TempPath = [System.IO.Path]::GetTempFileName()

if (($SourcePath -as [System.URI]).AbsoluteURI -ne $null)
{
    $handler = New-Object System.Net.Http.HttpClientHandler
    $client = New-Object System.Net.Http.HttpClient($handler)
    $client.Timeout = New-Object System.TimeSpan(0, 30, 0)
    $cancelTokenSource = New-Object System.Threading.CancellationTokenSource
    $uri = New-Object -TypeName System.Uri $SourcePath
    $responseMsg = $client.GetAsync($uri, $cancelTokenSource.Token)
    $responseMsg.Wait()

    if (!$responseMsg.IsCanceled)
    {
        $response = $responseMsg.Result
        if ($response.IsSuccessStatusCode)
        {
            $fileMode = [System.IO.FileMode]::Create
            $fileAccess = [System.IO.FileAccess]::Write
            $downloadedFileStream = New-Object System.IO.FileStream $TempPath,$fileMode,$fileAccess
            $copyStreamOp = $response.Content.CopyToAsync($downloadedFileStream)
            $copyStreamOp.Wait()
            $downloadedFileStream.Close()

            if ($copyStreamOp.Exception -ne $null)
            {
                throw $copyStreamOp.Exception
            }
        }
    }
}
else
{
    throw "Cannot copy from $SourcePath"
}

[System.IO.Compression.ZipFile]::ExtractToDirectory($TempPath, $DestinationPath)
Remove-Item $TempPath
# Use the latest Windows Server Core image.
FROM microsoft/windowsservercore

# Download useful tools to C:\Bin.
ADD https://dist.nuget.org/win-x86-commandline/v4.1.0/nuget.exe C:\\Bin\\nuget.exe

# RUN net use z: 

# Copy \\vspreinstall\PREINSTALL content to Dockerfile directory. To run StrongName hijack.
ADD preinstall C:\\TEMP\\preinstall

# Run Strong-name hijack.
RUN C:\TEMP\preinstall\preinstall.cmd


# Download the Build Tools bootstrapper outside of the PATH.
# Copy \\cpvsbuild\drops\VS-Val\d15.7stg\products\27414.9000.samadala.180214.015235\bootstrappers\BuildTools\vs_buildtools.exe C:\\TEMP\\vs_buildtools.exe
ADD Installer C:\\TEMP\\Installer

# Add C:\Bin to PATH and install Build Tools excluding workloads and components with known issues.
RUN setx /m PATH "%PATH%;C:\Bin" \
 && C:\TEMP\Installer\vs_buildtools.exe --quiet --wait --norestart --nocache --installPath C:\BuildTools --add Microsoft.VisualStudio.Workload.ManagedDesktopBuildTools --includeRecommended \
 || IF "%ERRORLEVEL%"=="3010" EXIT 0

# Start developer command prompt with any other commands specified.
ENTRYPOINT C:\BuildTools\Common7\Tools\VsDevCmd.bat &&

# Default to PowerShell if no other command specified.
CMD ["powershell.exe", "-NoLogo", "-ExecutionPolicy", "Bypass"]
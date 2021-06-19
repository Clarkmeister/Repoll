# Repoll
A service that automatically keeps selected projects up to date with git repositories on system startup.

Use Case: If a person frequently works on a project using multiple different computers this app can help keep your projects up to date with your repos.

# Windows Setup Instructions:

1. Open solution in Visual Studio and Install NewtonSoft.Json package from Nuget if errors show in solution.
2. Build Project
3. Run as Administrator InstallService.bat (Path to VsDevCmd.bat may need to be changed.)
4. Run as Administrator Visual Studio with Repoll Client as Startup Project.

# Windows Uninstall Instructions:

1. Run as Administrator UninstallService.bat (Path to VsDevCmd.bat may need to be changed.)

*Administrator rights needed to start/stop/install/uninstall Windows Service

# Notes (Windows)

1. Service runs in Automatic Delayed Start mode, this means that the service will will take 2 minutes to run after system startup.

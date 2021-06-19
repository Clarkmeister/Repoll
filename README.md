# Repoll
A service that automatically keeps selected projects up to date with git repositories on system startup.

Use Case: If a person frequently work on a project using multiple different computers this app can help keep your projects up to date with your repos.

# Windows Setup Instructions:

1. Build Project
2. Run (As Administrator) InstallService.bat
3. Run (As Administrator) WPFRepollClient.exe from WPFRepollClient bin directories or Run from Visual Studio (As Administrator)

# Windows Uninstall Instructions:

1. Run (As Administrator) UninstallService.bat

*Administrator rights needed to start/stop/install/uninstall Windows Service

# Notes (Windows)

1. Service runs in Automatic Delayed Start mode, this means that the service will will take 2 minutes to run after system startup.
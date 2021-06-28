call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\Tools\VsDevCmd.bat"
installutil -u "%~dp0RepollService\bin\Debug\RepollService.exe"
echo "Deleting Repoll Files..."
echo "Deleting: C:\ProgramData\Repoll\repos.json"
@RD /S /Q "C:\ProgramData\Repoll\"
echo "Completed - Press any key to exit..."
PAUSE